using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveTrackerFollower : MonoBehaviour
{
    private AmputationLevel amputation;

    private GameObject tracker = null;
    private GameObject mpl = null;

    private Transform mplCalibrators;
    private Dictionary<string, Transform> calibrators = new Dictionary<string, Transform>();

    // Start is called before the first frame update
    void Start()
    {
        amputation = GameObject.Find( "ConfigurationScripts" ).GetComponent<AmputationLevel>();

        tracker = GameObject.Find( "TrackerMPL" );
        mpl = GameObject.Find( "rMPL" );

        // add the calibrating transforms
        mplCalibrators = GameObject.Find( "MPLCalibrators" ).transform;
        calibrators.Add( "ShoulderDisarticulation", GameObject.Find( "MPLShoulderDisarticulationCalibrator" ).transform );
        calibrators.Add( "TransHumeral", GameObject.Find( "MPLTransHumeralCalibrator" ).transform );
        calibrators.Add( "TransRadial", GameObject.Find( "MPLTransRadialCalibrator" ).transform );
        calibrators.Add( "WristDisarticulation", GameObject.Find( "MPLWristDisarticulationCalibrator" ).transform );
    }

    // Update is called once per frame
    void Update()
    {
        switch( amputation.amputation ) {
            case AmputationLevel.Amputation.ShoulderDisarticulation:
                // make other calibrating transforms stick with this one
                calibrators["ShoulderDisarticulation"].SetParent( mplCalibrators );
                calibrators["TransHumeral"].SetParent( calibrators["ShoulderDisarticulation"] );
                calibrators["TransRadial"].SetParent( calibrators["ShoulderDisarticulation"] );
                calibrators["WristDisarticulation"].SetParent( calibrators["ShoulderDisarticulation"] );
                
                // make MPL arm move with this calibrator
                mpl.transform.SetParent( calibrators["ShoulderDisarticulation"] );
                calibrators["ShoulderDisarticulation"].position = tracker.transform.position;
                calibrators["ShoulderDisarticulation"].rotation = tracker.transform.rotation;
                break;
            case AmputationLevel.Amputation.TransHumeral:
                // make other calibrating transforms stick with this one
                calibrators["TransHumeral"].SetParent( mplCalibrators );
                calibrators["ShoulderDisarticulation"].SetParent( calibrators["TransHumeral"] );
                calibrators["TransRadial"].SetParent( calibrators["TransHumeral"] );
                calibrators["WristDisarticulation"].SetParent( calibrators["TransHumeral"] );

                // make MPL arm move with this calibrator
                mpl.transform.SetParent( calibrators["TransHumeral"] );
                calibrators["TransHumeral"].position = tracker.transform.position;
                calibrators["TransHumeral"].rotation = tracker.transform.rotation;
                break;
            case AmputationLevel.Amputation.TransRadial:
                // make other calibrating transforms stick with this one
                calibrators["TransRadial"].SetParent( mplCalibrators );
                calibrators["ShoulderDisarticulation"].SetParent( calibrators["TransRadial"] );
                calibrators["TransHumeral"].SetParent( calibrators["TransRadial"] );
                calibrators["WristDisarticulation"].SetParent( calibrators["TransRadial"] );

                // make MPL arm move with this calibrator
                mpl.transform.SetParent( calibrators["TransRadial"] );
                calibrators["TransRadial"].position = tracker.transform.position;
                calibrators["TransRadial"].rotation = tracker.transform.rotation;
                break;
            case AmputationLevel.Amputation.WristDisarticulation:
                // make other calibrating transforms stick with this one
                calibrators["WristDisarticulation"].SetParent( mplCalibrators );
                calibrators["ShoulderDisarticulation"].SetParent( calibrators["WristDisarticulation"] );
                calibrators["TransHumeral"].SetParent( calibrators["WristDisarticulation"] );
                calibrators["TransRadial"].SetParent( calibrators["WristDisarticulation"] );
                
                // make MPL arm move with this calibrator
                mpl.transform.SetParent( calibrators["WristDisarticulation"] );
                calibrators["WristDisarticulation"].position = tracker.transform.position;
                calibrators["WristDisarticulation"].rotation = tracker.transform.rotation;
                break;
        }
    }
}
