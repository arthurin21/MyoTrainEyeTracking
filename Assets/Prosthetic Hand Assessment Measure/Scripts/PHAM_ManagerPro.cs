using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViveSR.anipal.Eye
{
    public class PHAM_ManagerPro : MonoBehaviour
    {
        public enum ObjectPrimitive : uint
        {
            Cylinder = 0,
            Card = 1,
            Stick = 2,
            Tripod = 3
        }

        public enum PHAMStage : uint
        {
            Inactive = 0,   // triggered after a failed or completed task
            Active = 1    // triggered during the actual object manipulation
        }

        public int randomSeed = 0;

        public float taskTimeout = 15.0f;
        private bool taskSuccess = false;

        private static List<GameObject> vholders;
        private static List<GameObject> hholders;

        private static ObjectPrimitive currentObject;
        private static Transform initialTransform;
        private static Transform targetTransform;

        private static Dictionary<string, GameObject> primitives;
        private ButtonPress button = null;
        private DataLogger logger = null;
        private EyeDataCollect edc = null;

        private PHAMStage currentStage = PHAMStage.Inactive;
        private float timeElapsed = 0.0f;

        void Start()
        {
            // seed the random number generator
            Random.InitState(randomSeed);

            // save all primitives
            primitives = new Dictionary<string, GameObject>();
            foreach (Transform t in GameObject.Find("GeometricPrimitives").transform)
            {
                primitives.Add(t.name, t.gameObject);
            }

            //Add all the available holders to the manager
            vholders = new List<GameObject>();
            hholders = new List<GameObject>();

            foreach (Transform child in transform)
            {
                if (child.name.Contains("VerticalHolder"))
                {
                    vholders.Add(child.gameObject);
                }
                else if (child.name.Contains("HorizontalHolder"))
                {
                    hholders.Add(child.gameObject);
                }
            }

            // find button
            button = GetComponentInChildren<ButtonPress>();

            // find data logger
            logger = FindObjectOfType<DataLogger>();
            logger.SetLogging(DataLogger.LoggingStatus.Clear);
            edc = FindObjectOfType<EyeDataCollect>();
            edc.SetLogging(EyeDataCollect.LoggingStatus.Clear);

            // clear PHAM
            ClearPHAM();
        }

        void Update()
        {
            switch (currentStage)
            {
                case PHAMStage.Inactive:
                    taskSuccess = false;    // reset task success value
                    timeElapsed = 0.0f;     // reset timer

                    button.gameObject.GetComponent<Renderer>().material.color = Color.red;          // indicate that button needs to be pressed
                    if (button.GetButtonStatus())
                    {
                        ColorHolder();                                                              // generate a new task
                        button.SetButtonStatus(false);                                            // reset button status
                        button.gameObject.GetComponent<Renderer>().material.color = Color.green;     // reset button color
                        currentStage = PHAMStage.Active;                                            // set the stage to active
                        logger.SetLogging(DataLogger.LoggingStatus.Active);                       // activate data logging
                        edc.SetLogging(EyeDataCollect.LoggingStatus.Active);
                    }
                    break;
                case PHAMStage.Active:
                    timeElapsed += Time.deltaTime;  // update timer

                    // get dropped / success status
                    bool dropped = false;
                    bool success = false;

                    dropped = primitives["Cylinder"].GetComponent<DroppedObjectLogic>().GetDroppedStatus();
                    success = primitives["Cylinder"].GetComponent<SuccessTaskLogic>().GetSuccessStatus();
                    if (success || dropped)
                    {
                        primitives["Cylinder"].GetComponent<GraspingObjectLogic>().ClearFixedJoints();
                    }

                    
                    if (dropped || (timeElapsed > taskTimeout))
                    {           // object was dropped, task failed, load next task
                        if (dropped)
                        {
                            Debug.Log("Object was dropped! Task failed!");
                        }
                        else
                        {
                            Debug.Log("Task timeout reached! Task failed!");
                        }

                        taskSuccess = false;                                    // set task success to false
                        ClearPHAM();                                            // reset PHAM

                        logger.SetLogging(DataLogger.LoggingStatus.Pause);    // pause data logging
                        logger.Publish();                                       // publish to file
                        logger.SetLogging(DataLogger.LoggingStatus.Clear);    // clear data log
                        edc.SetLogging(EyeDataCollect.LoggingStatus.Pause);                    // pause data logging
                        edc.Publish();                                                       // publish to file
                        edc.SetLogging(EyeDataCollect.LoggingStatus.Clear);                    // clear data log

                        button.SetButtonStatus(false);                        // make sure button is not pressed
                        currentStage = PHAMStage.Inactive;                      // set PHAM stage to inactive
                    }
                    else if (success && button.GetButtonStatus())
                    {                                                         // object in the target holder, task succeeded
                        button.gameObject.GetComponent<Renderer>().material.color = Color.red;      // indicate that button needs to be pressed                                        // button press signals the end of the task
                        Debug.Log("Logging successful task!");

                        // clear PHAM
                        taskSuccess = true;                                                     // set task success to true
                        ClearPHAM();                                                            // reset PHAM

                        logger.SetLogging(DataLogger.LoggingStatus.Pause);                    // pause data logging
                        logger.Publish();                                                       // publish to file
                        logger.SetLogging(DataLogger.LoggingStatus.Clear);                    // clear data log
                        edc.SetLogging(EyeDataCollect.LoggingStatus.Pause);                    // pause data logging
                        edc.Publish();                                                       // publish to file
                        edc.SetLogging(EyeDataCollect.LoggingStatus.Clear);                    // clear data log

                        button.SetButtonStatus(false);                                        // make sure button is not pressed
                        button.gameObject.GetComponent<Renderer>().material.color = Color.grey; // reset button color
                        currentStage = PHAMStage.Inactive;                                      // set PHAM stage to inactive

                    }
                    break;
            }
        }

        private void ClearPHAM()
        {
            // deactivate all objects
            foreach (KeyValuePair<string, GameObject> kvp in primitives)
            {
                kvp.Value.SetActive(false);
                kvp.Value.GetComponent<SuccessTaskLogic>().SetSuccessStatus(false);
            }

            // clear the colors of all holders
            foreach (GameObject holder in hholders)
            {
                holder.GetComponent<Renderer>().material.color = Color.grey; // new Color( 0.8f, 0.8f, 0.8f );
                holder.GetComponent<Holder>().deactivate();
            }

            foreach (GameObject holder in vholders)
            {
                holder.GetComponent<Renderer>().material.color = Color.grey; // new Color( 0.8f, 0.8f, 0.8f );
                holder.GetComponent<Holder>().deactivate();
            }
        }

        private void ColorHolder()
        {

            //default setting for objects
            ClearPHAM();


            currentObject = ObjectPrimitive.Cylinder;

            // bool startHorizontal = false; 
            // currentObject = ObjectPrimitive.Cylinder;



            int taskType = Random.Range(1, 5);

            switch (taskType) {
                case 1:
                    initialTransform = GameObject.Find("VerticalHolder_TopLeft").transform.Find("TargetCylinder").transform;
                    primitives["Cylinder"].transform.position = initialTransform.position;
                    primitives["Cylinder"].transform.rotation = initialTransform.rotation;

                    primitives["Cylinder"].SetActive(true);
                    primitives["Cylinder"].GetComponent<DroppedObjectLogic>().SetDroppedStatus(false);
                    GameObject.Find("HorizontalHolder_BotRight").GetComponent<Renderer>().material.color = Color.red;
                    targetTransform = GameObject.Find("HorizontalHolder_BotRight").transform.Find("TargetCylinder").transform;
                    GameObject.Find("HorizontalHolder_BotRight").GetComponent<Holder>().activate();
                    break;

                case 2:
                    initialTransform = GameObject.Find("HorizontalHolder_TopLeft").transform.Find("TargetCylinder").transform;
                    primitives["Cylinder"].transform.position = initialTransform.position;
                    primitives["Cylinder"].transform.rotation = initialTransform.rotation;

                    primitives["Cylinder"].SetActive(true);
                    primitives["Cylinder"].GetComponent<DroppedObjectLogic>().SetDroppedStatus(false);
                    GameObject.Find("VerticalHolder_BotRight").GetComponent<Renderer>().material.color = Color.red;
                    targetTransform = GameObject.Find("VerticalHolder_BotRight").transform.Find("TargetCylinder").transform;
                    GameObject.Find("VerticalHolder_BotRight").GetComponent<Holder>().activate();
                    break;

                case 3:
                    initialTransform = GameObject.Find("HorizontalHolder_BotLeft").transform.Find("TargetCylinder").transform;
                    primitives["Cylinder"].transform.position = initialTransform.position;
                    primitives["Cylinder"].transform.rotation = initialTransform.rotation;

                    primitives["Cylinder"].SetActive(true);
                    primitives["Cylinder"].GetComponent<DroppedObjectLogic>().SetDroppedStatus(false);
                    GameObject.Find("VerticalHolder_TopRight").GetComponent<Renderer>().material.color = Color.red;
                    targetTransform = GameObject.Find("VerticalHolder_TopRight").transform.Find("TargetCylinder").transform;
                    GameObject.Find("VerticalHolder_TopRight").GetComponent<Holder>().activate();
                    break;

                case 4:
                    initialTransform = GameObject.Find("HorizontalHolder_TopoRight").transform.Find("TargetCylinder").transform;
                    primitives["Cylinder"].transform.position = initialTransform.position;
                    primitives["Cylinder"].transform.rotation = initialTransform.rotation;

                    primitives["Cylinder"].SetActive(true);
                    primitives["Cylinder"].GetComponent<DroppedObjectLogic>().SetDroppedStatus(false);
                    GameObject.Find("VerticalHolder_BotLeft").GetComponent<Renderer>().material.color = Color.red;
                    targetTransform = GameObject.Find("VerticalHolder_BotLeft").transform.Find("TargetCylinder").transform;
                    GameObject.Find("VerticalHolder_BotLeft").GetComponent<Holder>().activate();
                    break;
            }
           
        }

        public ObjectPrimitive GetPrimitive()
        {
            return currentObject;
        }

        public Transform GetInitialTransform()
        {
            return initialTransform;
        }

        public Transform GetTargetTransform()
        {
            return targetTransform;
        }

        public bool GetTaskSuccess()
        {
            return taskSuccess;
        }
    }
}