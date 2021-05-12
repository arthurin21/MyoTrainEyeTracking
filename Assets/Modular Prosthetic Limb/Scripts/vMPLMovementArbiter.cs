using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.IO.IsolatedStorage;
using System;



public class vMPLMovementArbiter : MonoBehaviour
{
    //---------------------------------------
    // PROPERTIES
    //---------------------------------------
    #region Properties


    //MPL Variables
    #region MPL Variables
    //MPL FEATURES
    private const int MPL_NUM_ARM_JOINTS = 7;
    private const int MPL_NUM_FINGER_JOINTS = 20;

    // Upper-Arm Joint Angles (7 total) for home position
    private float[] f_HomePositionAngles = new float[MPL_NUM_ARM_JOINTS];
	// Finger Joint Angles for home poistion
	private float[] f_HomeFingerAngles = new float[MPL_NUM_FINGER_JOINTS];

    //Determines whether it is necessary to tell VulcanXHandle to turn off MUD Command inputs
    private bool b_NeedToHaltUDPCommands = false;

    private bool b_usingLeftArm = false;
    private bool b_usingRightArm = false;

    //
    // These member variables hold the named joint position in degrees.
    //
    #region Left Position Member Variables

    static protected float ms_leftShoulderFE;
    static protected float ms_leftShoulderAA;
    static protected float ms_leftHumeralRot;
    static protected float ms_leftElbowFE;
    static protected float ms_leftWristRot;
    static protected float ms_leftWristDev;
    static protected float ms_leftWristFE;
    static protected float ms_leftIndexAA;
    static protected float ms_leftIndexMCP;
    static protected float ms_leftIndexPIP;
    static protected float ms_leftIndexDIP;
    static protected float ms_leftMiddleAA;
    static protected float ms_leftMiddleMCP;
    static protected float ms_leftMiddlePIP;
    static protected float ms_leftMiddleDIP;
    static protected float ms_leftRingAA;
    static protected float ms_leftRingMCP;
    static protected float ms_leftRingPIP;
    static protected float ms_leftRingDIP;
    static protected float ms_leftLittleAA;
    static protected float ms_leftLittleMCP;
    static protected float ms_leftLittlePIP;
    static protected float ms_leftLittleDIP;
    static protected float ms_leftThumbAA;
    static protected float ms_leftThumbFE;
    static protected float ms_leftThumbMCP;
    static protected float ms_leftThumbDIP;

    #endregion


    //
    // These member variables hold the named joint position in degrees.
    //
    #region Right Position Member Variables

    static protected float ms_rightShoulderFE;
    static protected float ms_rightShoulderAA;
    static protected float ms_rightHumeralRot;
    static protected float ms_rightElbowFE;
    static protected float ms_rightWristRot;
    static protected float ms_rightWristDev;
    static protected float ms_rightWristFE;
    static protected float ms_rightIndexAA;
    static protected float ms_rightIndexMCP;
    static protected float ms_rightIndexPIP;
    static protected float ms_rightIndexDIP;
    static protected float ms_rightMiddleAA;
    static protected float ms_rightMiddleMCP;
    static protected float ms_rightMiddlePIP;
    static protected float ms_rightMiddleDIP;
    static protected float ms_rightRingAA;
    static protected float ms_rightRingMCP;
    static protected float ms_rightRingPIP;
    static protected float ms_rightRingDIP;
    static protected float ms_rightLittleAA;
    static protected float ms_rightLittleMCP;
    static protected float ms_rightLittlePIP;
    static protected float ms_rightLittleDIP;
    static protected float ms_rightThumbAA;
    static protected float ms_rightThumbFE;
    static protected float ms_rightThumbMCP;
    static protected float ms_rightThumbDIP;

    #endregion


    #endregion //MPL VARIABLES

    //Movement States
    #region Movement States

    public float MaxArmSpeed    = 40.0f; // degrees per second
    public float MaxFingerSpeed = 100.0f;  // degrees per second
    private const float ANGLE_EPSILON = 1.0f; // in degrees

    public enum MovementState : int {
        Invalid             = -1,
        Rest                = 0,
        ShoulderFlexion     = 1,
        ShoulderExtension   = 2,
        ShoulderAdduction   = 3,
        ShoulderAbduction   = 4,
        HumeralMedial       = 5,
        HumeralLateral      = 6,
        ElbowFlexion        = 7,
        ElbowExtension      = 8,
        WristPronate        = 9,
        WristSupinate       = 10,
        WristUlnar          = 11,
        WristRadial         = 12,
        WristFlexion        = 13,
        WristExtension      = 14,
        HandOpen            = 15,
        PowerGrasp          = 16,
        TripodGrasp         = 17,
        KeyGrasp            = 18,
        PinchGrasp          = 19,
        IndexGrasp          = 20
    }

