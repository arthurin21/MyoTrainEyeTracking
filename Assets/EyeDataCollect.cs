using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using ViveSR.anipal.Eye;
using System;
using System.IO;


namespace ViveSR.anipal.Eye
{
    public class EyeDataCollect : MonoBehaviour
    {
        public ViveSR.anipal.Eye.EyeData eyeData = new ViveSR.anipal.Eye.EyeData();
        private SRanipal_EyeFocusSample FocusSam;
        public Vertexes vertexes;
        public VerboseData verboseData;
        public float pupilDiameterLeft;
        public float pupilDiameterRight;
        public Vector2 pupilPositionLeft;
        public Vector2 pupilPositionRight;
        public Vector3 gazeOriginLeft;
        public Vector3 gazeOriginRight;
        public Vector3 gazeDirectionLeft;
        public Vector3 gazeDirectionRight;
        public Vector3 fixPointPos;
        public Vector3 objectPos;
        public float eyeOpenLeft;
        public float eyeOpenRight;
        public float timeFrame;
        public int frameNum;
        public Vector3 headPos;
        public Vector3 vertex1;
        public Vector3 vertex2;
        public Vector3 vertex3;
        public Vector3 vertex4;
        public Vector3 vertex5;
        public Vector3 vertex6;
        public Vector3 vertex7;
        public Vector3 vertex8;

        public enum LoggingStatus : uint
        {
            Clear = 0,
            Active = 1,
            Pause = 2
        }
        private LoggingStatus logStatus = LoggingStatus.Clear;
        private List<float[]> buffer = new List<float[]>();




        // Start is called before the first frame update
        private void Start()
        {
            FocusSam = GameObject.Find("Focus Sample").GetComponent<SRanipal_EyeFocusSample>();
            vertexes = GameObject.Find("headbox").GetComponent<Vertexes>();
        }




