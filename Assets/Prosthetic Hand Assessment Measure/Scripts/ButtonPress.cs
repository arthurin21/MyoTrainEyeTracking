using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    private bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter( Collision hit ) {
        if ( hit.gameObject.name.Contains( "Palm" ) 
          || hit.gameObject.name.Contains( "Proximal" ) 
          || hit.gameObject.name.Contains( "Medial" ) 
          || hit.gameObject.name.Contains( "Distal" ) 
          || hit.gameObject.name.Contains( "MetaCarpal" ) 
          || hit.gameObject.name.Contains( "PlanetaryAsm" ) ) {
            pressed = true;
        }
    }

    public bool GetButtonStatus() {
        return pressed;
    }

    public void SetButtonStatus( bool status ) {
        pressed = status;
    }
}