    private float[] f_ShoulderFlexionArmAngles   = new float[MPL_NUM_ARM_JOINTS] {    175.0f, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN };
    private float[] f_ShoulderExtensionArmAngles = new float[MPL_NUM_ARM_JOINTS] {    -40.0f, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN };
    private float[] f_ShoulderAdductionArmAngles = new float[MPL_NUM_ARM_JOINTS] { float.NaN,    -20.0f, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN };
    private float[] f_ShoulderAbductionArmAngles = new float[MPL_NUM_ARM_JOINTS] { float.NaN,   -160.0f, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN };
    private float[] f_HumeralMedialArmAngles     = new float[MPL_NUM_ARM_JOINTS] { float.NaN, float.NaN,     90.0f, float.NaN, float.NaN, float.NaN, float.NaN };
    private float[] f_HumeralLateralArmAngles    = new float[MPL_NUM_ARM_JOINTS] { float.NaN, float.NaN,    -45.0f, float.NaN, float.NaN, float.NaN, float.NaN };
    
    private float[] f_ElbowFlexionArmAngles      = new float[MPL_NUM_ARM_JOINTS] { float.NaN, float.NaN, float.NaN,    150.0f, float.NaN, float.NaN, float.NaN };
    private float[] f_ElbowExtensionArmAngles    = new float[MPL_NUM_ARM_JOINTS] { float.NaN, float.NaN, float.NaN,      0.0f, float.NaN, float.NaN, float.NaN };

    private float[] f_WristPronateArmAngles      = new float[MPL_NUM_ARM_JOINTS] { float.NaN, float.NaN, float.NaN, float.NaN,     90.0f, float.NaN, float.NaN };
    private float[] f_WristSupinateArmAngles     = new float[MPL_NUM_ARM_JOINTS] { float.NaN, float.NaN, float.NaN, float.NaN,    -90.0f, float.NaN, float.NaN };
    private float[] f_WristUlnarArmAngles        = new float[MPL_NUM_ARM_JOINTS] { float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,     45.0f, float.NaN };
    private float[] f_WristRadialArmAngles       = new float[MPL_NUM_ARM_JOINTS] { float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,    -15.0f, float.NaN };
    private float[] f_WristFlexionArmAngles      = new float[MPL_NUM_ARM_JOINTS] { float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,     60.0f };
    private float[] f_WristExtensionArmAngles    = new float[MPL_NUM_ARM_JOINTS] { float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,    -60.0f };

    private float[] f_HandOpenFingerAngles    = new float[MPL_NUM_FINGER_JOINTS] { 0f,  0f,  0f,  0f, 0f,  0f,  0f,  0f, 0f,  0f,  0f,  0f, 0f,  0f,  0f,  0f,  0f,  0f,  0f,  0f };
    private float[] f_PowerFingerAngles       = new float[MPL_NUM_FINGER_JOINTS] { 0f, 65f, 65f, 65f, 0f, 65f, 65f, 65f, 0f, 65f, 65f, 65f, 0f, 65f, 65f, 65f, 85f, 65f, 25f, 35f };
    private float[] f_TripodFingerAngles      = new float[MPL_NUM_FINGER_JOINTS] { 0f, 75f, 50f, 15f, 0f, 65f, 30f,  0f, 0f, 80f, 90f, 45f, 0f, 80f, 90f, 45f, 90f, 55f, 10f, 45f };
    private float[] f_KeyFingerAngles         = new float[MPL_NUM_FINGER_JOINTS] { 0f, 45f, 65f, 65f, 0f, 55f, 65f, 65f, 0f, 65f, 65f, 65f, 0f, 65f, 65f, 65f, 20f, 85f,  0f,  0f };
    private float[] f_PinchClosedFingerAngles = new float[MPL_NUM_FINGER_JOINTS] { 0f, 65f, 50f, 15f, 0f, 80f, 90f, 45f, 0f, 80f, 90f, 45f, 0f, 80f, 90f, 45f, 75f, 40f, 30f, 30f };
    private float[] f_PinchOpenFingerAngles   = new float[MPL_NUM_FINGER_JOINTS] { 0f, 65f, 50f, 15f, 0f,  0f,  0f,  0f, 0f,  0f,  0f,  0f, 0f,  0f,  0f,  0f, 75f, 40f, 30f, 30f };
    private float[] f_ActiveIndexFingerAngles = new float[MPL_NUM_FINGER_JOINTS] { 0f,  0f,  0f,  0f, 0f, 65f, 65f, 65f, 0f, 65f, 65f, 65f, 0f, 65f, 65f, 65f, 85f, 65f, 25f, 35f };
    // private float[] f_CylinderFingerAngles = new float[MPL_NUM_FINGER_JOINTS] { 0f, 50f, 50f, 50f, 0f, 50f, 50f, 50f, 0f, 50f, 50f, 50f, 0f, 50f, 50f, 50f, 90f, 30f, 30f, 30f };
    
