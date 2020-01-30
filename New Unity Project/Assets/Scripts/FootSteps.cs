using UnityEngine;
using System.Collections;
using UnityEditor.Animations;
using System.Collections.Generic;

public class FootSteps : MonoBehaviour
{

    [SerializeField] private Animator animation;
    Vector3 oldPos;
    Vector3 newPos;

    public float soundRadius;
    public float stillRadius;
    private float soundAngle = 360;
    public float soundSpeed;
    [Range(0, 1)]
    private float t;
    [Range(0, 1)]
    private float q;
    [Range(0, 1)]
    private float p;
    [Range(0, 1)]
    private float w;
    public LayerMask targetMask;

    // [HideInInspector]
    public List<Transform> footStepTargets = new List<Transform>();

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;
    public float soundHeight;

    // **** Movement ****
    public float normalRadius;
    public float normalSpeed;
    public float normalJumpHeight;
    public float sprintSpeed;
    public float crouchSpeed;
    public float crouchJumpHeight;
    public float crouchRadius;
    public float sprintRadius;
    public float currentRadius; // used for the lerp
    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    PlayerMove playerMovement;

    public KeyCode crouch;
    public KeyCode sprint;
    public KeyCode forward;
    public KeyCode back;
    public KeyCode left;
    public KeyCode right;

    //**** Change the Vision Cone Colour
    private GameObject enemyObject;
    private GameObject visionCone;
    private Renderer coneColour;
    public Material alertColour;
    public Material normalColour;


    private bool isJumping = false;


    // **** Sets Varibles When The Project Starts ****
    void Start()
    {
        currentRadius = normalRadius;
        enemyObject = this.gameObject;
        playerMovement = enemyObject.GetComponent<PlayerMove>();
        visionCone = enemyObject.transform.Find("FootSteps").gameObject;
        coneColour = visionCone.GetComponent<Renderer>();
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            CanEnemyHearPlayer();
        }
    }

    void LateUpdate()
    {
        DrawSoundCirle();
        ChangeColourOfSoundCirle();


        if(Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }

        //animation.SetInteger("Animation", 7);

        if (Input.GetKey(forward) | Input.GetKey(back) | Input.GetKey(left) | Input.GetKey(right))
        {
            oldPos = transform.position;

            if (oldPos != newPos)
            {     

                if (Input.GetKey(crouch))
                {
                    animation.SetInteger("Animation", 5);
                    q = 0.0f;
                    p = 0.0f;
                    t = 0.0f;
                    currentRadius = crouchRadius;
                    w += soundSpeed;
                    w = Mathf.Clamp(w, 0.0f, 1.0f);
                    playerMovement.movementSpeed = crouchSpeed;
                    playerMovement.jumpMultiplier = crouchJumpHeight;
                    //currentRadius = currentRadius * (1 - w) + crouchRadius * w;
                    soundRadius = soundRadius * (1 - w) + crouchRadius * w;
                }
                else if (Input.GetKey(sprint) && !Input.GetKey(back))
                {                    
                    animation.SetInteger("Animation", 2);                    
                    q = 0.0f;
                    p = 0.0f;
                    w = 0.0f;
                    currentRadius = sprintRadius;
                    t += soundSpeed;
                    t = Mathf.Clamp(t, 0.0f, 1.0f);
                    //  soundRadius = currentRadius * (1 - t) + sprintRadius * t;
                    playerMovement.movementSpeed = sprintSpeed;
                    // currentRadius = currentRadius * (1 - t) + sprintRadius * t;
                    soundRadius = soundRadius * (1 - t) + sprintRadius * t;
                }
                else
                {                   
                    animation.SetInteger("Animation", 1);
                    t = 0.0f;
                    w = 0.0f;
                    p = 0.0f;
                    playerMovement.movementSpeed = normalSpeed;
                    playerMovement.jumpMultiplier = normalJumpHeight;
                    q += soundSpeed;
                    q = Mathf.Clamp(q, 0.0f, 1.0f);
                    soundRadius = soundRadius * (1 - q) + normalRadius * q;
                    
                }

                if(Input.GetKey(back))
                {
                    animation.SetInteger("Animation", 6);
                }               

            }
            else
            {
                t = 0.0f;
                w = 0.0f;
                q = 0.0f;
                p += soundSpeed;
                p = Mathf.Clamp(p, 0.0f, 1.0f);
                soundRadius = soundRadius * (1 - p) + stillRadius * p;
                playerMovement.movementSpeed = normalSpeed;
                playerMovement.jumpMultiplier = normalJumpHeight;
            }        
        }
        else
        {
            if (Input.GetKey(crouch))
            {
                animation.SetInteger("Animation", 4);
            }
            else
            {
                animation.SetInteger("Animation", 0);
            }
            t = 0.0f;
            w = 0.0f;
            q = 0.0f;
            p += soundSpeed;
            p = Mathf.Clamp(p, 0.0f, 1.0f);
            soundRadius = soundRadius * (1 - p) + stillRadius * p;
            playerMovement.movementSpeed = normalSpeed;
            playerMovement.jumpMultiplier = normalJumpHeight;
        }

        if (isJumping)
        {
            animation.SetInteger("Animation", 3);
            isJumping = false;
        }

        newPos = transform.position;

    }

    void CanEnemyHearPlayer()
    {
        footStepTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, soundRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < soundAngle / 2)
            {               
                    footStepTargets.Add(target);                
            }
        }
    }

    void ChangeColourOfSoundCirle()
    {
        int sizeOfList = footStepTargets.Count;
        if (sizeOfList > 0)
        {
            coneColour.material = alertColour;

        }
        else
        {
            coneColour.material = normalColour;
        }
    }


    void DrawSoundCirle()
    {
        int stepCount = Mathf.RoundToInt(soundAngle * meshResolution);
        float stepAngleSize = soundAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - soundAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }
            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        vertices[0].y = soundHeight;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            vertices[i + 1].y = soundHeight;

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }


    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }


    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

            return new ViewCastInfo(false, transform.position + dir * soundRadius, soundRadius, globalAngle);        
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

}






