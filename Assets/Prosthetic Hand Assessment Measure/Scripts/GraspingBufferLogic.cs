using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViveSR.anipal.Eye
{
    public class GraspingBufferLogic : MonoBehaviour
    {
        private GraspingObjectLogic graspingLogic;

        void Awake()
        {
            graspingLogic = gameObject.transform.parent.gameObject.GetComponent<GraspingObjectLogic>();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void OnCollisionExit(Collision hit)
        {
            graspingLogic.RemoveCollision(hit.gameObject.name);
        }
    }
}