    private float[] f_StartArmAngles = new float[MPL_NUM_ARM_JOINTS];
    private float[] f_TargetArmAngles = new float[MPL_NUM_ARM_JOINTS];

    private float [] f_StartFingerAngles = new float[MPL_NUM_FINGER_JOINTS];
    private float [] f_TargetFingerAngles = new float[MPL_NUM_FINGER_JOINTS];

    public MovementState i_MovementState = MovementState.Rest;
    private float fingerStateTimer;
    private float armStateTimer;

    #endregion // Movement States

    //VulcanX Interface Communication
    #region VulcanX Communication
    VulcanXInterface VulcanXHandle;
    #endregion //VulcanX Communication


    #endregion //Properties


    //---------------------------------------
    // FUNCTIONS - UNITY3D (awake, start, update, reset)
    //---------------------------------------
    #region Unity3D Functions

    #region Start
    /// <summary>
    /// Start function - called on start-up of program
    /// </summary>
	void Start() 
    {
        // --------------------
        // VULCANX COMMUNICATION, MPL HANDLERS
        // --------------------
        #region VulcanX Communication Initialization
        GameObject VIESYSHandle = GameObject.Find("VIESYS");
        VulcanXHandle = (VulcanXInterface)VIESYSHandle.GetComponent(typeof(VulcanXInterface));
        #endregion //VulcanX Communication Initialization

        f_HomePositionAngles = new float[MPL_NUM_ARM_JOINTS] { 0f,0f, 0f, 0f,0f, 0f, 0f };
        f_HomeFingerAngles = new float[MPL_NUM_FINGER_JOINTS] { 0f, 20f, 20f, 20f, 0f, 20f, 20f, 20f, 0f, 20f, 20f, 20f, 0f, 20f, 20f, 20f, 20f, 20f, 20f, 20f };

        // --------------------
        // GAME ELEMENTS SETUP
        // --------------------
        #region Game Elements Setup
        StartCoroutine(CommandRightMPLPosition(5, f_HomePositionAngles, f_HomeFingerAngles));
        #endregion //Game Elements Setup

        //Movement State (to Start)
        SetMovementState( i_MovementState );
    }//function - Start

    #endregion //Start

