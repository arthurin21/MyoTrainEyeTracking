using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarLeftArmIK : MonoBehaviour
{
    // private Transform TrackerTrunk;
    private Transform TrackerAbleArm;
    private Transform TrackerLeftFoot;
    private Transform TrackerRightFoot;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // TrackerTrunk = GameObject.Find( "TrackerTrunk" ).transform;
        TrackerAbleArm = GameObject.Find( "TrackerAbleArm" ).transform;
        TrackerLeftFoot = GameObject.Find( "TrackerLeftFoot" ).transform;
        TrackerRightFoot = GameObject.Find( "TrackerRightFoot" ).transform;
    }

    void OnAnimatorIK(int layerIndex)
    {
        // float hips = animator.GetFloat( "Hips" );
        float reach = animator.GetFloat("AbleArm");
        float Lfoot = animator.GetFloat("LeftFoot");
        float Rfoot = animator.GetFloat("RightFoot");

        // animator.SetIKPositionWeight( AvatarIKGoal.RightHip, hips );
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, Lfoot);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, Rfoot);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, TrackerAbleArm.position);
        animator.SetIKPosition(AvatarIKGoal.LeftFoot, TrackerLeftFoot.position);
        animator.SetIKPosition(AvatarIKGoal.RightFoot, TrackerRightFoot.position);
    }
}
