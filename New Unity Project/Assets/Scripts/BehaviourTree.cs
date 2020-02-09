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
    AiActionFunctions actions;
    
    public GameObject[,] Map;
    private GameObject gameManager;

    // private FieldOfView fieldOfViewAI;
   // [SerializeField] private FootSteps playerTargetList;
    [SerializeField] public GameObject TreeList;

    //Nodes
    CActionNode Hearing;
    CActionNode Sight;
    CActionNode MoveEnemy;
    CActionNode PatrolPT;
    CActionNode AttackThePlayer;
    CActionNode HealthLow;
    CActionNode GetHealth;

    CSequenceNode AttackSight;
    CSequenceNode AttackHeard;
    CSequenceNode Health;
    CSequenceNode Patrol;

    CInverterNode HealthInverter;
    CTimerNode AttackTimer;
    CSelectorNode Root;

    //List of all Nodes
    public List<CNode> AllNodes = new List<CNode>();

    void CreateTree()
    {
        // Action Nodes
        Hearing = new CActionNode(actions.HearThePlayer, "Hearing");
        Sight = new CActionNode(actions.SeeThePlayer, "Sight");
        MoveEnemy = new CActionNode(actions.MoveToPlayer, "MoveEnemy");     
        PatrolPT = new CActionNode(actions.MoveToPatrolPt, "PatrolPT");
        AttackThePlayer = new CActionNode(actions.AttackPlayer, "AttackThePlayer");
        HealthLow = new CActionNode(actions.IsHealthLow, "HealthLow");
        GetHealth = new CActionNode(actions.GetHealthPack, "GetHealth");

        // Inverter Nodes
        HealthInverter = new CInverterNode(HealthLow, "Health");

        // Timer Nodes
        AttackTimer = new CTimerNode(AttackThePlayer, "Attack" ,1f);


        // Sequence Nodes
        AttackSight = new CSequenceNode(new List<CNode>(){ Sight, MoveEnemy, AttackTimer }, "sight");
        AttackHeard = new CSequenceNode(new List<CNode>() { Hearing, MoveEnemy}, "hear");
        Health = new CSequenceNode(new List<CNode>() { HealthLow, GetHealth }, "health");
        Patrol = new CSequenceNode(new List<CNode>() { HealthInverter, PatrolPT }, "patrol");

        // Root Node
        Root = new CSelectorNode(new List<CNode>() { Health, AttackSight, AttackHeard, Patrol }, "Root");

        AllNodes.Add(Root);
        AllNodes.Add(AttackHeard);
        AllNodes.Add(AttackSight);
        AllNodes.Add(Patrol);
        AllNodes.Add(PatrolPT);
        AllNodes.Add(MoveEnemy);
        AllNodes.Add(Sight);
        AllNodes.Add(Hearing);
        AllNodes.Add(AttackThePlayer);
        AllNodes.Add(Health);
    }

   public CNode currentnode;
    public ENodeState currentState;
    public string currentName;
    bool showHud = false;

    void RunTree()
    { 
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

    // Start is called before the first frame update
    void Start()
    {
        actions = GetComponent<AiActionFunctions>();
        CreateTree();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RunTree();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            showHud = !showHud;
        }
    }
}