    #region Update
    //-----------------------------------------------------------------------------
    // UPDATED BY CHRISTOPHER HUNT <chunt11@jhmi.edu>
    void Update()
    {
        // Is called periodically for handling events
        MovementState movementState = GetMovementState();

        if ( movementState == MovementState.Rest ) {
            f_TargetArmAngles = GetRightUpperArmAngles();
            f_TargetFingerAngles = GetRightFingerAngles();
        } else {
            switch ( movementState ) {
                case MovementState.ShoulderFlexion:
                    f_StartArmAngles = GetRightUpperArmAngles();
                    f_TargetArmAngles = f_ShoulderFlexionArmAngles;
                    break;
                case MovementState.ShoulderExtension:
                    f_StartArmAngles = GetRightUpperArmAngles();
                    f_TargetArmAngles = f_ShoulderExtensionArmAngles;
                    break;
                case MovementState.ShoulderAdduction:
                    f_StartArmAngles = GetRightUpperArmAngles();
                    f_TargetArmAngles = f_ShoulderAdductionArmAngles;
                    break;
                case MovementState.ShoulderAbduction:
                    f_StartArmAngles = GetRightUpperArmAngles();
                    f_TargetArmAngles = f_ShoulderAbductionArmAngles;
                    break;
                case MovementState.HumeralMedial:
                    f_StartArmAngles = GetRightUpperArmAngles();
                    f_TargetArmAngles = f_HumeralMedialArmAngles;
                    break;
                case MovementState.HumeralLateral:
                    f_StartArmAngles = GetRightUpperArmAngles();
                    f_TargetArmAngles = f_HumeralLateralArmAngles;
                    break;
                case MovementState.ElbowFlexion:
                    f_StartArmAngles = GetRightUpperArmAngles();
                    f_TargetArmAngles = f_ElbowFlexionArmAngles;
                    break;
                case MovementState.ElbowExtension:
                    f_StartArmAngles = GetRightUpperArmAngles();
                    f_TargetArmAngles = f_ElbowExtensionArmAngles;
                    break;
                case MovementState.WristPronate:
                    f_StartArmAngles = GetRightUpperArmAngles();
                    f_TargetArmAngles = f_WristPronateArmAngles;
                    break;
                case MovementState.WristSupinate:
                    f_StartArmAngles = GetRightUpperArmAngles();
                    f_TargetArmAngles = f_WristSupinateArmAngles;
                    break;
                case MovementState.WristUlnar:
                    f_StartArmAngles = GetRightUpperArmAngles();
                    f_TargetArmAngles = f_WristUlnarArmAngles;
                    break;
                case MovementState.WristRadial:
                    f_StartArmAngles = GetRightUpperArmAngles();
                    f_TargetArmAngles = f_WristRadialArmAngles;
                    break;
                case MovementState.WristFlexion:
                    f_StartArmAngles = GetRightUpperArmAngles();
                    f_TargetArmAngles = f_WristFlexionArmAngles;
                    break;
                case MovementState.WristExtension:
                    f_StartArmAngles = GetRightUpperArmAngles();
                    f_TargetArmAngles = f_WristExtensionArmAngles;
                    break;
                case MovementState.HandOpen:
                    f_StartFingerAngles = GetRightFingerAngles();
                    f_TargetFingerAngles = f_HandOpenFingerAngles;
                    break;
                case MovementState.TripodGrasp:
                    f_StartFingerAngles = GetRightFingerAngles();
                    f_TargetFingerAngles = f_TripodFingerAngles;
                    break;
                case MovementState.KeyGrasp:
                    f_StartFingerAngles = GetRightFingerAngles();
                    f_TargetFingerAngles = f_KeyFingerAngles;
                    break;
                case MovementState.PinchGrasp:
                    f_StartFingerAngles = GetRightFingerAngles();
                    f_TargetFingerAngles = f_PinchOpenFingerAngles;
                    break;
                case MovementState.PowerGrasp:
                    f_StartFingerAngles = GetRightFingerAngles();
                    f_TargetFingerAngles = f_PowerFingerAngles;
                    break;
                case MovementState.IndexGrasp:
                    f_StartFingerAngles = GetRightFingerAngles();
                    f_TargetFingerAngles = f_ActiveIndexFingerAngles;
                    break;
            }
        }

        // if we are not resting
        if ( movementState != MovementState.Rest ) {

            if ( VulcanXHandle.m_haveRightMPL ) {
                // move finger angles for right MPL
                float [] f_CurrentFingerAngles = GetRightFingerAngles();
                float [] f_CurrentFingerDistances = new float[MPL_NUM_FINGER_JOINTS];

                float maxFingerDistance = 0.0f;
                for ( int i = 0; i < MPL_NUM_FINGER_JOINTS; i++ ) {
                    f_CurrentFingerDistances[i] = Mathf.Abs( f_CurrentFingerAngles[i] - f_TargetFingerAngles[i] );
                    maxFingerDistance = Mathf.Max( maxFingerDistance, f_CurrentFingerDistances[i] );
                }
                
                for ( int i = 0; i < MPL_NUM_FINGER_JOINTS; i++ ) {
                    if ( f_CurrentFingerDistances[i] > ANGLE_EPSILON ) {
                        if ( f_CurrentFingerAngles[i] < f_TargetFingerAngles[i] ) {
                            f_CurrentFingerAngles[i] += ( MaxFingerSpeed * ( f_CurrentFingerDistances[i] / maxFingerDistance ) * Time.deltaTime );
                        } else if ( f_CurrentFingerAngles[i] > f_TargetFingerAngles[i] ) {
                            f_CurrentFingerAngles[i] -= ( MaxFingerSpeed * ( f_CurrentFingerDistances[i] / maxFingerDistance ) * Time.deltaTime );
                        }
                    }
                }

                SetRightFingerAngles( f_CurrentFingerAngles );

                // move upper arm angles for right MPL
                float [] f_CurrentArmAngles = GetRightUpperArmAngles();
                float [] f_CurrentArmDistances = new float[MPL_NUM_ARM_JOINTS];

                float maxArmDistance = 0.0f;
                for ( int i = 0; i < MPL_NUM_ARM_JOINTS; i++ ) {
                    if ( !float.IsNaN( f_TargetArmAngles[i] ) ) {
                        f_CurrentArmDistances[i] = Mathf.Abs( f_CurrentArmAngles[i] - f_TargetArmAngles[i] );
                        maxArmDistance = Mathf.Max( maxArmDistance, f_CurrentArmDistances[i] );
                    }
                }

                for ( int i = 0; i < MPL_NUM_ARM_JOINTS; i++ ) {
                    if ( !float.IsNaN( f_TargetArmAngles[i] ) ) {
                        if ( f_CurrentArmDistances[i] > ANGLE_EPSILON ) {
                            if ( f_CurrentArmAngles[i] < f_TargetArmAngles[i] ) {
                                f_CurrentArmAngles[i] += ( MaxArmSpeed * Time.deltaTime );
                            } else if ( f_CurrentArmAngles[i] > f_TargetArmAngles[i] ) {
                                f_CurrentArmAngles[i] -= ( MaxArmSpeed * Time.deltaTime );
                            }
                        }
                    }
                }

                SetRightUpperArmAngles( f_CurrentArmAngles );
            } else if ( VulcanXHandle.m_haveLeftMPL ) {
                // pass
            }
        }

        // move MPL limb
        if ( VulcanXHandle.m_haveRightMPL ) {
            VulcanXHandle.SetRightUpperArmAngles( GetRightUpperArmAngles() );
            VulcanXHandle.SetRightFingerAngles( GetRightFingerAngles() );
        } else if ( VulcanXHandle.m_haveLeftMPL ) {
            VulcanXHandle.SetLeftUpperArmAngles( GetLeftUpperArmAngles() );
            VulcanXHandle.SetLeftFingerAngles( GetLeftFingerAngles() );
        }

    }//function - Update

