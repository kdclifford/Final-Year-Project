using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Parent Class of All the behaviour tree Nodes
public abstract class CNode
{
   protected CNode mParent;
    //protected CNode mCurrentNode;
    private string mNameOfNode;
   protected ENodeState mCurrentNodeState;


    public abstract ENodeState RunTree();

    public string GetName()
    {
        return mNameOfNode;
    }
    public void SetName(string name)
    {
         mNameOfNode = name;
    }
    public CNode GetParent()
    {
        return mParent;
    }
    public void SetParent(CNode parent)
    {
        mParent = parent;
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
    

    public override ENodeState RunTree()
    {
            //mCurrentNode = this;
        if(currentAction() == ENodeState.Failure)
        {
            mCurrentNodeState = ENodeState.Failure;
            Debug.Log("Failure " + GetName());
            return ENodeState.Failure;
        }
        else if (currentAction() == ENodeState.Success)
        {
            mCurrentNodeState = ENodeState.Success;
            Debug.Log("Success " + GetName());
            return ENodeState.Success;
        }
        else
        {
            mCurrentNodeState = ENodeState.Running;
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


    public override ENodeState RunTree()
    {
        foreach(CNode nodes in childNodes)
        {
           // mCurrentNode = nodes;
            nodes.SetParent(this);
            if (nodes.RunTree() == ENodeState.Success)
            {
                mCurrentNodeState = ENodeState.Success;
                Debug.Log("Success " + nodes.GetName());
                
                return ENodeState.Success;
            }
            else if (nodes.RunTree() == ENodeState.Running)
            {
                mCurrentNodeState = ENodeState.Running;
                //mParent = this;
                Debug.Log("Running " + nodes.GetName());
                return ENodeState.Running;
            }


        }
        mCurrentNodeState = ENodeState.Failure;
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


    public override ENodeState RunTree()
    {
        foreach (CNode nodes in childNodes)
        {
            //mCurrentNode = nodes;
            nodes.SetParent(this);
            if (nodes.RunTree() == ENodeState.Failure)
            {
                mCurrentNodeState = ENodeState.Failure;
                Debug.Log("Failure " + nodes.GetName());

                return ENodeState.Failure;
            }
            else if (nodes.RunTree() == ENodeState.Running)
            {
                mCurrentNodeState = ENodeState.Running;
                //mParent = this;
                Debug.Log("Running " + nodes.GetName());
                return ENodeState.Running;
            }


        }
        mCurrentNodeState = ENodeState.Success;
        return ENodeState.Success;
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
    NavMeshAgent agent;
    
    int playerFootSteps;
    int playerList;
    private List<Transform> visibleTargets = new List<Transform>();

    public Vector3 targetLocation;

    public List<GameObject> patrolPts = new List<GameObject>();

    CActionNode Hearing;
    CActionNode Sight;
    CActionNode MoveEnemy;
    CActionNode Movept;


    CSequenceNode AttackSight;
    CSequenceNode AttackHeard;
    CSequenceNode patrol;

    CSelectorNode Root;


    Vector3 t;


    public int currentPatrolPt;





    ENodeState HearThePlayer()
    {
        if (playerFootSteps > 0)
        {
            for (int i = 0; i < playerFootSteps; i++)
            {
                if (playerTargetList.footStepTargets[i].transform == this.transform)
                {
                    targetLocation = playerObject.transform.position;
                    // Message with a GameObject name.
                    Debug.Log("I Hear the Player " + this.gameObject.name);

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
            targetLocation = playerObject.transform.position;
            Debug.Log("I see the Player " + this.gameObject.name);
            return ENodeState.Success;
        }
        return ENodeState.Failure;
    }

    ENodeState MoveToPlayer()
    {
       // if (!Def.isPointInsideSphere(transform.position, targetLocation, 10.0f))
      
            agent.SetDestination(targetLocation);        
        
        return ENodeState.Running;
    }

    ENodeState MoveToPatrolPt()
    {
        while (SeeThePlayer() != ENodeState.Success && HearThePlayer() != ENodeState.Success)
        {
            if (patrolPts.Count > 0)
            {
                if (currentPatrolPt >= patrolPts.Count)
                {
                    currentPatrolPt = 0;
                }

                agent.SetDestination(patrolPts[currentPatrolPt].transform.position);

                if (Def.isPointInsideSphere(transform.position, patrolPts[currentPatrolPt].transform.position, 2f))
                {
                    currentPatrolPt++;
                }
            }
            return ENodeState.Running;
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
        Movept = new CActionNode(MoveToPatrolPt, "MoveToPoint");

        List<CNode> AttackIfSeenPlayer = new List<CNode>() { Sight, MoveEnemy };
        List<CNode> AttackIfHeardPlayer = new List<CNode>() { Hearing, MoveEnemy };
        List<CNode> GoToPatrolPoint = new List<CNode>() { Movept};

        AttackSight = new CSequenceNode(AttackIfSeenPlayer, "sightSequence");
        AttackHeard = new CSequenceNode(AttackIfHeardPlayer, "hearSequence");
        patrol = new CSequenceNode(GoToPatrolPoint, "patrolSequence");

        List<CNode> Tree = new List<CNode>() { AttackHeard, AttackSight, patrol };

        Root = new CSelectorNode(Tree, "Root");


    }

    // Update is called once per frame
    void LateUpdate()
    {
        visibleTargets = enemyObject.GetComponent<FieldOfView>().visibleTargets;
        playerTargetList = enemyObject.GetComponent<FieldOfView>().playerTargetList;

        playerList = visibleTargets.Count;
        playerFootSteps = playerTargetList.footStepTargets.Count;

        Root.RunTree();

       // Hearing.ReturnNode();


    }
}
