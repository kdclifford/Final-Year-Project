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


    private void Start()
    {
        NodeHolder = GameObject.FindGameObjectWithTag("NodeHolder");
        ParentNode = NodeHolder.transform.Find("ParentNode").gameObject;
    }


    // Update is called once per frame
    void Update()
    {

        if (!isSpawned)
        {
            for (int i = 0; i < 4; i++)
            {
                // 60 width of item

                //newSpawn Position
                Vector3 pos = new Vector3(ParentNode.transform.position.x, ParentNode.transform.position.y - (20 * (i + 1)) , ParentNode.transform.position.z);
                //instantiate item
                GameObject SpawnedItem = Instantiate(nodePrefab, new Vector3(0,0,0), ParentNode.transform.rotation);
                //setParent
                SpawnedItem.transform.SetParent(NodeHolder.transform, false);
                SpawnedItem.transform.position = pos;

                //set name
                SpawnedItem.GetComponent<Text>().text = SpawnedItem.name  + i;

            }
            isSpawned = true;
        }





    }
}