    #endregion //Update


    #endregion //Unity3D Function


    //---------------------------------------
    // FUNCTIONS - MPL MOVEMENT STATE
    //---------------------------------------
    #region MPL Movement State
    
    public void SetMovementState(MovementState i_NewMovementState)
    {
        if (i_MovementState != i_NewMovementState) {
            if ( i_NewMovementState >= MovementState.Rest && i_NewMovementState <= MovementState.WristSupinate ) {
                i_MovementState = i_NewMovementState;
            }
        }
        i_MovementState = i_NewMovementState;

    }//function - SetMovementState


    public MovementState GetMovementState()
    {
        return i_MovementState;

    }//function - GetMovementState

    #endregion //MPL Movement State


    //---------------------------------------
    // FUNCTIONS - MPL INTERFACE (Movement, Attributes)
    //---------------------------------------
    #region MPL Interface (Movement, Attributes)

    /// <summary>
    /// Will reset the hand position to that of start
    /// </summary>
    private IEnumerator CommandRightMPLPosition(float f_Duration, float[] f_RightUpperArmJointAngles, float[] f_RightFingerJointAngles)
    {
        //Will Home the MPL based on position definition in XML file
        bool b_RightHandEnabled = false;
        bool b_LeftHandEnabled = false;

        if (GameObject.Find("rPalm") != null)
        {
            b_RightHandEnabled = true;
        }

        if (GameObject.Find("lPalm") != null)
        {
            b_LeftHandEnabled = true;
        }
                

        //Halt MPL Movement (turn off MUD Commands receipt and processing)
        #region HALT MPL

        if (b_NeedToHaltUDPCommands)
        {
            #region RIGHT HAND

            //Debug.Log("Moving Arm to Home");

            if (b_RightHandEnabled)
            {
                Debug.Log("Moving Right Arm to Home");

                //Freeze the MUD Command System (temporarily)
                VulcanXHandle.SetRightMPLMUDMovementEnable(false);

                //Turn OFF the colliders for the MPL so that it can move into position without being impeded/stalled
                //        Physics.IgnoreLayerCollision(GameObject.Find("Placement_TableTopShelf").layer, GameObject.Find("rPalm").layer, true);
                //        Physics.IgnoreLayerCollision(GameObject.Find("TableHandCol").layer, GameObject.Find("rPalm").layer, true);
                //        Physics.IgnoreLayerCollision(GameObject.Find("DefaultRoom").layer, GameObject.Find("rPalm").layer, true);
            }//if - test for each MPL (right/left)
            #endregion //RIGHT HAND

            #region LEFT HAND
            if (b_LeftHandEnabled)
            {
                Debug.Log("Moving Left Arm to Home");

                //Freeze the MUD Command System (temporarily)
                VulcanXHandle.SetLeftMPLMUDMovementEnable(false);

                //Turn OFF the colliders for the MPL so that it can move into position without being impeded/stalled
                //        Physics.IgnoreLayerCollision(GameObject.Find("Placement_TableTopShelf").layer, GameObject.Find("lPalm").layer, true);
                //        Physics.IgnoreLayerCollision(GameObject.Find("TableHandCol").layer, GameObject.Find("lPalm").layer, true);
                //        Physics.IgnoreLayerCollision(GameObject.Find("DefaultRoom").layer, GameObject.Find("lPalm").layer, true);

            }//if - test for each MPL (right/left)
            #endregion //LEFT HAND

        }//if - b_NeedToHaltUDPCommands

        #endregion //HALT MPL


        //Reset MPL Position for next trial (using internal controller)
        #region COMMANDS

        //Send the vMPL to the commanded position 
        
        
        int i = 0;
        int i_NumberSignals = 20;

        if (b_RightHandEnabled)
        {
            Debug.Log("Setting Right Hand Angles: (" + f_RightUpperArmJointAngles[0] + ", " + f_RightUpperArmJointAngles[1] + ", " + f_RightUpperArmJointAngles[2] + ", " + f_RightUpperArmJointAngles[3] + ", " + f_RightUpperArmJointAngles[4] + ", " + f_RightUpperArmJointAngles[5] + ", " + f_RightUpperArmJointAngles[6] + ")");
            VulcanXHandle.SetRightUpperArmAngles(f_RightUpperArmJointAngles); //Passes an array of the upper 7 joint angles
            VulcanXHandle.SetRightFingerAngles(f_RightFingerJointAngles);

            for (i = 1; i < i_NumberSignals; i++)
            {
                VulcanXHandle.SetRightUpperArmAngles(f_RightUpperArmJointAngles); //Passes an array of the upper 7 joint angles
                VulcanXHandle.SetRightFingerAngles(f_RightFingerJointAngles);

            }//for - continue passing in commands

        }//if - test for each MPL (right/left)


        

        //TODO - Replace with value in XML File
        //yield return new UnityEngine.WaitForSeconds(2);
        //yield return new UnityEngine.WaitForSeconds(timer_Default_PreTrial);
        yield return new UnityEngine.WaitForSeconds(f_Duration);

        #endregion //COMMANDS


        //Turn MPL movement back on (turn on MUD Commands receipt and processing)
        #region FREE MPL
        if (b_NeedToHaltUDPCommands)
        {
            #region RIGHT HAND
            if (b_RightHandEnabled)
            {
                //Turn ON the colliders for the MPL collides normally
                //        Physics.IgnoreLayerCollision(GameObject.Find("TableHandCol").layer, GameObject.Find("rPalm").layer, false);
                //        Physics.IgnoreLayerCollision(GameObject.Find("DefaultRoom").layer, GameObject.Find("rPalm").layer, false);
                //        Physics.IgnoreLayerCollision(GameObject.Find("Placement_TableTopShelf").layer, GameObject.Find("rPalm").layer, false);

                //Un-Freeze the MUD Command System
                VulcanXHandle.SetRightMPLMUDMovementEnable(true);

            }//if - test for each MPL (right/left)
            #endregion //RIGHT HAND

            #region LEFT HAND
            if (b_LeftHandEnabled)
            {
                //Turn ON the colliders for the MPL collides normally
                //        Physics.IgnoreLayerCollision(GameObject.Find("TableHandCol").layer, GameObject.Find("lPalm").layer, false);
                //        Physics.IgnoreLayerCollision(GameObject.Find("DefaultRoom").layer, GameObject.Find("lPalm").layer, false);
                //        Physics.IgnoreLayerCollision(GameObject.Find("Placement_TableTopShelf").layer, GameObject.Find("lPalm").layer, false);

                //Un-Freeze the MUD Command System
                VulcanXHandle.SetLeftMPLMUDMovementEnable(true);

            }//if - test for each MPL (right/left)
            #endregion //LEFT HAND

        }//if - b_NeedToHaltUDPCommands

        #endregion //FREE MPL


    }//function - CommandRightMPLPosition



