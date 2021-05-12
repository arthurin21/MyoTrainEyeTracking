using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TrackerConfig : MonoBehaviour
{
    // needed to complete
    public string trackerSerialPHAM;
    public string trackerSerialMPL;

    // needed to move the avatar
    public string trackerSerialTrunk;
    public string trackerSerialAbleArm;
    public string trackerSerialLeftFoot;
    public string trackerSerialRightFoot;

    // Start is called before the first frame update
    void Start()
    {
        var error = ETrackedPropertyError.TrackedProp_Success;
        for ( uint i = 0; i < 16; i++ ) {
            var result = new System.Text.StringBuilder((int)64);
            OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_SerialNumber_String, result, OpenVR.k_unMaxPropertyStringSize, ref error);
            
            string serialNumber = result.ToString();
            if ( serialNumber == trackerSerialPHAM ) {
                GameObject.Find( "TrackerPHAM" ).GetComponent<SteamVR_TrackedObject>().index = ( SteamVR_TrackedObject.EIndex ) i;
            } else if ( serialNumber == trackerSerialMPL ) {
                GameObject.Find( "TrackerMPL" ).GetComponent<SteamVR_TrackedObject>().index = ( SteamVR_TrackedObject.EIndex ) i;
            } else if ( serialNumber == trackerSerialTrunk ) {
                GameObject.Find( "TrackerTrunk" ).GetComponent<SteamVR_TrackedObject>().index = ( SteamVR_TrackedObject.EIndex ) i;
            } else if ( serialNumber == trackerSerialAbleArm ) {
                GameObject.Find( "TrackerAbleArm" ).GetComponent<SteamVR_TrackedObject>().index = ( SteamVR_TrackedObject.EIndex ) i;
            } else if ( serialNumber == trackerSerialLeftFoot ) {
                GameObject.Find( "TrackerLeftFoot" ).GetComponent<SteamVR_TrackedObject>().index = ( SteamVR_TrackedObject.EIndex ) i;
            } else if ( serialNumber == trackerSerialRightFoot ) {
                GameObject.Find( "TrackerRightFoot" ).GetComponent<SteamVR_TrackedObject>().index = ( SteamVR_TrackedObject.EIndex ) i;
            } else {
                // skip this device
            }
        }
    }
}
