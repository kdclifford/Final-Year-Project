using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Node States
public enum NodeState
{
    Success,
    Running,
    Failure,
}

//Parent Class of All the behaviour tree Nodes
public abstract class Node
{
    public Node() { }

    private NodeState mCurrentNodeState;

    //Getter for the Current Node State
    public NodeState GetNodeState
    {
        get
        {
            return mCurrentNodeState;
        }
    }

    public NodeState SetNodeState
    {
        set
        {
            mCurrentNodeState = value;
        }
    }
    public abstract NodeState TreeStatus();

}

//Selector Node Derived from Parent class node
public class Selector : Node
{
    private List<Node> mChildNodes = new List<Node>();

    public Selector(List<Node> childNodes)
    {
        mChildNodes = childNodes;
    }

    public override NodeState TreeStatus()
    {
        foreach (Node loopNode in mChildNodes)
        {
            if (loopNode.GetNodeState == NodeState.Success)
            {
                SetNodeState = NodeState.Success;
                return GetNodeState;
            }
            else if (loopNode.GetNodeState == NodeState.Running)
            {
                SetNodeState = NodeState.Running;
                return GetNodeState;
            }
            else
            {
                continue;
            }
        }
        SetNodeState = NodeState.Failure;
        return GetNodeState;        
    }






}







public class BehaviourTree : MonoBehaviour
{

    public GameObject[,] Map;
    private GameObject gameManager;
    private GameObject playerObject;
    private GameObject enemyObject;
    private FieldOfView fieldOfViewAI;
    private FootSteps playerTargetList;


    int playerFootSteps;
    int playerList;
    private List<Transform> visibleTargets = new List<Transform>();

    bool HearThePlayer()
    {
        if (playerFootSteps > 0)
        {
            for (int i = 0; i < playerFootSteps; i++)
            {
                if (playerTargetList.footStepTargets[i].transform == this.transform)
                {
                    // Message with a GameObject name.
                    Debug.Log("I Hear the Player " + this.gameObject.name);

                    //transform.LookAt(playerObject.transform);
                    //coneColour.material = alertColour;
                    return true;
                }
            }
        }
        return false;
    }

    bool SeeThePlayer()
    {
        if (playerList > 0)
        {
            Debug.Log("I see the Player " + this.gameObject.name);
            return true;
        }
        return false;
    }


    // Start is called before the first frame update
    void Start()
    {
        enemyObject = this.gameObject;
        //gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        //Map = gameManager.GetComponent<GameManager>().floorList;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        // fieldOfViewAI = gameManager.GetComponent<GameManager>()
    }

    // Update is called once per frame
    void LateUpdate()
    {
        visibleTargets = enemyObject.GetComponent<FieldOfView>().visibleTargets;
        playerTargetList = enemyObject.GetComponent<FieldOfView>().playerTargetList;

        playerList = visibleTargets.Count;
        playerFootSteps = playerTargetList.footStepTargets.Count;

        if (SeeThePlayer() || HearThePlayer())
        {

        }


    }
}
