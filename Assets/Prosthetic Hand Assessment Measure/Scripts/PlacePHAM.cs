using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePHAM : MonoBehaviour
{
        public const float floorOffset = 0.17f;
       private GameObject trackerPHAM;
        Quaternion initialRotation;
        Quaternion offsetRotation;

    // Start is called before the first frame update
    void Start()
    {
        trackerPHAM = GameObject.Find( "TrackerPHAM" );
        initialRotation = trackerPHAM.transform.rotation;
        offsetRotation = Quaternion.Euler( 90, 0, 0 );
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.R ) ) {
            initialRotation = trackerPHAM.transform.rotation;
        }

        Quaternion currentRotation = Quaternion.Inverse( initialRotation ) * trackerPHAM.transform.rotation * offsetRotation;
        Vector3 currentEulerAngles = currentRotation.eulerAngles;
        gameObject.transform.rotation = Quaternion.Euler( 0, currentEulerAngles.z, 0 );

    
        // Vector3 newPosition = new Vector3( trackerPHAM.transform.position.x, floorOffset, trackerPHAM.transform.position.z );
        // gameObject.transform.position = newPosition;
        gameObject.transform.position = trackerPHAM.transform.position;
    }
}