    #endregion //MPL Interface (Movement, Attributes)


    //---------------------------------------
    // FUNCTIONS - MPL JOINT ANGLES
    //---------------------------------------
    #region MPL Joint Angles

    /// <summary>
    /// Will send an internal command to the limb that sets the right upper-arm joint angles
    /// </summary>
    public void SetRightUpperArmAngles(float[] f_JointAnglesDegrees)
    {
        // float[7] f_JointAnglesDegrees - Joint angles in degrees (0-180)

        #region UpperArmJoints
        ms_rightShoulderFE = f_JointAnglesDegrees[0];
        ms_rightShoulderAA = f_JointAnglesDegrees[1];
        ms_rightHumeralRot = f_JointAnglesDegrees[2];
        ms_rightElbowFE = f_JointAnglesDegrees[3];
        ms_rightWristRot = f_JointAnglesDegrees[4];
        ms_rightWristDev = f_JointAnglesDegrees[5];
        ms_rightWristFE = f_JointAnglesDegrees[6];
        #endregion //UpperArmJoints

        // Debug.Log( string.Format( "SetRightUpperArmAngles: {0}, {1}, {2}, {3}, {4}, {5}, {6}", ms_rightShoulderFE.ToString(), ms_rightShoulderAA.ToString(), ms_rightHumeralRot.ToString(), 
        //             ms_rightElbowFE.ToString(), ms_rightWristRot.ToString(), ms_rightWristDev.ToString(), ms_rightWristFE.ToString() ) );
    }//function - SetRightUpperArmAngles

