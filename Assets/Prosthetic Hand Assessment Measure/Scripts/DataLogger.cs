using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

namespace ViveSR.anipal.Eye
{
    public class DataLogger : MonoBehaviour
    {
        public enum LoggingStatus : uint
        {
            Clear = 0,
            Active = 1,
            Pause = 2
        }
        private LoggingStatus logStatus = LoggingStatus.Clear;

        public string SubjectID = "TestSubject";

        private GameObject mplHand;
        private Dictionary<PHAM_ManagerPro.ObjectPrimitive, GameObject> objPrimitives = new Dictionary<PHAM_ManagerPro.ObjectPrimitive, GameObject>();

        private PHAM_ManagerPro manager;
        private vMPLMovementArbiter arbiter;
        private FingerTipForceSensors sensors;

        private float currentTime;
        private List<float[]> buffer = new List<float[]>();

        void Awake()
        {
            // these need to be initialized earlier than Start since object primitives may be inactivated in the manager script
            manager = GameObject.Find("PHAM").GetComponent<PHAM_ManagerPro>();
            objPrimitives[PHAM_ManagerPro.ObjectPrimitive.Cylinder] = GameObject.Find("Cylinder");
            objPrimitives[PHAM_ManagerPro.ObjectPrimitive.Card] = GameObject.Find("Card");
            objPrimitives[PHAM_ManagerPro.ObjectPrimitive.Stick] = GameObject.Find("Stick");
            objPrimitives[PHAM_ManagerPro.ObjectPrimitive.Tripod] = GameObject.Find("Tripod");
        }

        // Start is called before the first frame update
        void Start()
        {
            // get MPL palm
            mplHand = GameObject.Find("rPalm");


            // get movement arbiter
            arbiter = GameObject.Find("vMPLMovementArbiter").GetComponent<vMPLMovementArbiter>();

            // get finger tip force sensors
            sensors = GameObject.FindObjectOfType<FingerTipForceSensors>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (logStatus)
            {
                case LoggingStatus.Clear:
                    buffer.Clear();     // clear buffer
                    currentTime = 0.0f; // update current time
                    break;
                case LoggingStatus.Active:
                    // record data
                    float[] row = new float[21];

                    currentTime += Time.deltaTime;
                    row[0] = currentTime; // timestamp

                    Vector3 handPosition = mplHand.transform.position;
                    row[1] = handPosition.x;
                    row[2] = handPosition.y;
                    row[3] = handPosition.z;

                    Quaternion handRotation = mplHand.transform.rotation;
                    row[4] = handRotation.w;
                    row[5] = handRotation.x;
                    row[6] = handRotation.y;
                    row[7] = handRotation.z;

                    PHAM_ManagerPro.ObjectPrimitive currentPrimitive = manager.GetPrimitive();

                    Vector3 objPosition = objPrimitives[currentPrimitive].transform.position;
                    row[8] = objPosition.x;
                    row[9] = objPosition.y;
                    row[10] = objPosition.z;

                    Quaternion objRotation = objPrimitives[currentPrimitive].transform.rotation;
                    row[11] = objRotation.w;
                    row[12] = objRotation.x;
                    row[13] = objRotation.y;
                    row[14] = objRotation.z;

                    row[15] = (float)arbiter.GetMovementState();

                    float[] forces = sensors.GetForceValues();
                    row[16] = forces[0];
                    row[17] = forces[1];
                    row[18] = forces[2];
                    row[19] = forces[3];
                    row[20] = forces[4];

                    // add to list
                    buffer.Add(row);
                    break;
                case LoggingStatus.Pause:
                    break;
            }
        }

        public void SetLogging(LoggingStatus newStatus)
        {
            logStatus = newStatus;
        }

        public void Publish()
        {
            // get filename for this publish
            string filedir = Path.Combine(GetDataPath(), "Data", SubjectID, System.DateTime.Today.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(filedir))
            {
                Directory.CreateDirectory(filedir);
            }

            int task = 0;
            string filename;
            do
            {
                filename = Path.Combine(filedir, $"{SubjectID}_{System.DateTime.Today.ToString("yyyy-MM-dd")}_Task{task.ToString("D4")}.csv");
                task += 1;
            } while (File.Exists(filename));

            using (StreamWriter writer = new StreamWriter(new FileStream(filename, FileMode.Create, FileAccess.Write)))
            {
                writer.WriteLine("sep=,");
                writer.WriteLine("ID, Date, Object, InitPosX, InitPosY, InitPosZ, InitRotW, InitRotX, InitRotY, InitRotZ, TargetPosX, TargetPosY, TargetPosZ, TargetRotW, TargetRotX, TargetRotY, TargetRotZ, Success");

                string currentPrimitive = Enum.GetName(typeof(PHAM_ManagerPro.ObjectPrimitive), manager.GetPrimitive());
                Transform init = manager.GetInitialTransform();
                Vector3 initPos = init.position;
                Quaternion initRot = init.rotation;

                Transform target = manager.GetTargetTransform();
                Vector3 targetPos = target.position;
                Quaternion targetRot = target.rotation;

                int success = manager.GetTaskSuccess() ? 1 : 0;

                string line = $"{SubjectID}, {System.DateTime.Now.ToString()}, {currentPrimitive}, ";
                line += $"{initPos.x}, {initPos.y}, {initPos.z}, {initRot.w}, {initRot.x}, {initRot.y}, {initRot.z}, ";
                line += $"{targetPos.x}, {targetPos.y}, {targetPos.z}, {targetRot.w}, {targetRot.x}, {targetRot.y}, {targetRot.z}, ";
                line += $"{success}";
                writer.WriteLine(line);

                writer.WriteLine("");
                writer.WriteLine("Time, HandPosX, HandPosY, HandPosZ, HandRotW, HandRotX, HandRotY, HandRotZ, ObjPosX, ObjPosY, ObjPosZ, ObjRotW, ObjRotX, ObjRotY, ObjRotZ, Control, ForceThumb, ForceIndex, ForceMiddle, ForceRing, ForceLittle");
                foreach (float[] sample in buffer)
                {
                    string row = "";
                    foreach (float val in sample)
                    {
                        row += $"{val}, ";
                    }
                    row = row.Substring(0, row.Length - 2); // remove last 2 elements from string
                    writer.WriteLine(row);
                }
            }
        }

        private string GetDataPath()
        {
#if UNITY_ANDROID
            return Application.persistentDataPath;
#elif UNITY_IPHONE
            return Application.persistentDataPath;
#else
            return Application.dataPath;
#endif
        }
    }
}