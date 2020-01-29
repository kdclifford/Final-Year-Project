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
    private FieldOfView fieldOfViewAI;
    private FootSteps playerTargetList;
    NavMeshAgent agent;
    public GameObject TreeList;

    int playerFootSteps;
    int playerList;
    private List<Transform> visibleTargets = new List<Transform>();

    public Vector3 targetLocation;

    public List<GameObject> patrolPts = new List<GameObject>();
    public PlayerStats playerHealth; 
    public GameObject myPrefab;

    CActionNode Hearing;
    CActionNode Sight;
    CActionNode MoveEnemy;
    CActionNode MoveEnemy2;
    CActionNode Movept;
    CActionNode AttackThePlayer;
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

        HearThePlayer();
        SeeThePlayer();
        agent.SetDestination(targetLocation);

        if (Def.isPointInsideSphere(transform.position, targetLocation, 3f))
        {
            animation.SetInteger("Animation", 2);
            return ENodeState.Success;
        }
        return ENodeState.Running;

    }

    ENodeState MoveToPatrolPt()
    {
        while (SeeThePlayer() != ENodeState.Success && HearThePlayer() != ENodeState.Success)
        {
            animation.SetInteger("Animation", 1);
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
            playerHealth.currentHealth -= 10f;
            return ENodeState.Success;
        }
        return ENodeState.Failure;

    }






    void CreateTree()
    {
        //tree Stuff
        Hearing = new CActionNode(HearThePlayer, "Hearing", myPrefab, new Vector2(-100f, 0f));
        Sight = new CActionNode(SeeThePlayer, "Sight", myPrefab, new Vector2(-450f, 0f));
        MoveEnemy = new CActionNode(MoveToPlayer, "MoveToPlayer", myPrefab, new Vector2(-280f, 0f));
        MoveEnemy2 = new CActionNode(MoveToPlayer, "MoveToPlayer", myPrefab, new Vector2(100f, 0f));
        Movept = new CActionNode(MoveToPatrolPt, "MoveToPoint", myPrefab, new Vector2(350f, 100f));
        AttackThePlayer = new CActionNode(AttackPlayer, "AttackThePlayer", myPrefab, new Vector2(-350, -50));


        List<CNode> AttackIfSeenPlayer = new List<CNode>() { Sight, MoveEnemy, AttackThePlayer };
        List<CNode> AttackIfHeardPlayer = new List<CNode>() { Hearing, MoveEnemy2 };
        List<CNode> GoToPatrolPoint = new List<CNode>() { Movept };

        AttackSight = new CSequenceNode(AttackIfSeenPlayer, "sightSequence", myPrefab, new Vector2(-350f, 100f));
        AttackHeard = new CSequenceNode(AttackIfHeardPlayer, "hearSequence", myPrefab, new Vector2(0f, 100f));
        // patrol = new CSequenceNode(GoToPatrolPoint, "patrolSequence", myPrefab, new Vector2(350f, 100f));

        List<CNode> Tree = new List<CNode>() { AttackSight, AttackHeard, Movept };

        Root = new CSelectorNode(Tree, "Root", myPrefab, new Vector2(0f, 200f));

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

    CNode currentnode;
    bool showHud = false;

    void RunTree()
    {
        foreach (CNode i in AllNodes)
        {
            //i.ResetTreeStates();
            //i.UpdatePrefab();
            if (showHud)
            {
                i.mPrefab.SetActive(true);
            }
            else
            {
                i.mPrefab.SetActive(false);
            }
        }
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
                i.UpdatePrefab();
            }
            currentnode = Root.RunTree();
        }
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

        //TreeList.GetComponent<BehaviourTab>().stringVar1 = currentnode.GetName();


        // DisplayCurrentNode();


        if (Input.GetKeyDown(KeyCode.Space))
        {
            showHud = !showHud;
        }
        // Hearing.ReturnNode();


    }
}
