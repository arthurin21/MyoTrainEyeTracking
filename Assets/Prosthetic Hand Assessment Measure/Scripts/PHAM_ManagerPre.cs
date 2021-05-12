using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHAM_ManagerPre : MonoBehaviour
{
    private static GameObject[] vholders;
    private static GameObject[] hholders;
    public static int objects;
    public static int[] tasks;
    private static int current_task_indx;
    public static PHAM_ManagerPre instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        vholders = new GameObject[6];
        hholders = new GameObject[6];
        int vcounter = 0;
        int hcounter = 0;
        //Add all the available vertical holders to the manager
        for (int i = 8; i < 14; i++)
        {
            if (gameObject.transform.GetChild(i).name.Contains("VHolder"))
            {
                vholders[vcounter] = gameObject.transform.GetChild(i).gameObject;
                vcounter++;
            }
        }
        //Debug.Log(vcounter);
        //Add all the available horizontal holders to the manager
        for (int i = 2; i < 8; i++)
        {
            if (gameObject.transform.GetChild(i).name.Contains("HHolder"))
            {
                hholders[hcounter] = gameObject.transform.GetChild(i).gameObject;
                hcounter++;
            }
        }
        //Debug.Log(hcounter);

        nextTask();
    }

    public static void ColorHolder()
    {

        //GameObject.Find("CylinderPrimitive").GetComponent<PHAM_CylinderNew>().defSuccess();
        //GameObject.Find("Card").GetComponent<PHAM_CardNew>().defSuccess();
        //GameObject.Find("Stick").GetComponent<PHAM_StickNew>().defSuccess();
        //GameObject.Find("Tripod").GetComponent<PHAM_TripodNew>().defSuccess();
        Vector3 inst_pos = new Vector3(0, 0, 0);
        Vector3 FAR_AWAY = new Vector3(10000,10000,10000);
        GameObject.Find("CylinderPrimitive").transform.position = FAR_AWAY;
        GameObject.Find("Card").transform.position = FAR_AWAY;
        GameObject.Find("Stick").transform.position = FAR_AWAY;
        GameObject.Find("Tripod").transform.position = FAR_AWAY;
        int judge = Random.Range(0,2);
        int initial = Random.Range(0, 6);
        int target = Random.Range(0, 6);
        objects = Random.Range(1, 5);
        if (judge==1)
        {
            //Clear the colors of all holders
            hholders[0].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            hholders[0].GetComponent<Holder>().deactivate();
            hholders[1].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            hholders[1].GetComponent<Holder>().deactivate();
            hholders[2].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            hholders[2].GetComponent<Holder>().deactivate();
            hholders[3].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            hholders[3].GetComponent<Holder>().deactivate();
            hholders[4].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            hholders[4].GetComponent<Holder>().deactivate();
            hholders[5].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            hholders[5].GetComponent<Holder>().deactivate();
            vholders[0].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            vholders[0].GetComponent<Holder>().deactivate();
            vholders[1].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            vholders[1].GetComponent<Holder>().deactivate();
            vholders[2].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            vholders[2].GetComponent<Holder>().deactivate();
            vholders[3].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            vholders[3].GetComponent<Holder>().deactivate();
            vholders[4].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            vholders[4].GetComponent<Holder>().deactivate();
            vholders[5].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            vholders[5].GetComponent<Holder>().deactivate();



            inst_pos = hholders[initial].transform.position;
            switch (objects) {
                case 1:
                    GameObject.Find("CylinderPrimitive").transform.rotation = Quaternion.Euler(0, 0, 90);
                    GameObject.Find("CylinderPrimitive").transform.position = inst_pos + new Vector3(0, 0, -.75f);
                    vholders[target].GetComponent<Renderer>().material.color = new Color(.8f, .03f, .02f);
                    break;

                case 2:
                    GameObject.Find("Card").transform.rotation = Quaternion.Euler(0, 0, 90);
                    GameObject.Find("Card").transform.position = inst_pos + new Vector3(.8795f, -.038f, -.8f);
                    vholders[target].GetComponent<Renderer>().material.color = new Color(22/255f, 250/255f, 8/255f, 1.0f);
                    break;

                case 3:
                    GameObject.Find("Stick").transform.rotation = Quaternion.Euler(0, 0, 90);
                    GameObject.Find("Stick").transform.position = inst_pos + new Vector3(-.5885f, .251f, -.431f);
                    vholders[target].GetComponent<Renderer>().material.color = new Color(251/255f, 1.0f, 0.0f, 1.0f);
                    break;

                case 4:
                    GameObject.Find("Tripod").transform.rotation = Quaternion.Euler(90, 0, 30);
                    GameObject.Find("Tripod").transform.position = inst_pos + new Vector3(.8095f, .656f, -.456f);
                    vholders[target].GetComponent<Renderer>().material.color = new Color(0,10/255f,1,1);
                    break;

            }
            vholders[target].GetComponent<Holder>().activate();

        }
        else {

            //Clear the colors of all holders
            hholders[0].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            hholders[0].GetComponent<Holder>().deactivate();
            hholders[1].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            hholders[1].GetComponent<Holder>().deactivate();
            hholders[2].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            hholders[2].GetComponent<Holder>().deactivate();
            hholders[3].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            hholders[3].GetComponent<Holder>().deactivate();
            hholders[4].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            hholders[4].GetComponent<Holder>().deactivate();
            hholders[5].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            hholders[5].GetComponent<Holder>().deactivate();
            vholders[0].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            vholders[0].GetComponent<Holder>().deactivate();
            vholders[1].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            vholders[1].GetComponent<Holder>().deactivate();
            vholders[2].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            vholders[2].GetComponent<Holder>().deactivate();
            vholders[3].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            vholders[3].GetComponent<Holder>().deactivate();
            vholders[4].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            vholders[4].GetComponent<Holder>().deactivate();
            vholders[5].GetComponent<Renderer>().material.color = new Color(.8f, .8f, .8f);
            vholders[5].GetComponent<Holder>().deactivate();


            inst_pos = vholders[initial].transform.position;
            
            switch (objects)
            {
                case 1:
                    GameObject.Find("CylinderPrimitive").transform.rotation = Quaternion.Euler(0, 0, 0);
                    GameObject.Find("CylinderPrimitive").transform.position = inst_pos + new Vector3(0, 0, -.5f);
                    hholders[target].GetComponent<Renderer>().material.color = new Color(.8f, .03f, .02f);
                    break;

                case 2:
                    GameObject.Find("Card").transform.rotation = Quaternion.Euler(0, 0, 0);
                    GameObject.Find("Card").transform.position = inst_pos + new Vector3(0, .8325f, -.777f);
                    hholders[target].GetComponent<Renderer>().material.color = new Color(22 / 255f, 250 / 255f, 8 / 255f, 1.0f);
                    break;

                case 3:
                    GameObject.Find("Stick").transform.rotation = Quaternion.Euler(0, 0, 0);
                    GameObject.Find("Stick").transform.position = inst_pos + new Vector3(0, -.8425f, -.881f);
                    hholders[target].GetComponent<Renderer>().material.color = new Color(251 / 255f, 1.0f, 0.0f, 1.0f);
                    break;

                case 4:
                    GameObject.Find("Tripod").transform.rotation = Quaternion.Euler(0, 90,90);
                    GameObject.Find("Tripod").transform.position = inst_pos + new Vector3(.258f, 1.0945f, -.767f);
                    hholders[target].GetComponent<Renderer>().material.color = new Color(0, 10 / 255f, 1, 1);
                    break;

            }
           
            hholders[target].GetComponent<Holder>().activate();




        }
    }

    public static int whichObj()
    {

        return objects;

    }
    public static void nextTask()
    {
        //current_task_indx++;
        ColorHolder();
    }

}
