using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NodeList : MonoBehaviour
{
    public List<CNode> ChildNodes = new List<CNode>();
    public List<string> UiList = new List<string>();
    public GameObject nodePrefab;
    public GameObject NodeHolder;
    public bool isSpawned = false;
    public GameObject ParentNode;

    public CNode SelectedTarget;
    public List<CNode> childNodes;

    public List<GameObject> SpawnedItem = new List<GameObject>();

    private void Start()
    {
        NodeHolder = GameObject.FindGameObjectWithTag("NodeHolder");
        ParentNode = NodeHolder.transform.Find("ParentNode").gameObject;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<MouseClick>().currentNode != null)
        {
            SelectedTarget = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MouseClick>().currentNode;

            childNodes = SelectedTarget.GetChildren();
            if (childNodes != null)
            {
                if (!isSpawned)
                {
                   
                    for (int i = 0; i < 6; i++)
                    {
                        // 60 width of item

                        //newSpawn Position
                        Vector3 pos = new Vector3(ParentNode.transform.position.x, ParentNode.transform.position.y - (20 * (i + 1)), ParentNode.transform.position.z);
                        //instantiate item
                        GameObject childNode = Instantiate(nodePrefab, new Vector3(0, 0, 0), ParentNode.transform.rotation);
                        //setParent
                        childNode.transform.SetParent(NodeHolder.transform, false);
                        childNode.transform.position = pos;

                        //set name
                        //childNode.GetComponent<Text>().text = n.GetName();
                        SpawnedItem.Add(childNode);

                    }
                    isSpawned = true;
                }
            }

            if(isSpawned)
            {

                if (childNodes != null)
                {
                    if(childNodes.Count > SpawnedItem.Count )
                    {
                        GameObject childNode = Instantiate(nodePrefab, new Vector3(0, 0, 0), ParentNode.transform.rotation);
                        childNode.transform.SetParent(NodeHolder.transform, false);
                        childNode.transform.position = new Vector3(SpawnedItem[SpawnedItem.Count].transform.position.z, SpawnedItem[SpawnedItem.Count].transform.position.y - 20, SpawnedItem[SpawnedItem.Count].transform.position.z);
                        SpawnedItem.Add(childNode);
                    }


                    int i = 0;
                    foreach (GameObject g in SpawnedItem)
                    {
                        if (childNodes.Count > i)
                        {
                            g.GetComponent<Text>().text = childNodes[i].GetName();
                            g.GetComponent<Text>().color = Def.NodeColour(childNodes[i].mCurrentNodeState);
                        }
                        else
                        {
                            g.GetComponent<Text>().text = "";
                            //g.GetComponent<Text>().color = Color.clear;
                        }
                        i++;
                    }
                }
            }


        }




    }
}
