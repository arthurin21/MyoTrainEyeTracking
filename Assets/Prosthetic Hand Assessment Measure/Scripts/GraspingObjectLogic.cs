using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViveSR.anipal.Eye
{
    public class GraspingObjectLogic : MonoBehaviour
    {
        public PHAM_ManagerPro.ObjectPrimitive primitive;
        public Dictionary<string, Collision> collisions = new Dictionary<string, Collision>();

        private List<string> validCollisions;
        private bool createdJoint = false;

        void Awake()
        {
            switch (primitive)
            {
                case PHAM_ManagerPro.ObjectPrimitive.Cylinder:
                    validCollisions = new List<string>(new string[] { "Palm", "ThDistal", "IndDistal", "MidDistal", "RingDistal", "LittleDistal" });
                    break;
                case PHAM_ManagerPro.ObjectPrimitive.Card:
                    validCollisions = new List<string>(new string[] { "ThDistal", "IndMedial" });
                    break;
                case PHAM_ManagerPro.ObjectPrimitive.Stick:
                    validCollisions = new List<string>(new string[] { "ThDistal", "IndDistal" });
                    break;
                case PHAM_ManagerPro.ObjectPrimitive.Tripod:
                    validCollisions = new List<string>(new string[] { "ThDistal", "IndDistal", "MidDistal" });
                    break;
            }
        }

        // Use this for initialization
        void Start()
        {
        }

        public void OnCollisionStay(Collision hit)
        {
            foreach (string valid in validCollisions)
            {
                if (hit.gameObject.name.Contains(valid))
                {
                    if (!collisions.ContainsKey(hit.gameObject.name))
                    {
                        collisions.Add(hit.gameObject.name, hit);
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (collisions.Count >= validCollisions.Count)
            {
                if (!createdJoint)
                {
                    foreach (KeyValuePair<string, Collision> kvp in collisions)
                    {
                        FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
                        fixedJoint.connectedBody = kvp.Value.gameObject.GetComponent<Rigidbody>();
                        fixedJoint.connectedAnchor = Vector3.zero;
                    }
                    createdJoint = true;
                }
            }
            else
            {
                ClearFixedJoints();
            }
        }

        public void ClearFixedJoints()
        {
            FixedJoint[] fixedJoints = gameObject.GetComponents<FixedJoint>();
            if (fixedJoints.Length > 0)
            {
                foreach (FixedJoint joint in fixedJoints)
                {
                    Destroy(joint);
                }
                collisions.Clear();
                createdJoint = false;
            }
        }

        public void RemoveCollision(string toRemove)
        {
            if (collisions.ContainsKey(toRemove))
            {
                collisions.Remove(toRemove);
            }
        }
    }
}