    /// <summary>
    /// Will send an internal command to the limb that sets the right finger joint angles
    /// </summary>
    public void SetRightFingerAngles(float[] j_FingerJointAnglesDegrees)
    {
        #region Fingers
        ms_rightIndexAA = j_FingerJointAnglesDegrees[0];
        ms_rightIndexMCP = j_FingerJointAnglesDegrees[1];
        ms_rightIndexPIP = j_FingerJointAnglesDegrees[2];
        ms_rightIndexDIP = j_FingerJointAnglesDegrees[3];
        ms_rightMiddleAA = j_FingerJointAnglesDegrees[4];
        ms_rightMiddleMCP = j_FingerJointAnglesDegrees[5];
        ms_rightMiddlePIP = j_FingerJointAnglesDegrees[6];
        ms_rightMiddleDIP = j_FingerJointAnglesDegrees[7];
        ms_rightRingAA = j_FingerJointAnglesDegrees[8];
        ms_rightRingMCP = j_FingerJointAnglesDegrees[9];
        ms_rightRingPIP = j_FingerJointAnglesDegrees[10];
        ms_rightRingDIP = j_FingerJointAnglesDegrees[11];
        ms_rightLittleAA = j_FingerJointAnglesDegrees[12];
        ms_rightLittleMCP = j_FingerJointAnglesDegrees[13];
        ms_rightLittlePIP = j_FingerJointAnglesDegrees[14];
        ms_rightLittleDIP = j_FingerJointAnglesDegrees[15];
        ms_rightThumbAA = j_FingerJointAnglesDegrees[16];
        ms_rightThumbFE = j_FingerJointAnglesDegrees[17];
        ms_rightThumbMCP = j_FingerJointAnglesDegrees[18];
        ms_rightThumbDIP = j_FingerJointAnglesDegrees[19];
        #endregion //Fingers

    }//function - SetRightFingerAngles

    /// <summary>
    /// Will send an internal command to the limb that sets the left upper-arm joint angles
    /// </summary>
    public void SetLeftUpperArmAngles(float[] f_JointAnglesDegrees)
    {
        // float[7] f_JointAnglesDegrees - Joint angles in degrees (0-180)

        #region UpperArmJoints
        ms_leftShoulderFE = f_JointAnglesDegrees[0];
        ms_leftShoulderAA = f_JointAnglesDegrees[1];
        ms_leftHumeralRot = f_JointAnglesDegrees[2];
        ms_leftElbowFE = f_JointAnglesDegrees[3];
        ms_leftWristRot = f_JointAnglesDegrees[4];
        ms_leftWristDev = f_JointAnglesDegrees[5];
        ms_leftWristFE = f_JointAnglesDegrees[6];
        #endregion //UpperArmJoints

    }//function - SetLeftUpperArmAngles

