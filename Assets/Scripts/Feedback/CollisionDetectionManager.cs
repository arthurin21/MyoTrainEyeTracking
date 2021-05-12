using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectionManager : MonoBehaviour
{
    List<GameObject> children = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        HingeJoint[] hinges = gameObject.GetComponentsInChildren<HingeJoint>();
        foreach( HingeJoint h in hinges ) {
            h.autoConfigureConnectedAnchor = false;
            GameObject src = h.gameObject;
            GameObject tgt = h.connectedBody.gameObject;

            CollisionDetection detector = (CollisionDetection) tgt.AddComponent<CollisionDetection>();
            detector.connectedHinge = src;
            
            if ( tgt.name.Contains( "PlanetaryAsm" ) ) {
                detector.isSpecial = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 }
