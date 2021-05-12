using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmputationLevel : MonoBehaviour
{
    public enum Amputation : uint {
        ShoulderDisarticulation = 0,
        TransHumeral = 1,
        TransRadial = 2,
        WristDisarticulation = 3,
        AbleBodied = 4,
    }

    public Amputation amputation = Amputation.TransHumeral;
    
    private GameObject avatar = null;
    private Dictionary<string, Transform> skins = new Dictionary<string, Transform>();

    private GameObject mpl = null;
    private Dictionary<string, Transform> parts = new Dictionary<string, Transform>();

    // Start is called before the first frame update
    void Start() {
        mpl = GameObject.Find( "rMPL" );
        parts = RecursiveAddChildren( mpl.transform, parts );

        avatar = GameObject.Find( "SkinMeshes" );
        // skins = RecursiveAddChildren( avatar.transform, skins );
    }

    // Update is called once per frame
    void Update() {
        // hide and show parts of the MPL
        SetActiveMPL( parts["rShoulderFlexAssembly"].gameObject, !( amputation > Amputation.ShoulderDisarticulation ) );
        SetActiveMPL( parts["rShoulderShell"].gameObject, !( amputation > Amputation.ShoulderDisarticulation ) );
        SetActiveMPL( parts["rHumeralRotator_Elbow"].gameObject, !( amputation > Amputation.TransHumeral ) );
        SetActiveMPL( parts["rForeArm"].gameObject, !( amputation > Amputation.TransRadial ) );
        SetActiveMPL( parts["rWristShell"].gameObject, !( amputation > Amputation.TransRadial ) );
        SetActiveMPL( parts["rWristDev"].gameObject, !( amputation > Amputation.TransRadial ) );

        // hide and show parts of the residual limb
        // skins["RightUpperArm1"].gameObject.GetComponent<SkinnedMeshRenderer>().enabled = !( amputation < Amputation.TransHumeral );
        // skins["RightUpperArm2"].gameObject.GetComponent<SkinnedMeshRenderer>().enabled = !( amputation < Amputation.TransRadial );
        // skins["RightArm1"].gameObject.GetComponent<SkinnedMeshRenderer>().enabled = !( amputation < Amputation.TransRadial );
        // skins["RightArm2"].gameObject.GetComponent<SkinnedMeshRenderer>().enabled = !( amputation < Amputation.WristDisarticulation );
        // skins["RightHand"].gameObject.GetComponent<SkinnedMeshRenderer>().enabled = !( amputation < Amputation.AbleBodied );
    }

    private Dictionary<string, Transform> RecursiveAddChildren( Transform t, Dictionary<string, Transform> dict ) {
        if ( t.childCount == 0 ) {
            dict.Add( t.name, t );
        } else {
            foreach( Transform tchild in t ) {
                dict = RecursiveAddChildren( tchild, dict );
            }
            dict.Add( t.name, t );
        }
        return dict;
    }

    private void SetActiveMPL( GameObject go, bool enable ) {
        go.GetComponent<MeshRenderer>().enabled = enable;
        foreach( Collider c in go.GetComponents<Collider>() ) {
            c.enabled = enable;
        }
    }
}