        // Update is called once per frame
        void Update()
        {
            switch (logStatus)
            {
                case LoggingStatus.Clear:
                    buffer.Clear();     // clear buffer
                    timeFrame = 0.0f; // update current time
                    break;
                case LoggingStatus.Active:
                    SRanipal_Eye.GetVerboseData(out verboseData);

                    pupilDiameterLeft = verboseData.left.pupil_diameter_mm / 1000;
                    pupilDiameterRight = verboseData.left.pupil_diameter_mm / 1000;
                    pupilPositionLeft = verboseData.left.pupil_position_in_sensor_area;
                    pupilPositionRight = verboseData.right.pupil_position_in_sensor_area;
                    gazeOriginLeft = verboseData.left.gaze_origin_mm / 1000;
                    gazeOriginRight = verboseData.right.gaze_origin_mm / 1000;
                    gazeDirectionLeft = verboseData.left.gaze_direction_normalized;
                    gazeDirectionRight = verboseData.right.gaze_direction_normalized;
                    fixPointPos = FocusSam.fixedPoint;
                    objectPos = GameObject.Find("Cylinder").transform.position;
                    eyeOpenLeft = verboseData.left.eye_openness;
                    eyeOpenRight = verboseData.right.eye_openness;
                    headPos = Camera.main.transform.position;
                    vertex1 = vertexes.points[0];
                    vertex2 = vertexes.points[1];
                    vertex3 = vertexes.points[2];
                    vertex4 = vertexes.points[3];
                    vertex5 = vertexes.points[4];
                    vertex6 = vertexes.points[5];
                    vertex7 = vertexes.points[6];
                    vertex8 = vertexes.points[7];
                    timeFrame += Time.deltaTime;
                    frameNum = Time.frameCount;

                    float[] row = new float[55];
                    row[0] = timeFrame;
                    row[1] = frameNum;
                    row[2] = pupilDiameterLeft;
                    row[3] = pupilDiameterRight;
                    row[4] = pupilPositionLeft.x;
                    row[5] = pupilPositionLeft.y;
                    row[6] = pupilPositionRight.x;
                    row[7] = pupilPositionRight.y;
                    row[8] = eyeOpenLeft;
                    row[9] = eyeOpenRight;
                    row[10] = gazeOriginLeft.x;
                    row[11] = gazeOriginLeft.y;
                    row[12] = gazeOriginLeft.z;
                    row[13] = gazeOriginRight.x;
                    row[14] = gazeOriginRight.y;
                    row[15] = gazeOriginRight.z;
                    row[16] = gazeDirectionLeft.x;
                    row[17] = gazeDirectionLeft.y;
                    row[18] = gazeDirectionLeft.z;
                    row[19] = gazeDirectionRight.x;
                    row[20] = gazeDirectionRight.y;
                    row[21] = gazeDirectionRight.z;
                    row[22] = fixPointPos.x;
                    row[23] = fixPointPos.y;
                    row[24] = fixPointPos.z;
                    row[25] = objectPos.x;
                    row[26] = objectPos.y;
                    row[27] = objectPos.z;
                    row[28] = headPos.x;
                    row[29] = headPos.y;
                    row[30] = headPos.z;
                    row[31] = vertex1.x;
                    row[32] = vertex1.y;
                    row[33] = vertex1.z;
                    row[34] = vertex2.x;
                    row[35] = vertex2.y;
                    row[36] = vertex2.z;
                    row[37] = vertex3.x;
                    row[38] = vertex3.y;
                    row[39] = vertex3.z;
                    row[40] = vertex4.x;
                    row[41] = vertex4.y;
                    row[42] = vertex4.z;
                    row[43] = vertex5.x;
                    row[44] = vertex5.y;
                    row[45] = vertex5.z;
                    row[46] = vertex6.x;
                    row[47] = vertex6.y;
                    row[48] = vertex6.z;
                    row[49] = vertex7.x;
                    row[50] = vertex7.y;
                    row[51] = vertex7.z;
                    row[52] = vertex8.x;
                    row[53] = vertex8.y;
                    row[54] = vertex8.z;

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
            string filedir = Path.Combine(Application.dataPath, "EyeData", System.DateTime.Today.ToString("yyyy-MM-dd"));
            Debug.Log(filedir);
            if (!Directory.Exists(filedir))
            {
                Directory.CreateDirectory(filedir);
            }
            int task = 0;
            string filename;
            do { 
                filename = Path.Combine(filedir, $"{System.DateTime.Today.ToString("yyyy-MM-dd")}_eyeTrackingData.csv");
                task += 1;
            }while (File.Exists(filename));

            using (StreamWriter writer = new StreamWriter(new FileStream(filename, FileMode.Create, FileAccess.Write)))
            {
                writer.WriteLine("sep=,");
                writer.WriteLine("Time:UnityTime, Time:FrameNum, E:Automatic__LeftPupil:Diameter, E:Automatic__RightPupil:Diameter, E:Automatic__LeftPupilNormPos:Point2D:X, E:Automatic__LeftPupilNormPos:Point2D:Y, E:Automatic__RightPupilNormPos:Point2D:X, E:Automatic__RightPupilNormPos:Point2D:Y, E:Automatic_LeftPupil:EyeOpeness, E:Automatic_RightPupil:EyeOpeness, GV:VRGV:Origin:Left:Point3D:X, GV:VRGV:Origin:Left:Point3D:Y, GV:VRGV:Origin:Left:Point3D:Z, GV:VRGV:Origin:Right:Point3D:X, GV:VRGV:Origin:Right:Point3D:Y, GV:VRGV:Origin:Right:Point3D:Z, GV:VRGV:NormDirection:Left:Point3D:X, GV:VRGV:NormDirection:Left:Point3D:Y, GV:VRGV:NormDirection:Left:Point3D:Z, GV:VRGV:NormDirection:Right:Point3D:X, GV:VRGV:NormDirection:Right:Point3D:Y, GV:VRGV:NormDirection:Right:Point3D:Z, GV:VRGV:FixPoint:Point3D:X, GV:VRGV:FixPoint:Point3D:Y, GV:VRGV:FixPoint:Point3D:Z, M:TargetPos:Point3D:X, M:TargetPos:Point3D:Y, M:TargetPos:Point3D:Z, O:HeadObj:Center:Point3D:X, O:HeadObj:Center:Point3D:Y, O:HeadObj:Center:Point3D:Z, O:HeadObj:Vertex1:Point3D:X, O:HeadObj:Vertex1:Point3D:Y, O:HeadObj:Vertex1:Point3D:Z, O:HeadObj:Vertex2:Point3D:X, O:HeadObj:Vertex2:Point3D:Y, O:HeadObj:Vertex2:Point3D:Z, O:HeadObj:Vertex3:Point3D:X, O:HeadObj:Vertex3:Point3D:Y, O:HeadObj:Vertex3:Point3D:Z, O:HeadObj:Vertex4:Point3D:X, O:HeadObj:Vertex4:Point3D:Y, O:HeadObj:Vertex4:Point3D:Z, O:HeadObj:Vertex5:Point3D:X, O:HeadObj:Vertex5:Point3D:Y, O:HeadObj:Vertex5:Point3D:Z, O:HeadObj:Vertex6:Point3D:X, O:HeadObj:Vertex6:Point3D:Y, O:HeadObj:Vertex6:Point3D:Z, O:HeadObj:Vertex7:Point3D:X, O:HeadObj:Vertex7:Point3D:Y, O:HeadObj:Vertex7:Point3D:Z, O:HeadObj:Vertex8:Point3D:X, O:HeadObj:Vertex8:Point3D:Y, O:HeadObj:Vertex8:Point3D:Z");
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
    }
}
