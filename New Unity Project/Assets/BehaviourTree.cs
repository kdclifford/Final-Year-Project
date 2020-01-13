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

//Inverter Node Derived from Parent class node
public class Inverter : Node
{
    /* Child node to evaluate */
    private Node m_node;

    public Node node
    {
        get { return m_node; }
    }

    /* The constructor requires the child node that this inverter  decorator
     * wraps*/
    public Inverter(Node node)
    {
        m_node = node;
    }

    /* Reports a success if the child fails and
     * a failure if the child succeeeds. Running will report
     * as running */
    public override NodeState TreeStatus()
    {
        switch (m_node.TreeStatus())
        {
            case NodeState.Failure:
                SetNodeState = NodeState.Success;
                return GetNodeState;
            case NodeState.Success:
                SetNodeState = NodeState.Failure;
                return GetNodeState;
            case NodeState.Running:
                SetNodeState = NodeState.Running;
                return GetNodeState;
        }
        SetNodeState = NodeState.Success;
        return GetNodeState;
    }
}


public class Sequence : Node
{
    /** Chiildren nodes that belong to this sequence */
    private List<Node> m_nodes = new List<Node>();

    /** Must provide an initial set of children nodes to work */
    public Sequence(List<Node> nodes)
    {
        m_nodes = nodes;
    }

    /* If any child node returns a failure, the entire node fails. Whence all 
     * nodes return a success, the node reports a success. */
    public override NodeState TreeStatus()
    {
        bool anyChildRunning = false;

        foreach (Node node in m_nodes)
        {
            switch (node.TreeStatus())
            {
                case NodeState.Failure:
                    SetNodeState = NodeState.Failure;
                    return GetNodeState;
                case NodeState.Success:
                    continue;
                case NodeState.Running:
                    anyChildRunning = true;
                    continue;
                default:
                    SetNodeState = NodeState.Success;
                    return GetNodeState;
            }
        }
        SetNodeState = anyChildRunning ? NodeState.Running : NodeState.Success;
        return GetNodeState;
    }
}

public class ActionNode : Node
{
    /* Method signature for the action. */
    public delegate NodeState ActionNodeDelegate();

    /* The delegate that is called to evaluate this node */
    private ActionNodeDelegate m_action;

    /* Because this node contains no logic itself,
     * the logic must be passed in in the form of 
     * a delegate. As the signature states, the action
     * needs to return a NodeStates enum */
    public ActionNode(ActionNodeDelegate action)
    {
        m_action = action;
    }

    /* Evaluates the node using the passed in delegate and 
     * reports the resulting state as appropriate */
    public override NodeState TreeStatus()
    {
        switch (m_action())
        {
            case NodeState.Success:
                SetNodeState = NodeState.Success;
                return GetNodeState;
            case NodeState.Failure:
                SetNodeState = NodeState.Failure;
                return GetNodeState;
            case NodeState.Running:
                SetNodeState = NodeState.Running;
                return GetNodeState;
            default:
                SetNodeState = NodeState.Failure;
                return GetNodeState;
        }
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
