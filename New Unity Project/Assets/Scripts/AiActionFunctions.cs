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
    public Vector3 randomPos = new Vector3();
    bool pathAccessible = false;
    private Quaternion targetRotation = new Quaternion();
    RaycastHit hit = new RaycastHit();
    Vector3 enemyPos = new Vector3();



    public GameObject[] HealthPackList;
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

        HealthPackList = GameObject.FindGameObjectsWithTag("StaminaPot");


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
                NavMeshPath path = new NavMeshPath();
                agentNavMesh.CalculatePath(targetLocation, path);
                if (path.status != NavMeshPathStatus.PathPartial && targetLocation.y < 2f)
                {
               

                    targetRotation = Quaternion.LookRotation(playerObject.transform.position - transform.position);

                enemyPos = transform.position;
                enemyPos.y = 1f;

                if (Physics.Raycast(enemyPos,-(enemyPos - playerObject.transform.position).normalized, out hit))
                {
                    if (hit.transform.gameObject.tag == playerObject.tag )
                    {
                        float str = Mathf.Min(5 * Time.deltaTime, agentNavMesh.speed);
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);

                    }
                }

                }
                else
                {
                    return ENodeState.Failure;
                }


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
            enemyStats.staminaMuliplier = 0.125f;
            agentNavMesh.speed = 7;
            targetLocation = playerObject.transform.position;
            return ENodeState.Success;
        }
        else
        {
            enemyStats.staminaMuliplier = 0.5f;
            agentNavMesh.speed = 3.5f;
        }
        return ENodeState.Failure;
    }

    public ENodeState MoveToPlayer()
    {
        HearThePlayer();
        SeeThePlayer();
        enemyStats.staminaMuliplier = 0.5f;
        NavMeshPath path = new NavMeshPath();
        agentNavMesh.CalculatePath(targetLocation, path);
        if (path.status != NavMeshPathStatus.PathPartial && targetLocation.y < 2f)
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
        if (randomPos == new Vector3(0, 0, 0))
        {
            while (!pathAccessible)
            {
                randomPos = new Vector3(-25f, 0f, 1.53f) + Random.insideUnitSphere * 60;
                randomPos.y = 0.3f;
                NavMeshPath path = new NavMeshPath();
                agentNavMesh.CalculatePath(randomPos, path);
                if (path.status != NavMeshPathStatus.PathPartial)
                {
                    pathAccessible = true;
                    break;
                }

            }

        }

       
            enemyStats.staminaMuliplier = 0.5f;
            aiAnimation.SetInteger("Animation", 1);
            agentNavMesh.speed = 3.5f;
            //if (patrolPts.Count > 0)
            //{
            //    if (currentPatrolPt >= patrolPts.Count)
            //    {
            //        currentPatrolPt = 0;
            //    }

            agentNavMesh.SetDestination(randomPos);

            if (Def.isPointInsideSphere(transform.position, randomPos, 3f))
            {
                // currentPatrolPt++;
                randomPos = new Vector3(0, 0, 0);
                pathAccessible = false;
                return ENodeState.Success;
            }



         
        return ENodeState.Running;
    }


    //Attack the Player if close
    public ENodeState AttackPlayer()
    {
        if (Def.isPointInsideSphere(transform.position, playerObject.transform.position, 3f))
        {
            enemyStats.staminaMuliplier = 0.5f;
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
                if (healthPack.GetComponent<HealthPackInfo>().IsBeingUsed == false)
                {
                    healthPack.GetComponent<HealthPackInfo>().IsBeingUsed = true;
                    enemyStats.HealthPack = healthPack;
                    break;
                }
            }
        }


        if (enemyStats.HealthPack != null)
        {
            enemyStats.staminaMuliplier = 1;
            agentNavMesh.SetDestination(enemyStats.HealthPack.transform.position);
            aiAnimation.SetInteger("Animation", 1);
            if (Def.isPointInsideSphere(transform.position, enemyStats.HealthPack.transform.position, 3f))
            {
                aiAnimation.SetInteger("Animation", 0);
                targetRotation = Quaternion.LookRotation(enemyStats.HealthPack.transform.position - transform.position);

                float str = Mathf.Min(5 * Time.deltaTime, agentNavMesh.speed);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);


                enemyStats.staminaMuliplier = 10;

                StaminaRegenTimer += Time.deltaTime;
                seconds = StaminaRegenTimer % 60;
                if (seconds > 0.05f)
                {
                    StaminaRegenTimer = 0;
                    enemyStats.currentHealth += 1;

                }


                if (enemyStats.currentHealth > enemyStats.maxHealth)
                {
                    enemyStats.HealthPack.GetComponent<HealthPackInfo>().IsBeingUsed = false;
                    enemyStats.HealthPack = null;
                    return ENodeState.Success;
                }
            }

            return ENodeState.Running;
        }



        return ENodeState.Failure;

    }

    public ENodeState Guard()
    {
        while (SeeThePlayer() != ENodeState.Success && HearThePlayer() != ENodeState.Success && IsHealthLow() != ENodeState.Success)
        {
            aiAnimation.SetInteger("Animation", 1);
            agentNavMesh.speed = 3.5f;
            agentNavMesh.SetDestination(spawnPosition);
            enemyStats.staminaMuliplier = 5;

            if (Def.isPointInsideSphere(transform.position, spawnPosition, 1f))
            {
                enemyStats.staminaMuliplier = 10;
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

            return ENodeState.Running;
        }
        return ENodeState.Failure;
    }
}
