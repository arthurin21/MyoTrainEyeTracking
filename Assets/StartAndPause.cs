using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViveSR.anipal.Eye
{
    public class StartAndPause : MonoBehaviour
    {
        private EyeDataCollect edc = null;
        // Start is called before the first frame update
        void Start()
        {
            edc = FindObjectOfType<EyeDataCollect>();
            edc.SetLogging(EyeDataCollect.LoggingStatus.Clear);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                edc.SetLogging(EyeDataCollect.LoggingStatus.Active);

            }

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {

                edc.SetLogging(EyeDataCollect.LoggingStatus.Pause);                    // pause data logging
                edc.Publish();                                                       // publish to file
                edc.SetLogging(EyeDataCollect.LoggingStatus.Clear);                    // clear data log


            }
        }
    }
}