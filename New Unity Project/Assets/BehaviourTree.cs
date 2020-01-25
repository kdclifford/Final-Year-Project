using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class CUI
{
    public float xPos = -500f;
    public float yPos = 0f;
    public string colour = "";
    public string NodeName = "";
    public ENodeState NodeState;
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

    public GameObject myPrefab;

    CActionNode Hearing;
    CActionNode Sight;
    CActionNode MoveEnemy;
    CActionNode Movept;


    CSequenceNode AttackSight;
    CSequenceNode AttackHeard;
    CSequenceNode patrol;

    CSelectorNode Root;

    List<CNode> AllNodes = new List<CNode>();
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
                    return ENodeState.Success;
                }
            }
            return ENodeState.Running;
        }
        return ENodeState.Failure;
    }
     



    void CreateTree()
    {
        //tree Stuff
       // Hearing = new CActionNode(HearThePlayer, "Hearing", myPrefab);
        Sight = new CActionNode(SeeThePlayer, "Sight", myPrefab);
        MoveEnemy = new CActionNode(MoveToPlayer, "MoveToPlayer", myPrefab);
      //  Movept = new CActionNode(MoveToPatrolPt, "MoveToPoint", myPrefab);

        List<CNode> AttackIfSeenPlayer = new List<CNode>() { Sight, MoveEnemy };
       // List<CNode> AttackIfHeardPlayer = new List<CNode>() { Hearing, MoveEnemy };
        //List<CNode> GoToPatrolPoint = new List<CNode>() { Movept };

        AttackSight = new CSequenceNode(AttackIfSeenPlayer, "sightSequence", myPrefab);
        //AttackHeard = new CSequenceNode(AttackIfHeardPlayer, "hearSequence", myPrefab);
       // patrol = new CSequenceNode(GoToPatrolPoint, "patrolSequence", myPrefab);

        List<CNode> Tree = new List<CNode>() { AttackSight /*, AttackHeard, Movept */};

        Root = new CSelectorNode(Tree, "Root", myPrefab);

        AllNodes.Add(Root);
      //  AllNodes.Add(patrol);
      //  AllNodes.Add(AttackHeard);
        AllNodes.Add(AttackSight);

        foreach(CNode i in AllNodes)
        {
            foreach(CNode j in i.GetChildren())
            {
                j.nodeUI.xPos = i.nodeUI.xPos + 100;
                j.nodeUI.yPos = i.nodeUI.yPos + 100;
                GameObject temp;

                temp = j.mPrefab.transform.GetChild(0).gameObject;
                temp.transform.localPosition = new Vector2(j.nodeUI.xPos, j.nodeUI.yPos);  
            }
        }
    }

    CNode currentnode;


    void RunTree()
    {
        if (currentnode == null)
        {
            currentnode = Root.RunTree();
           //Def.SpawnNodeUI(currentnode, myPrefab);
        }
        //else if (currentnode.mCurrentNodeState == ENodeState.Running)
        //{
        //    currentnode = currentnode.GetParent().RunTree();
        //}
        else
        {
            currentnode = Root.RunTree();
        }
        currentnode.UpdatePrefab();

      // SpawnNodeUI(currentnode, myPrefab);





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
        
       // DisplayCurrentNode();
        



       // Hearing.ReturnNode();


    }
}
