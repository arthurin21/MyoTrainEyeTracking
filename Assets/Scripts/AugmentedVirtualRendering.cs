using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentedVirtualRendering : MonoBehaviour
{
    public bool isAugmented = false; // true if in augmented reality, false else
    private GameObject roomEnvironment;
    private MeshRenderer [] renderers;

    // Start is called before the first frame update
    void Start()
    {
        roomEnvironment = GameObject.Find( "Room" );
        renderers = roomEnvironment.GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach( MeshRenderer mesh in renderers ) {
            mesh.enabled = !isAugmented;
        }
    }
}
