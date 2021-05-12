using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViveSR.anipal.Eye
{
    public class Vertexes : MonoBehaviour
    {
        public Vector3[] points;
        public BoxCollider headbox;
        // Start is called before the first frame update
        void Start()
        {
            headbox = this.gameObject.GetComponent<BoxCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            points = GetVertexPos(headbox);
        }

        public Vector3[] GetVertexPos(BoxCollider boxcollider)
        {
            var vertices = new Vector3[9];

            vertices[0] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(-boxcollider.size.x, -boxcollider.size.y, -boxcollider.size.z) * 0.5f);
            vertices[1] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, -boxcollider.size.y, -boxcollider.size.z) * 0.5f);
            vertices[2] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(-boxcollider.size.x, -boxcollider.size.y, boxcollider.size.z) * 0.5f);
            vertices[3] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, -boxcollider.size.y, boxcollider.size.z) * 0.5f);

            vertices[4] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(-boxcollider.size.x, boxcollider.size.y, -boxcollider.size.z) * 0.5f);
            vertices[5] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, boxcollider.size.y, -boxcollider.size.z) * 0.5f);
            vertices[6] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(-boxcollider.size.x, boxcollider.size.y, boxcollider.size.z) * 0.5f);
            vertices[7] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, boxcollider.size.y, boxcollider.size.z) * 0.5f);

            vertices[8] = boxcollider.transform.TransformPoint(boxcollider.center);
            return vertices;



        }
    }
}
