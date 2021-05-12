using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowVRCamera : MonoBehaviour
{
    private bool initialized = false;
    GameObject vrCamera;
    GameObject headJoint;
    GameObject bodyJoint;
    
    GameObject trunkTracker;
    Quaternion initialTrunkRotation;
    Vector3 initialTrunkPosition;

    Quaternion rotationOffset;
    // Vector3 positionOffset;

    // Start is called before the first frame update
    void Start()
    {
        vrCamera = GameObject.Find( "VRCamera" );
        // headJoint = GameObject.Find( "HeadJoint" );
        // bodyJoint = GameObject.Find( "FirstPerson-AIO" );

        trunkTracker = GameObject.Find( "TrackerTrunk" );
        initialTrunkRotation = trunkTracker.transform.rotation;
        initialTrunkPosition = trunkTracker.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ( !initialized ) {
            if ( ( trunkTracker.transform.rotation != initialTrunkRotation )
              && ( trunkTracker.transform.position != initialTrunkPosition ) ) {
                initialTrunkRotation = trunkTracker.transform.rotation;
                initialTrunkPosition = trunkTracker.transform.position;

                rotationOffset = Quaternion.Euler( 0, 90, 0 );
                // positionOffset = vrCamera.transform.position - gameObject.transform.position;
                initialized = true;
            }
        } else {
            // rotate body
            Quaternion currentTrunkRotation = Quaternion.Inverse( initialTrunkRotation ) * trunkTracker.transform.rotation;
            Quaternion restrictedTrunkRotation = Quaternion.Euler( 0, -currentTrunkRotation.eulerAngles.x, 0 );
            gameObject.transform.rotation = restrictedTrunkRotation * rotationOffset;

            // translate body
            // Vector3 currentTrunkPosition = ( trunkTracker.transform.position - initialTrunkPosition );  // position around the trunkTracker origin
            gameObject.transform.position = trunkTracker.transform.position; // currentTrunkPosition + positionOffset;                      // add difference between initialized camera location
        }

        // rotate camera
        // headJoint.transform.rotation = vrCamera.transform.rotation;
    }
}