    /// <summary>
    /// Will send an internal command to the limb that sets the left finger joint angles
    /// </summary>
    public void SetLeftFingerAngles(float[] j_FingerJointAnglesDegrees)
    {
        #region Fingers
        ms_leftIndexAA = j_FingerJointAnglesDegrees[0];
        ms_leftIndexMCP = j_FingerJointAnglesDegrees[1];
        ms_leftIndexPIP = j_FingerJointAnglesDegrees[2];
        ms_leftIndexDIP = j_FingerJointAnglesDegrees[3];
        ms_leftMiddleAA = j_FingerJointAnglesDegrees[4];
        ms_leftMiddleMCP = j_FingerJointAnglesDegrees[5];
        ms_leftMiddlePIP = j_FingerJointAnglesDegrees[6];
        ms_leftMiddleDIP = j_FingerJointAnglesDegrees[7];
        ms_leftRingAA = j_FingerJointAnglesDegrees[8];
        ms_leftRingMCP = j_FingerJointAnglesDegrees[9];
        ms_leftRingPIP = j_FingerJointAnglesDegrees[10];
        ms_leftRingDIP = j_FingerJointAnglesDegrees[11];
        ms_leftLittleAA = j_FingerJointAnglesDegrees[12];
        ms_leftLittleMCP = j_FingerJointAnglesDegrees[13];
        ms_leftLittlePIP = j_FingerJointAnglesDegrees[14];
        ms_leftLittleDIP = j_FingerJointAnglesDegrees[15];
        ms_leftThumbAA = j_FingerJointAnglesDegrees[16];
        ms_leftThumbFE = j_FingerJointAnglesDegrees[17];
        ms_leftThumbMCP = j_FingerJointAnglesDegrees[18];
        ms_leftThumbDIP = j_FingerJointAnglesDegrees[19];
        #endregion //Fingers

    }//function - SetRightFingerAngles

#endregion //MPL Joint Angles


    // Added by Christopher Hunt <chunt11@jhmi.edu>
    public float [] GetRightUpperArmAngles() {
        float[] ret = new float[MPL_NUM_ARM_JOINTS];

        ret[0] = ms_rightShoulderFE;
        ret[1] = ms_rightShoulderAA;
        ret[2] = ms_rightHumeralRot;
        ret[3] = ms_rightElbowFE;
        ret[4] = ms_rightWristRot;
        ret[5] = ms_rightWristDev;
        ret[6] = ms_rightWristFE;

        return ret;
    }

    public float [] GetRightFingerAngles() {
        float[] ret = new float[MPL_NUM_FINGER_JOINTS];

        ret[0] = ms_rightIndexAA;
        ret[1] = ms_rightIndexMCP;
        ret[2] = ms_rightIndexPIP;
        ret[3] = ms_rightIndexDIP;
        ret[4] = ms_rightMiddleAA;
        ret[5] = ms_rightMiddleMCP;
        ret[6] = ms_rightMiddlePIP;
        ret[7] = ms_rightMiddleDIP;
        ret[8] = ms_rightRingAA;
        ret[9] = ms_rightRingMCP;
        ret[10] = ms_rightRingPIP;
        ret[11] = ms_rightRingDIP;
        ret[12] = ms_rightLittleAA;
        ret[13] = ms_rightLittleMCP;
        ret[14] = ms_rightLittlePIP;
        ret[15] = ms_rightLittleDIP;
        ret[16] = ms_rightThumbAA;
        ret[17] = ms_rightThumbFE;
        ret[18] = ms_rightThumbMCP;
        ret[19] = ms_rightThumbDIP;

        return ret;
    }

    public float [] GetLeftUpperArmAngles() {
        float[] ret = new float[MPL_NUM_ARM_JOINTS];

        ret[0] = ms_leftShoulderFE;
        ret[1] = ms_leftShoulderAA;
        ret[2] = ms_leftHumeralRot;
        ret[3] = ms_leftElbowFE;
        ret[4] = ms_leftWristRot;
        ret[5] = ms_leftWristDev;
        ret[6] = ms_leftWristFE;

        return ret;
    }

    public float [] GetLeftFingerAngles() {
        float[] ret = new float[MPL_NUM_FINGER_JOINTS];

        ret[0] = ms_leftIndexAA;
        ret[1] = ms_leftIndexMCP;
        ret[2] = ms_leftIndexPIP;
        ret[3] = ms_leftIndexDIP;
        ret[4] = ms_leftMiddleAA;
        ret[5] = ms_leftMiddleMCP;
        ret[6] = ms_leftMiddlePIP;
        ret[7] = ms_leftMiddleDIP;
        ret[8] = ms_leftRingAA;
        ret[9] = ms_leftRingMCP;
        ret[10] = ms_leftRingPIP;
        ret[11] = ms_leftRingDIP;
        ret[12] = ms_leftLittleAA;
        ret[13] = ms_leftLittleMCP;
        ret[14] = ms_leftLittlePIP;
        ret[15] = ms_leftLittleDIP;
        ret[16] = ms_leftThumbAA;
        ret[17] = ms_leftThumbFE;
        ret[18] = ms_leftThumbMCP;
        ret[19] = ms_leftThumbDIP;

        return ret;
    }

}//file - CatchGameManager
