using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViveSR.anipal.Eye
{
    public class Cubie : MonoBehaviour
    {
        private Renderer Renderer;
        // Start is called before the first frame update
        private void Awake()
        {
            Renderer = GetComponent<Renderer>();
            Targeted("");
        }


        // Update is called once per frame
        public void Targeted(string name)
        {
            if (name == this.gameObject.name)
            {
                Renderer.material.color = Color.yellow;




            }


        }
    }
}




