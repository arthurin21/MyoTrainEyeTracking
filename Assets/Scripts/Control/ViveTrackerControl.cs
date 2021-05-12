using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveTrackerControl : MonoBehaviour
{
    public bool trackShoulder = true;
    public bool trackElbow = true;
    public bool trackWrist = false;

    private GameObject trackerTrunk;
    private GameObject trackerUpperArm;
    private GameObject trackerForearm;
    private GameObject trackerHand;

    private vMPLMovementArbiter arbiter = null;

    private bool trunkInitialized = false;
    private bool upperArmInitialized = false;
    private bool forearmInitialized = false;
    private bool handInitialized = false;

    Quaternion initialTrunkRotation;
    Quaternion initialUpperArmRotation;
    Quaternion initialForearmRotation;
    Quaternion initialHandRotation;

    // Start is called before the first frame update
    void Start()
    {
        if ( trackShoulder ) {
            trackerTrunk = GameObject.Find( "Tracker_Trunk" );
            initialTrunkRotation = trackerTrunk.transform.rotation;
        }

        if ( trackShoulder || trackElbow ) {
            trackerUpperArm = GameObject.Find( "Tracker_UpperArm" );
            initialUpperArmRotation = trackerUpperArm.transform.rotation;
        }

        if ( trackElbow || trackWrist ) {
            trackerForearm = GameObject.Find( "Tracker_Forearm" );
            initialForearmRotation = trackerForearm.transform.rotation;
        }

        if ( trackWrist ) {
            trackerHand = GameObject.Find( "Tracker_Hand" );
            initialHandRotation = trackerHand.transform.rotation;
        }

        arbiter = GameObject.Find("vMPLMovementArbiter").GetComponent<vMPLMovementArbiter>();
    }

    // Update is called once per frame
    void Update()
    {
        // initialize trunk orientation
        if ( trackShoulder && !trunkInitialized ) {
            if ( trackerTrunk.transform.rotation != initialTrunkRotation ) {
                initialTrunkRotation = trackerTrunk.transform.rotation;
                trunkInitialized = true;
            }
        }

        // initialize upper arm orientation
        if ( ( trackShoulder || trackElbow ) && !upperArmInitialized ) {
            if ( trackerUpperArm.transform.rotation != initialUpperArmRotation ) {
                initialUpperArmRotation = trackerUpperArm.transform.rotation;
                upperArmInitialized = true;
            }
        }

        // initialize forearm orientation
        if ( ( trackElbow || trackWrist ) && !forearmInitialized ) {
            if ( trackerForearm.transform.rotation != initialForearmRotation ) {
                initialForearmRotation = trackerForearm.transform.rotation;
                forearmInitialized = true;
            }
        }

        // initialize hand orientation
        if ( trackWrist && !handInitialized ) {
            if ( trackerHand.transform.rotation != initialHandRotation ) {
                initialHandRotation = trackerHand.transform.rotation;
                handInitialized = true;
            }
        }

        float [] joint_angles = arbiter.GetRightUpperArmAngles();
        
        // compute shoulder angles
        if ( trackShoulder && trunkInitialized && upperArmInitialized ) {
            Quaternion currentTrunkRotation = Quaternion.Inverse( initialTrunkRotation ) * trackerTrunk.transform.rotation;
            Quaternion currentUpperArmRotation = Quaternion.Inverse( initialUpperArmRotation ) * trackerUpperArm.transform.rotation;
            
            Quaternion relativeShoulder = Quaternion.Inverse( currentUpperArmRotation ) * currentTrunkRotation;
            Vector3 shoulderAngles = relativeShoulder.eulerAngles;

            // joint_angles[0] =  shoulderAngles[1];
            // joint_angles[1] = shoulderAngles[2];
            // joint_angles[2] = -shoulderAngles[0];
        


            joint_angles[0] = shoulderAngles[2];
            joint_angles[1] = -shoulderAngles[1];
            joint_angles[2] = -shoulderAngles[0];
        }

        // compute elbow angle
        if ( trackElbow && upperArmInitialized && forearmInitialized ) {
            Quaternion currentUpperArmRotation = Quaternion.Inverse( initialUpperArmRotation ) * trackerUpperArm.transform.rotation;
            Quaternion currentForearmRotation = Quaternion.Inverse( initialForearmRotation ) * trackerForearm.transform.rotation;
            
            Quaternion relativeElbow = Quaternion.Inverse( currentUpperArmRotation ) * currentForearmRotation;
            Vector3 elbowAngles = relativeElbow.eulerAngles;
            joint_angles[3] = -elbowAngles.z;
        }

        // compute wrist angle
        if ( trackWrist && forearmInitialized && handInitialized ) {
            Quaternion currentForearmRotation = Quaternion.Inverse( initialForearmRotation ) * trackerForearm.transform.rotation;
            Quaternion currentHandRotation = Quaternion.Inverse( initialHandRotation ) * trackerHand.transform.rotation;

            Quaternion relativeWrist = Quaternion.Inverse( currentHandRotation ) * currentForearmRotation;
            Vector3 wristAngles = relativeWrist.eulerAngles;

            // TODO: VERIFY THIS
            joint_angles[4] =  wristAngles[2];
            joint_angles[5] = -wristAngles[1];
            joint_angles[6] = -wristAngles[0];
        }

        arbiter.SetRightUpperArmAngles( joint_angles );
    }
}
