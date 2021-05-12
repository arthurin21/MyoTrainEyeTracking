using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : MonoBehaviour
{
    const int NUM_MPL_JOINT_ANGLES = 7;

    int dof = 0;
    float speed = 0.1f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    
    private GameObject hud = null;
    // private GraspingLogicCylinder graspCld = null;
    // private GraspingLogicCard graspCrd = null;
    // private GraspingLogicStick graspStk = null;
    // private GraspingLogicTripod graspTri = null;
    
    private GameObject firstPersonAIO = null;
    private vMPLMovementArbiter arbiter = null;
    private float [] joint_angles = new float[NUM_MPL_JOINT_ANGLES];

    // Start is called before the first frame update
    void Start()
    {
        // grasp = GameObject.Find("CylinderPrimitive").GetComponent<GraspingLogic>();
        hud = GameObject.Find( "HUD" );
        // graspCld = (GraspingLogicCylinder) GameObject.FindObjectOfType<GraspingLogicCylinder>();
        // graspCrd = (GraspingLogicCard) GameObject.FindObjectOfType<GraspingLogicCard>();
        // graspStk = (GraspingLogicStick) GameObject.FindObjectOfType<GraspingLogicStick>();
        // graspTri = (GraspingLogicTripod) GameObject.FindObjectOfType<GraspingLogicTripod>();

        firstPersonAIO = GameObject.Find( "FirstPersion-AIO" );
        arbiter = GameObject.Find("vMPLMovementArbiter").GetComponent<vMPLMovementArbiter>();
    }

    // Update is called once per frame
    void Update()
    {
        // set arm degree of freedom
        if ( Input.GetKey( KeyCode.Alpha1 ) || Input.GetKey( KeyCode.Keypad1 ) ) { 
            Debug.Log( "Controlling Shoulder Flexion/Extension..." );
            dof = 0; 
        }
        else if ( Input.GetKey( KeyCode.Alpha2 ) || Input.GetKey( KeyCode.Keypad2 ) ) { 
            Debug.Log( "Controlling Shoulder Abduction/Adduction..." );
            dof = 1; 
        } 
        else if ( Input.GetKey( KeyCode.Alpha3 ) || Input.GetKey( KeyCode.Keypad3 ) ) { 
            Debug.Log( "Controlling Humeral Internal/External Rotation..." );
            dof = 2; 
        }
        else if ( Input.GetKey( KeyCode.Alpha4 ) || Input.GetKey( KeyCode.Keypad4 ) ) {
            Debug.Log( "Controlling Elbow Flexion/Extension..." );
            dof = 3; 
        }  
        else if ( Input.GetKey( KeyCode.Alpha5 ) || Input.GetKey( KeyCode.Keypad5 ) ) { 
            Debug.Log( "Controlling Wrist Pronation/Supination..." );
            dof = 4; 
        }
        else if ( Input.GetKey( KeyCode.Alpha6 ) || Input.GetKey( KeyCode.Keypad6 ) ) { 
            Debug.Log( "Controlling Wrist Radial/Ulnar Deviation..." );
            dof = 5; 
        }
        else if ( Input.GetKey( KeyCode.Alpha7 ) || Input.GetKey( KeyCode.Keypad7 ) ) { 
            Debug.Log( "Controlling Wrist Flexion/Extension..." );
            dof = 6; 
        }

        // move arm
        joint_angles = arbiter.GetRightUpperArmAngles();
        if ( Input.GetKey( KeyCode.Equals ) || Input.GetKey( KeyCode.KeypadPlus ) ) {
            joint_angles[dof]++;
        } else if ( Input.GetKey( KeyCode.Minus ) || Input.GetKey( KeyCode.KeypadMinus ) ) {
            joint_angles[dof]--;
        }
        

        // Debug.Log( string.Format( "Joint Angles: {0}, {1}, {2}, {3}, {4}, {5}, {6}", joint_angles[0].ToString("F1"), joint_angles[1].ToString("F1"), 
        //                                                                              joint_angles[2].ToString("F1"), joint_angles[3].ToString("F1"), 
        //                                                                              joint_angles[4].ToString("F1"), joint_angles[5].ToString("F1"), 
        //                                                                              joint_angles[6].ToString("F1") ) );
        arbiter.SetRightUpperArmAngles( joint_angles );

        // move camera
        // if ( Input.GetKey( KeyCode.UpArrow ) ) {
        //     Debug.Log( "Moving Forward..." );
        //     Camera.main.transform.Translate( new Vector3( 0, 0, speed * Time.deltaTime ) );
        // } else if ( Input.GetKey( KeyCode.DownArrow ) ) {
        //     Debug.Log( "Moving Backward..." );
        //     Camera.main.transform.Translate( new Vector3( 0, 0, -speed * Time.deltaTime ) );
        // } else if ( Input.GetKey( KeyCode.LeftArrow ) ) {
        //     Debug.Log( "Moving Left..." );
        //     Camera.main.transform.Translate( new Vector3( -speed * Time.deltaTime, 0, 0 ) );
        // } else if ( Input.GetKey( KeyCode.RightArrow ) ) {
        //     Debug.Log( "Moving Right..." );
        //     Camera.main.transform.Translate( new Vector3( speed * Time.deltaTime, 0, 0 ) );
        // }

        // rotate camera
        if ( Input.GetMouseButton( 0 ) ) {
            yaw += Input.GetAxis( "Mouse X" );
            pitch -= Input.GetAxis( "Mouse Y" );
            Camera.main.transform.eulerAngles = new Vector3( pitch, yaw, 0.0f );
            // Debug.Log( Camera.main.transform.eulerAngles );
        }

        // grasp Cylinder
        if ( Input.GetKeyDown( KeyCode.G ) ) {
            // graspCld.GraspingCylinder = true;
            arbiter.SetMovementState(vMPLMovementArbiter.MovementState.PowerGrasp);
        }

        // grasp Card
        if (Input.GetKeyDown(KeyCode.H))
        {
            // graspCrd.GraspingCard = true;
            arbiter.SetMovementState(vMPLMovementArbiter.MovementState.KeyGrasp);
        }

        //grasp Stick
        if (Input.GetKeyDown(KeyCode.J))
        {
            // graspStk.GraspingStick = true;
            arbiter.SetMovementState(vMPLMovementArbiter.MovementState.PinchGrasp);
        }

        //grasp Tripod
        if (Input.GetKeyDown(KeyCode.K))
        {
            // graspTri.GraspingTripod = true;
            arbiter.SetMovementState(vMPLMovementArbiter.MovementState.TripodGrasp);
        }

        //release
        if (Input.GetKeyDown(KeyCode.I))
        {
            arbiter.SetMovementState(vMPLMovementArbiter.MovementState.HandOpen);
            // graspCld.GraspingCylinder = false;
            // graspCrd.GraspingCard = false;
            // graspStk.GraspingStick = false;
            // graspTri.GraspingTripod = false;
        }

        // hand commands (SHIFT+NUMBER)
        if ( Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift ) ) {
            if ( Input.GetKeyDown( KeyCode.Alpha0 ) || Input.GetKeyDown( KeyCode.Keypad0 ) ) {
                arbiter.SetMovementState( vMPLMovementArbiter.MovementState.Rest );
            } else if ( Input.GetKeyDown( KeyCode.Alpha1 ) || Input.GetKeyDown( KeyCode.Keypad1 ) ) {
                arbiter.SetMovementState( vMPLMovementArbiter.MovementState.HandOpen );
            } else if ( Input.GetKeyDown( KeyCode.Alpha2 ) || Input.GetKeyDown( KeyCode.Keypad2 ) ) {
                arbiter.SetMovementState( vMPLMovementArbiter.MovementState.PowerGrasp );
            } else if ( Input.GetKeyDown( KeyCode.Alpha3 ) || Input.GetKeyDown( KeyCode.Keypad3 ) ) {
                arbiter.SetMovementState( vMPLMovementArbiter.MovementState.TripodGrasp );
            } else if ( Input.GetKeyDown( KeyCode.Alpha4 ) || Input.GetKeyDown( KeyCode.Keypad4 ) ) {
                arbiter.SetMovementState( vMPLMovementArbiter.MovementState.KeyGrasp );
            } else if ( Input.GetKeyDown( KeyCode.Alpha5 ) || Input.GetKeyDown( KeyCode.Keypad5 ) ) {
                arbiter.SetMovementState( vMPLMovementArbiter.MovementState.PinchGrasp );
            } else if ( Input.GetKeyDown( KeyCode.Alpha6 ) || Input.GetKeyDown( KeyCode.Keypad6 ) ) {
                arbiter.SetMovementState( vMPLMovementArbiter.MovementState.IndexGrasp );
            } else if ( Input.GetKeyDown( KeyCode.Alpha7 ) || Input.GetKeyDown( KeyCode.Keypad7 ) ) {
                arbiter.SetMovementState( vMPLMovementArbiter.MovementState.WristPronate );
            } else if ( Input.GetKeyDown( KeyCode.Alpha8 ) || Input.GetKeyDown( KeyCode.Keypad8 ) ) {
                arbiter.SetMovementState( vMPLMovementArbiter.MovementState.WristSupinate );
            }
        }

        // hud
        if ( Input.GetKeyDown( KeyCode.L ) ) {
            hud.SetActive( !hud.activeSelf );
        }
    }
}
