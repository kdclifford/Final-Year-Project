using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine;

public class AiActionFunctions : MonoBehaviour
{
    private GameObject playerObject;
    private NavMeshAgent agentNavMesh;
    private Animator aiAnimation;
    private FootSteps hearTargetList;
    private List<Transform> visibleTargets = new List<Transform>();

    [SerializeField] private Vector3 targetLocation;

    public List<GameObject> patrolPts = new List<GameObject>();
    public int currentPatrolPt;
    private PlayerStats playerHealth;


    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        agentNavMesh = GetComponent<NavMeshAgent>();
        aiAnimation = GetComponent<Animator>();
        hearTargetList = playerObject.GetComponent<FootSteps>();
        visibleTargets = GetComponent<FieldOfView>().visibleTargets;
        playerHealth = playerObject.GetComponent<PlayerStats>();
    }


    void Update()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        agentNavMesh = GetComponent<NavMeshAgent>();
        aiAnimation = GetComponent<Animator>();
        hearTargetList = playerObject.GetComponent<FootSteps>();
        visibleTargets = GetComponent<FieldOfView>().visibleTargets;
        playerHealth = playerObject.GetComponent<PlayerStats>();
    }

    public ENodeState HearThePlayer()
    {

        for (int i = 0; i < hearTargetList.footStepTargets.Count; i++)
        {
            if (hearTargetList.footStepTargets[i] == this.transform)
            {
                agentNavMesh.speed = 7;
                targetLocation = playerObject.transform.position;
                return ENodeState.Success;
            }
        }

        agentNavMesh.speed = 3.5f;
        return ENodeState.Failure;
    }

    public ENodeState SeeThePlayer()
    {
        if (visibleTargets.Count > 0)
        {
            agentNavMesh.speed = 7;
            targetLocation = playerObject.transform.position;
            return ENodeState.Success;
        }
        else
        {
            agentNavMesh.speed = 3.5f;
        }
        return ENodeState.Failure;
    }

    public ENodeState MoveToPlayer()
    {
        HearThePlayer();
        SeeThePlayer();
        agentNavMesh.SetDestination(targetLocation);
        aiAnimation.SetInteger("Animation", 2);

        if (Def.isPointInsideSphere(transform.position, targetLocation, 3f))
        {
            aiAnimation.SetInteger("Animation", 0);
            return ENodeState.Success;
        }
        return ENodeState.Running;

    }

   public ENodeState MoveToPatrolPt()
    {
        while (SeeThePlayer() != ENodeState.Success && HearThePlayer() != ENodeState.Success)
        {
            aiAnimation.SetInteger("Animation", 1);
            agentNavMesh.speed = 3.5f;
            if (patrolPts.Count > 0)
            {
                if (currentPatrolPt >= patrolPts.Count)
                {
                    currentPatrolPt = 0;
                }

                agentNavMesh.SetDestination(patrolPts[currentPatrolPt].transform.position);

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


   public  ENodeState AttackPlayer()
    {
        if (Def.isPointInsideSphere(transform.position, playerObject.transform.position, 3f))
        {
            aiAnimation.SetInteger("Animation", 4);
            playerHealth.currentHealth -= 10f;
            return ENodeState.Success;
        }
        return ENodeState.Failure;
    }
           
}
