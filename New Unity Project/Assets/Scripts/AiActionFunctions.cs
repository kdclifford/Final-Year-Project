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
    private EnemyInfo enemyStats;

    public List<GameObject> HealthPackList = new List<GameObject>();
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    [SerializeField] private Vector3 targetLocation;

    public List<GameObject> patrolPts = new List<GameObject>();
    public int currentPatrolPt;
    private PlayerStats playerHealth;
    private float StaminaRegenTimer = 0;
    float seconds;
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        agentNavMesh = GetComponent<NavMeshAgent>();
        aiAnimation = GetComponent<Animator>();
        hearTargetList = playerObject.GetComponent<FootSteps>();
        visibleTargets = GetComponent<FieldOfView>().visibleTargets;
        playerHealth = playerObject.GetComponent<PlayerStats>();
        enemyStats = GetComponent<EnemyInfo>();
        spawnPosition = gameObject.transform.position;
        spawnRotation = gameObject.transform.rotation.eulerAngles;
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
            enemyStats.staminaMuliplier = 4;
            agentNavMesh.speed = 7;
            targetLocation = playerObject.transform.position;
            return ENodeState.Success;
        }
        else
        {
            enemyStats.staminaMuliplier = 2;
            agentNavMesh.speed = 3.5f;
        }
        return ENodeState.Failure;
    }

    public ENodeState MoveToPlayer()
    {
        HearThePlayer();
        SeeThePlayer();
        enemyStats.staminaMuliplier = 2;
        NavMeshPath path = new NavMeshPath();
        agentNavMesh.CalculatePath(targetLocation, path);
        if (path.status != NavMeshPathStatus.PathPartial)
        {
        agentNavMesh.SetDestination(targetLocation);
        aiAnimation.SetInteger("Animation", 2);

            if (Def.isPointInsideSphere(transform.position, targetLocation, 3f))
            {
                aiAnimation.SetInteger("Animation", 0);
                return ENodeState.Success;
            }
        }
        else
        {
            return ENodeState.Failure;
        }
        return ENodeState.Running;

    }

    public ENodeState MoveToPatrolPt()
    {
        while (SeeThePlayer() != ENodeState.Success && HearThePlayer() != ENodeState.Success && IsHealthLow() != ENodeState.Success)
        {
            enemyStats.staminaMuliplier = 2;
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


    public ENodeState AttackPlayer()
    {
        if (Def.isPointInsideSphere(transform.position, playerObject.transform.position, 3f))
        {
            enemyStats.staminaMuliplier = 2;
            aiAnimation.SetInteger("Animation", 4);
            playerHealth.currentHealth -= 10f;
            return ENodeState.Success;
        }
        return ENodeState.Failure;
    }

    public ENodeState IsHealthLow()
    {
        if (enemyStats.currentHealth <= (enemyStats.maxHealth / enemyStats.maxHealth) * 10)
        {
            return ENodeState.Success;
        }

        return ENodeState.Failure;
    }

    public ENodeState GetHealthPack()
    {
        if (enemyStats.HealthPack == null)
        {
            foreach (GameObject healthPack in HealthPackList)
            {
                healthPack.GetComponent<HealthPackInfo>().IsBeingUsed = true;
                enemyStats.HealthPack = healthPack;
            }
        }


        if (enemyStats.HealthPack != null)
        {
            enemyStats.staminaMuliplier = 1;
            agentNavMesh.SetDestination(enemyStats.HealthPack.transform.position);
            if (Def.isPointInsideSphere(transform.position, enemyStats.HealthPack.transform.position, 3f))
            {
                enemyStats.staminaMuliplier = 0;

                StaminaRegenTimer += Time.deltaTime;
                seconds = StaminaRegenTimer % 60;
                if (seconds > 1)
                {
                    StaminaRegenTimer = 0;
                    enemyStats.currentHealth += (enemyStats.maxHealth / enemyStats.maxHealth) * 10;

                }


                if (enemyStats.currentHealth > enemyStats.maxHealth)
                {
                    return ENodeState.Success;
                }
            }

            return ENodeState.Running;
        }



        return ENodeState.Failure;

    }

    public ENodeState Guard()
    {
        aiAnimation.SetInteger("Animation", 1);
        agentNavMesh.speed = 3.5f;
        agentNavMesh.SetDestination(spawnPosition);
        enemyStats.staminaMuliplier = 1;

        if (Def.isPointInsideSphere(transform.position, spawnPosition, 1f))
        {
            enemyStats.staminaMuliplier = 0;
            if (transform.rotation.eulerAngles.y < spawnRotation.y - 1)
            {
                transform.Rotate(Vector3.up * (50 * Time.deltaTime));
            }

            else if (transform.rotation.eulerAngles.y > spawnRotation.y + 1)
            {
                transform.Rotate(Vector3.down * (50 * Time.deltaTime));
            }
            else
            {
            aiAnimation.SetInteger("Animation", 0);
                transform.eulerAngles = spawnRotation;
                return ENodeState.Success;
            }
           // transform.Rotate(Vector3.up * (50 * Time.deltaTime));
        }

        return ENodeState.Failure;
    }
}
