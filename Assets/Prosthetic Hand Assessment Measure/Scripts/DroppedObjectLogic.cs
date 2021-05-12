using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroppedObjectLogic : MonoBehaviour
{
    // public bool successfulActivation;

    private GameObject floor;
    private bool droppedObject = false;

    void OnCollisionEnter( Collision other )
    {
        // Reset if object hits floor
        if ( other.gameObject.name == "Floor" )
        {
            Debug.Log( "Dropped object! This task is over!" );
            droppedObject = true;
        }
    }

    public bool GetDroppedStatus() {
        return droppedObject;
    }

    public void SetDroppedStatus( bool status ) {
        droppedObject = status;
    }
}
