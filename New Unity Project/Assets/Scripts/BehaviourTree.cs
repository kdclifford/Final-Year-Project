using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class CUI
{
    public float xPos = 0f;
    public float yPos = 0f;
    public string colour = "";
    public string NodeName = "";
    public ENodeState NodeState;
}

public class BehaviourTree : MonoBehaviour
{
    [SerializeField] private Animator animation;
    
    public GameObject[,] Map;
    private GameObject gameManager;
    private GameObject playerObject;
    private GameObject enemyObject;
    // private FieldOfView fieldOfViewAI;
    [SerializeField] private FootSteps playerTargetList;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] public GameObject TreeList;

    int playerFootSteps;
    int playerList;
    [SerializeField] private List<Transform> visibleTargets = new List<Transform>();

    [SerializeField] private Vector3 targetLocation;

    public List<GameObject> patrolPts = new List<GameObject>();
    public PlayerStats playerHealth; 
    //public GameObject myPrefab;

    //Nodes
    CActionNode Hearing;
    CActionNode Sight;
    CActionNode MoveEnemy;
    CActionNode MoveEnemy2;
    CActionNode Movept;
    CActionNode AttackThePlayer;
    CSequenceNode AttackSight;
    CSequenceNode AttackHeard;
    CSequenceNode patrol;
    CTimerNode AttackTimer;
    CSelectorNode Root;

    //List of all Nodes
    public List<CNode> AllNodes = new List<CNode>();


    public int currentPatrolPt;

    ENodeState HearThePlayer()
    {
        
            for (int i = 0; i < playerFootSteps; i++)
            {
                if (playerTargetList.footStepTargets[i] == this.transform)
                {
                    agent.speed = 7;
                    targetLocation = playerObject.transform.position;
                    return ENodeState.Success;
                }
            }
        
        agent.speed = 3.5f;
        return ENodeState.Failure;
    }

    ENodeState SeeThePlayer()
    {
        if (playerList > 0)
        {
            agent.speed = 7;
            targetLocation = playerObject.transform.position;
            return ENodeState.Success;
        }
        else
        {
        agent.speed = 3.5f;
        }
        return ENodeState.Failure;
    }

    ENodeState MoveToPlayer()
    {
        // if (!Def.isPointInsideSphere(transform.position, targetLocation, 10.0f))

        HearThePlayer();
        SeeThePlayer();
        agent.SetDestination(targetLocation);
        animation.SetInteger("Animation", 2);

        if (Def.isPointInsideSphere(transform.position, targetLocation, 3f))
        {
            animation.SetInteger("Animation", 0);
            return ENodeState.Success;
        }
        return ENodeState.Running;

    }

    ENodeState MoveToPatrolPt()
    {
        while (SeeThePlayer() != ENodeState.Success && HearThePlayer() != ENodeState.Success)
        {
            animation.SetInteger("Animation", 1);
            agent.speed = 3.5f;
            if (patrolPts.Count > 0)
            {
                if (currentPatrolPt >= patrolPts.Count)
                {
                    currentPatrolPt = 0;
                }

                agent.SetDestination(patrolPts[currentPatrolPt].transform.position);

                if (Def.isPointInsideSphere(transform.position, patrolPts[currentPatrolPt].transform.position, 3f))
                {
                    currentPatrolPt++;
                    return ENodeState.Success;
                }



            }
            return ENodeState.Running;
        }
        return ENodeState.Failure;
    }


    ENodeState AttackPlayer()
    {
        if (Def.isPointInsideSphere(transform.position, playerObject.transform.position, 3f))
        {
            animation.SetInteger("Animation", 4);
            playerHealth.currentHealth -= 10f;
            return ENodeState.Success;
        }
        return ENodeState.Failure;
    }

    void CreateTree()
    {
        //tree Stuff
        Hearing = new CActionNode(HearThePlayer, "Hearing");
        Sight = new CActionNode(SeeThePlayer, "Sight");
        MoveEnemy = new CActionNode(MoveToPlayer, "MoveToPlayer");
        MoveEnemy2 = new CActionNode(MoveToPlayer, "MoveToPlayer");
        Movept = new CActionNode(MoveToPatrolPt, "MoveToPoint");
        AttackThePlayer = new CActionNode(AttackPlayer, "AttackThePlayer");
        AttackTimer = new CTimerNode(AttackThePlayer, "Attack Timer" ,1f);

        List<CNode> AttackIfSeenPlayer = new List<CNode>() { Sight, MoveEnemy, AttackTimer };
        List<CNode> AttackIfHeardPlayer = new List<CNode>() { Hearing, MoveEnemy2 };
        List<CNode> GoToPatrolPoint = new List<CNode>() { Movept };

        AttackSight = new CSequenceNode(AttackIfSeenPlayer, "sightSequence");
        AttackHeard = new CSequenceNode(AttackIfHeardPlayer, "hearSequence");
        // patrol = new CSequenceNode(GoToPatrolPoint, "patrolSequence", myPrefab, new Vector2(350f, 100f));

        List<CNode> Tree = new List<CNode>() { AttackSight, AttackHeard, Movept };

        Root = new CSelectorNode(Tree, "Root");

        AllNodes.Add(Root);
        // AllNodes.Add(patrol);
        AllNodes.Add(AttackHeard);
        AllNodes.Add(AttackSight);
        AllNodes.Add(Movept);
        AllNodes.Add(MoveEnemy);
        AllNodes.Add(MoveEnemy2);
        AllNodes.Add(Sight);
        AllNodes.Add(Hearing);
        AllNodes.Add(AttackThePlayer);
    }

   public CNode currentnode;
    public ENodeState currentState;
    public string currentName;
    bool showHud = false;

    void RunTree()
    {
        //foreach (CNode i in AllNodes)
        //{
        //    //i.ResetTreeStates();
        //    //i.UpdatePrefab();
        //    if (showHud)
        //    {
        //        i.mPrefab.SetActive(true);
        //    }
        //    else
        //    {
        //        i.mPrefab.SetActive(false);
        //    }
        //}
        if (currentnode == null)
        {
            currentnode = Root.RunTree();            
        }
       else if (currentnode.mCurrentNodeState == ENodeState.Running)
       {
           currentnode = currentnode.RunTree();
       }
        else if (currentnode.mCurrentNodeState == ENodeState.Failure || currentnode.mCurrentNodeState == ENodeState.Success)
        {
            foreach (CNode i in AllNodes)
            {
                i.ResetTreeStates();
               // i.UpdatePrefab();
            }
            currentnode = Root.RunTree();
        }
      //  currentnode = Root.RunTree();

        currentState = currentnode.mCurrentNodeState;
        currentName = currentnode.GetName();
    }

    //void DisplayCurrentNode()
    //{
    //    Debug.Log("oioi " + currentnode.GetName());
    //}


    // Start is called before the first frame update
    void Start()
    {
        enemyObject = this.gameObject;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerHealth = playerObject.GetComponent<PlayerStats>();
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            showHud = !showHud;
        }
    }
}
