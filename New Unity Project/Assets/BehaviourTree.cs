using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//Parent Class of All the behaviour tree Nodes
public abstract class CNode
{
   protected CNode mParent;
    //protected CNode mCurrentNode;
    private string mNameOfNode;
    public ENodeState mCurrentNodeState;
    private List<CNode> childrenNodes;


    //public abstract ENodeState RunTree();
    public abstract CNode RunTree();


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

    public List<CNode> GetChildren()
    {
        return childrenNodes;
    }
    public void SetChildren(List<CNode> children)
    {
        childrenNodes = children;
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

    public override CNode RunTree()
    {
           
        if(currentAction() == ENodeState.Failure)
        {
            mCurrentNodeState = ENodeState.Failure;
            //Debug.Log("Failure " + GetName());
            return this;
        }
        else if (currentAction() == ENodeState.Success)
        {
            mCurrentNodeState = ENodeState.Success;
           // Debug.Log("Success " + GetName());
            return this;
        }
        else
        {
            mCurrentNodeState = ENodeState.Running;
           // Debug.Log("Running " + GetName());
            return this;
        }
    }
}

public class CSelectorNode : CNode
{
    public CNode mCurrentChildNode;
   //public List<CNode> childNodes = new List<CNode>();
    public CSelectorNode( List<CNode> PassedChildNodes, string name)
    {
        SetChildren(PassedChildNodes);
        SetName(name);

        foreach(CNode i in GetChildren())
        {
            i.SetParent(this);
        }
    }

    public override CNode RunTree()
    {
        foreach (CNode nodes in GetChildren())
        {
            mCurrentChildNode = nodes;
            ENodeState childnodestate = nodes.RunTree().mCurrentNodeState;
           
            if (childnodestate == ENodeState.Success)
            {
                mCurrentNodeState = ENodeState.Success;
                Debug.Log("Success " + nodes.GetName());

                return nodes;
            }
            else if (childnodestate == ENodeState.Running)
            {
                mCurrentNodeState = ENodeState.Running;
                Debug.Log("Running " + nodes.GetName());
                return nodes;
            }
        }
        mCurrentNodeState = ENodeState.Failure;
        return this;
    }

}

public class CSequenceNode : CNode
{
    public CNode mCurrentChildNode;
    //public List<CNode> childNodes = new List<CNode>();
    public CSequenceNode(List<CNode> PassedChildNodes, string name)
    {
        SetChildren(PassedChildNodes);
        SetName(name);
        foreach (CNode i in GetChildren())
        {
            i.SetParent(this);
        }
    }


    public override CNode RunTree()
    {
       // mCurrentNodeState = ENodeState.Running;
        foreach (CNode nodes in GetChildren())
        {
            mCurrentChildNode = nodes;
            ENodeState childnodestate = nodes.RunTree().mCurrentNodeState;      
            //SpawnNodeUI(Root);
            if (childnodestate == ENodeState.Failure)
            {
                mCurrentNodeState = ENodeState.Failure;
                Debug.Log("Failure " + nodes.GetName());

                return nodes;
            }
            else if (childnodestate == ENodeState.Running)
            {
                mCurrentNodeState = ENodeState.Running;
                //mParent = this;
                Debug.Log("Running " + nodes.GetName());
                return nodes;
            }


        }
        mCurrentNodeState = ENodeState.Success;
        return mCurrentChildNode;
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
            return ENodeState.Success;
        }
        return ENodeState.Failure;
    }

