using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Def
{
    public static bool isPointInsideSphere(Vector3 point, Vector3 sphere, float radius)
    {
        // we are using multiplications because is faster than calling Math.pow
        float distance = Mathf.Sqrt((point.x - sphere.x) * (point.x - sphere.x) +
                                 (point.y - sphere.y) * (point.y - sphere.y) +
                                 (point.z - sphere.z) * (point.z - sphere.z));
        return distance < radius;
    }

    public static GameObject SpawnNodeUI(CNode spwanNodeUi, GameObject myPrefab)
    {
    GameObject NodeListUI;
    GameObject NodeTitle;

    Text nodetext;
    RawImage colourImg;


       

    NodeTitle = myPrefab.transform.GetChild(0).gameObject;
        
        
        colourImg = NodeTitle.GetComponent<RawImage>();


        if (spwanNodeUi.mCurrentNodeState == ENodeState.Success)
        {
            colourImg.color = new Vector4(0, 1, 0, 0.1f);
        }
        else if (spwanNodeUi.mCurrentNodeState == ENodeState.Failure)
        {
            colourImg.color = new Vector4(1,0,0, 0.1f);
        }
        else if (spwanNodeUi.mCurrentNodeState == ENodeState.Running)
        {
            colourImg.color = new Vector4(1, 0.92f, 0.016f, 0.1f);
        }

       // colourImg.color.a = 1;

        NodeTitle = myPrefab.transform.GetChild(0).GetChild(0).gameObject;
        nodetext = NodeTitle.GetComponent<Text>();

        nodetext.text = spwanNodeUi.GetName();

        NodeListUI = GameObject.Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0.0f, 0.0f, 0.0f));
        NodeTitle = NodeListUI.transform.GetChild(0).gameObject;


        NodeTitle.transform.localPosition = new Vector2(spwanNodeUi.mNodeUI.xPos, spwanNodeUi.mNodeUI.yPos);
        return NodeListUI;
    }

    //public static void UpdateNodeUI(CNode spwanNodeUi, GameObject myPrefab)
    //{   
    //    GameObject NodeListUI;
    //    GameObject NodeTitle;       

    //    Text nodetext;
    //    //private Text state;
    //    RawImage colourImg;



    //    NodeTitle = myPrefab.transform.GetChild(0).gameObject;
    //    NodeTitle.transform.localPosition = new Vector2(-500, 0);
    //    // NodeState = myPrefab.transform.GetChild(1).gameObject;
    //    colourImg = NodeTitle.GetComponent<RawImage>();

    //    //colour.color = Color.black;
    //    //string nodeStateText = "";
    //    if (spwanNodeUi.mCurrentNodeState == ENodeState.Success)
    //    {
    //        colourImg.color = Color.green;
    //    }
    //    else if (spwanNodeUi.mCurrentNodeState == ENodeState.Failure)
    //    {
    //        colourImg.color = Color.red;
    //    }
    //    else if (spwanNodeUi.mCurrentNodeState == ENodeState.Running)
    //    {
    //        colourImg.color = Color.yellow;
    //    }


    //    NodeTitle = myPrefab.transform.GetChild(0).GetChild(0).gameObject;
    //    nodetext = NodeTitle.GetComponent<Text>();
    //    // nodetext.text = "Node: " + spwanNodeUi.GetName();

    //    //nodetext = NodeState.GetComponent<Text>();
    //    nodetext.text = spwanNodeUi.GetName();
    //    //NodeTitle.t.position = new Vector3(110, 110, 110);

    //    NodeListUI = GameObject.Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0.0f, 0.0f, 0.0f));

    //}


    public static Color NodeColour (ENodeState state)
    {
        Color NodeText = Color.white;
        switch (state)
        {
            case ENodeState.Success:
                return NodeText = Color.green;
            case ENodeState.Failure:
                return NodeText = Color.red;
            case ENodeState.Running:
                return NodeText = Color.yellow;
        }
        return Color.white;
    }



}