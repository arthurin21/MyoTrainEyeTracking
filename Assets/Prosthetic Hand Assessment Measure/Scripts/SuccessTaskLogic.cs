using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessTaskLogic : MonoBehaviour
{
    private bool success = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter( Collider other ) {
        if ( other.gameObject.name.Contains( "Holder") && other.gameObject.GetComponent<Holder>().isActivated() ) {
            success = true;
        }
    }

    void OnTriggerExit( Collider other ) {
        if ( other.gameObject.name.Contains( "Holder") && other.gameObject.GetComponent<Holder>().isActivated() ) {
            success = false;
        }
    }

    public bool GetSuccessStatus() {
        return success;
    }

    public void SetSuccessStatus( bool status ) {
        success = status;
    }
}