    ENodeState MoveToPlayer()
    {
       // if (!Def.isPointInsideSphere(transform.position, targetLocation, 10.0f))
      
            agent.SetDestination(targetLocation);

        if (Def.isPointInsideSphere(transform.position, playerObject.transform.position, 2f))
        {         
            return ENodeState.Success;
        }


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
                    //return ENodeState.Success;
                }
            }
            return ENodeState.Running;
        }
        return ENodeState.Failure;
    }


    void CreateTree()
    {
        //tree Stuff
        Hearing = new CActionNode(HearThePlayer, "Hearing");
        Sight = new CActionNode(SeeThePlayer, "Sight");
        MoveEnemy = new CActionNode(MoveToPlayer, "MoveToPlayer");
        Movept = new CActionNode(MoveToPatrolPt, "MoveToPoint");

        List<CNode> AttackIfSeenPlayer = new List<CNode>() { Sight, MoveEnemy };
        List<CNode> AttackIfHeardPlayer = new List<CNode>() { Hearing, MoveEnemy };
        List<CNode> GoToPatrolPoint = new List<CNode>() { Movept };

        AttackSight = new CSequenceNode(AttackIfSeenPlayer, "sightSequence");
        AttackHeard = new CSequenceNode(AttackIfHeardPlayer, "hearSequence");
        patrol = new CSequenceNode(GoToPatrolPoint, "patrolSequence");

        List<CNode> Tree = new List<CNode>() { AttackHeard, AttackSight, Movept };

        Root = new CSelectorNode(Tree, "Root");
    }

    CNode currentnode;


    void RunTree()
    {
        if (currentnode == null)
        {
            currentnode = Root.RunTree();
            //SpawnNodeUI(Root);
        }
        else if (currentnode.mCurrentNodeState == ENodeState.Running)
        {
            currentnode = currentnode.GetParent().RunTree();
        }
        else
        {
            currentnode = Root.RunTree();
        }


        SpawnNodeUI(currentnode);





    }

    public GameObject myPrefab;
    public GameObject NodeListUI;
    public GameObject NodeTitle;
    public GameObject NodeState;
    //public GameObject NodeListUI;
    //public text myPrefab;

    private Text nodetext;
    private Text state;

    void SpawnNodeUI(CNode spwanNodeUi)
    {
        //CNode parent = tree;

        //while (parent.GetChildren() != null)
        //{
        //    foreach(CNode nodes in parent.GetChildren())
        //    {





        //    }

        //   // NodeListUI[i] = (GameObject)Instantiate(myPrefab, new Vector3(100, 100, 0), Quaternion.Euler(0.0f, 0.0f, 0.0f));



        //}

        string nodeStateText = "";
        if (spwanNodeUi.mCurrentNodeState == ENodeState.Success)
        {
            nodeStateText = "Success";
        }
        else if (spwanNodeUi.mCurrentNodeState == ENodeState.Failure)
        {
            nodeStateText = "Failure";
        }
        else if (spwanNodeUi.mCurrentNodeState == ENodeState.Running)
        {
            nodeStateText = "Running";
        }




        NodeTitle = myPrefab.transform.GetChild(1).GetChild(1).gameObject;
        NodeState = myPrefab.transform.GetChild(1).GetChild(2).gameObject;
        nodetext = NodeTitle.GetComponent<Text>();
        nodetext.text = "Node: " + spwanNodeUi.GetName();

        nodetext = NodeState.GetComponent<Text>();
        nodetext.text = "State: " + nodeStateText;
        //NodeTitle.t.position = new Vector3(110, 110, 110);

        NodeListUI = (GameObject)Instantiate(myPrefab, new Vector3(100,0,100), Quaternion.Euler(0.0f, 0.0f, 0.0f));        
    }


    //void DisplayCurrentNode()
    //{
    //    Debug.Log("oioi " + currentnode.GetName());
    //}


    // Start is called before the first frame update
    void Start()
    {
        enemyObject = this.gameObject;
        //gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        //Map = gameManager.GetComponent<GameManager>().floorList;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        // fieldOfViewAI = gameManager.GetComponent<GameManager>()
        agent = GetComponent<NavMeshAgent>();

        CreateTree();

       


    }

    // Update is called once per frame
    void LateUpdate()
    {
        visibleTargets = enemyObject.GetComponent<FieldOfView>().visibleTargets;
        playerTargetList = enemyObject.GetComponent<FieldOfView>().playerTargetList;

        playerList = visibleTargets.Count;
        playerFootSteps = playerTargetList.footStepTargets.Count;

       
        RunTree();
        //DisplayCurrentNode();
        



       // Hearing.ReturnNode();


    }
}
