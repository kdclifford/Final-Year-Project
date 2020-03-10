using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsBehaviours : MonoBehaviour
{
    AiActionFunctions actions;
    public List<CNode> AllNodes = new List<CNode>();
    //Nodes
    CActionNode Hearing;
        CActionNode Sight;
        CActionNode Chase;
        CActionNode MoveToEnemyPos;
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
    void CreateTree()
    {



        // Action Nodes
        Hearing = new CActionNode(actions.HearThePlayer, "Hearing");
        Sight = new CActionNode(actions.SeeThePlayer, "Sight");
        Chase = new CActionNode(actions.MoveToPlayer, "ChaseEnemy");
        MoveToEnemyPos = new CActionNode(actions.MoveToPlayer, "MoveToEnemyPos");
        PatrolPT = new CActionNode(actions.MoveToPatrolPt, "PatrolPT");
        AttackThePlayer = new CActionNode(actions.AttackPlayer, "AttackThePlayer");
        HealthLow = new CActionNode(actions.IsHealthLow, "HealthLow");
        GetHealth = new CActionNode(actions.GetHealthPack, "GetHealth");

        // Inverter Nodes
        HealthInverter = new CInverterNode(HealthLow, "Health");

        // Timer Nodes
        AttackTimer = new CTimerNode(AttackThePlayer, "Attack", 1f);


        // Sequence Nodes
        AttackSight = new CSequenceNode(new List<CNode>() { Sight, Chase, AttackTimer }, "Sight ");
        AttackHeard = new CSequenceNode(new List<CNode>() { Hearing, MoveToEnemyPos }, "Hearing ");
        Health = new CSequenceNode(new List<CNode>() { HealthLow, GetHealth }, "Health ");
        Patrol = new CSequenceNode(new List<CNode>() { HealthInverter, PatrolPT }, "Patrol ");

        // Root Node
        Root = new CSelectorNode(new List<CNode>() { Health, AttackSight, AttackHeard, Patrol }, "Root");

        AllNodes.Add(Root);
        AllNodes.Add(AttackHeard);
        AllNodes.Add(AttackSight);
        AllNodes.Add(Patrol);
        AllNodes.Add(PatrolPT);
        AllNodes.Add(Chase);
        AllNodes.Add(MoveToEnemyPos);
        AllNodes.Add(Sight);
        AllNodes.Add(Hearing);
        AllNodes.Add(AttackThePlayer);
        AllNodes.Add(Health);
    }






    // Start is called before the first frame update
    void Start()
    {
        actions = GetComponent<AiActionFunctions>();
        CreateTree();
    }
}
