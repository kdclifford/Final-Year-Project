using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Parent Class of All the behaviour tree Nodes
public abstract class CNode
{
   protected CNode mParent;
    private string mNameOfNode;
  // protected Node mCurrentNode;
   protected ENodeState mCurrentNodeState;


    public abstract ENodeState ReturnNode();

    public string GetName()
    {
        return mNameOfNode;
    }
    public void SetName(string name)
    {
         mNameOfNode = name;
    }


}

public class CActionNode : CNode
{
    public delegate ENodeState mAction();
    mAction currentAction;

   public CActionNode( mAction PassedAction, string name)
    {
        currentAction = PassedAction;
        SetName(name);
    }

    public override ENodeState ReturnNode()
    {
        if(currentAction() == ENodeState.Failure)
        {
            Debug.Log("Failure " + GetName());
            return ENodeState.Failure;
        }
        else if (currentAction() == ENodeState.Success)
        {
            Debug.Log("Success " + GetName());
            return ENodeState.Success;
        }
        else
        {           
            Debug.Log("Running " + GetName());
            return ENodeState.Running;
        }
    }
}

public class CSelectorNode : CNode
{
   public List<CNode> childNodes = new List<CNode>();
    public CSelectorNode( List<CNode> PassedChildNodes, string name)
    {
        childNodes = PassedChildNodes;
        SetName(name);
    }


    public override ENodeState ReturnNode()
    {
        foreach(CNode nodes in childNodes)
        {
            if(nodes.ReturnNode() == ENodeState.Success)
            {
                Debug.Log("Success " + nodes.GetName());
                
                return ENodeState.Success;
            }
            else if (nodes.ReturnNode() == ENodeState.Running)
            {
                mCurrentNodeState = ENodeState.Running;
                mParent = this;
                Debug.Log("Running " + nodes.GetName());
                return ENodeState.Running;
            }


        }

        return ENodeState.Failure;
    }

}

public class CSequenceNode : CNode
{
    public List<CNode> childNodes = new List<CNode>();
    public CSequenceNode(List<CNode> PassedChildNodes, string name)
    {
        childNodes = PassedChildNodes;
        SetName(name);
    }


    public override ENodeState ReturnNode()
    {
        foreach (CNode nodes in childNodes)
        {
            if (nodes.ReturnNode() == ENodeState.Failure)
            {
                Debug.Log("Failure " + nodes.GetName());

                return ENodeState.Failure;
            }
            else if (nodes.ReturnNode() == ENodeState.Running)
            {
                mCurrentNodeState = ENodeState.Running;
                mParent = this;
                Debug.Log("Running " + nodes.GetName());
                return ENodeState.Running;
            }


        }

        return ENodeState.Success;
    }

}






//public class Tree : Node
//{
//    public List<Node> WholeTree;
//}

public class BehaviourTree : MonoBehaviour
{
    public GameObject[,] Map;
    private GameObject gameManager;
    private GameObject playerObject;
    private GameObject enemyObject;
    private FieldOfView fieldOfViewAI;
    private FootSteps playerTargetList;
    NavMeshAgent agent;
    
    int playerFootSteps;
    int playerList;
    private List<Transform> visibleTargets = new List<Transform>();

    private Transform targetLocation;




    CActionNode Hearing;
    CActionNode Sight;
    CActionNode MoveEnemy;


    CSequenceNode AttackSight;
    CSequenceNode AttackHeard;

    CSelectorNode Root;


    Vector3 t;






    ENodeState HearThePlayer()
    {
        if (playerFootSteps > 0)
        {
            for (int i = 0; i < playerFootSteps; i++)
            {
                if (playerTargetList.footStepTargets[i].transform == this.transform)
                {
                    targetLocation = playerObject.transform;
                    // Message with a GameObject name.
                    Debug.Log("I Hear the Player " + this.gameObject.name);

                    //transform.LookAt(playerObject.transform);
                    //coneColour.material = alertColour;
                    return ENodeState.Success;
                }
            }
        }
        return ENodeState.Failure;
    }

    ENodeState SeeThePlayer()
    {
        if (playerList > 0)
        {
            targetLocation = playerObject.transform;
            Debug.Log("I see the Player " + this.gameObject.name);
            return ENodeState.Success;
        }
        return ENodeState.Failure;
    }

    ENodeState MoveToPlayer()
    {        
        agent.SetDestination(targetLocation.position);
        if(Def.isPointInsideSphere(transform.position, playerObject.transform.position, 5.0f))
        {
            return ENodeState.Success;
        }
        return ENodeState.Failure;
    }






    // Start is called before the first frame update
    void Start()
    {
        enemyObject = this.gameObject;
        //gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        //Map = gameManager.GetComponent<GameManager>().floorList;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        // fieldOfViewAI = gameManager.GetComponent<GameManager>()
        agent = GetComponent<NavMeshAgent>();



        //tree Stuff
        Hearing = new CActionNode(HearThePlayer, "Hearing");
        Sight = new CActionNode(SeeThePlayer, "Sight");
        MoveEnemy = new CActionNode(MoveToPlayer, "MoveToPlayer");


        List<CNode> AttackIfSeenPlayer = new List<CNode>() { Sight, MoveEnemy };
        List<CNode> AttackIfHeardPlayer = new List<CNode>() { Hearing, MoveEnemy };

        AttackSight = new CSequenceNode(AttackIfSeenPlayer, "sightSequence");
        AttackHeard = new CSequenceNode(AttackIfHeardPlayer, "hearSequence");


        List<CNode> Tree = new List<CNode>() { AttackHeard, AttackSight };

        Root = new CSelectorNode(Tree, "Root");


    }

    // Update is called once per frame
    void LateUpdate()
    {
        visibleTargets = enemyObject.GetComponent<FieldOfView>().visibleTargets;
        playerTargetList = enemyObject.GetComponent<FieldOfView>().playerTargetList;

        playerList = visibleTargets.Count;
        playerFootSteps = playerTargetList.footStepTargets.Count;

        Root.ReturnNode();

       // Hearing.ReturnNode();


    }
}
