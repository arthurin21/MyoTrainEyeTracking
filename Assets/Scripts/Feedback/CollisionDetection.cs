using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {
    [SerializeField]
    public GameObject connectedHinge;

    [SerializeField]
    public bool isSpecial;
    
    [SerializeField]
    public bool isDebugging;

    HingeJoint joint;
    JointLimits limits;
    float maxAngle;

    // Start is called before the first frame update
    void Start()
    {
        // check special if there are more than 1 hinge joint script on the connectedHinge object
        if (isSpecial) {
            HingeJoint[] _joints= connectedHinge.GetComponents<HingeJoint>();
            for (int i = 0; i < _joints.Length; i++) {
                if (_joints[i].connectedBody.name.Equals(this.gameObject.name)) {
                    joint = _joints[i];
                    break;
                }
            }
        } else {
            joint = connectedHinge.GetComponent<HingeJoint>();
        }

        limits = joint.limits;
        maxAngle = limits.max;

        if (isDebugging) {
            Debug.Log("Actual:" + joint.angle + " Min:" + limits.min + " Max:" + limits.max+"--"+ (maxAngle - limits.min));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag("Grab")) {
            if (isDebugging) {
                Debug.Log("Actual:" + joint.angle + " Min:" + limits.min + " Max:" + limits.max);
            }
            float angle = joint.angle;
            // set actual angle as the maximun angle, so the finger can not close more
            // the angle starts with 0(zero) even if the minimun angle is dif of 0, so this math allows me to calibrate the actual as the max angle
            limits.max = (maxAngle -  limits.min) - ( maxAngle - angle);
            joint.limits = limits;
            if (isDebugging) {
                Debug.Log("novo Actual:" + joint.angle + " Min:" + limits.min + " Max:" + limits.max);
            }
        }
    }

    private void OnCollisionExit(Collision col) {
        if (col.gameObject.CompareTag("Grab")) {
            limits.max = maxAngle;
            joint.limits = limits;
        }
    }
}
