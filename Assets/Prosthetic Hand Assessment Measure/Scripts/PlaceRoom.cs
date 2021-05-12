using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceRoom : MonoBehaviour
{
    GameObject trackerPHAM;

    // Start is called before the first frame update
    void Start()
    {
        trackerPHAM = GameObject.Find( "TrackerPHAM" );
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = gameObject.transform.position;
        Vector3 newPosition = new Vector3( currentPosition.x, trackerPHAM.transform.position.y, currentPosition.z );
        gameObject.transform.position = newPosition;
    }
}
