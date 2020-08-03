using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR 
using UnityEditor.Animations;
#endif 
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine;


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

    public TreeOptions behaviours;

    // private FieldOfView fieldOfViewAI;
   // [SerializeField] private FootSteps playerTargetList;
    [SerializeField] public GameObject TreeList;


    public List<CNode> AllNodes = new List<CNode>();
    CSelectorNode Root;


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
        behaviours = GetComponent<TreeOptions>();
        Root = behaviours.GetTree();
        AllNodes = behaviours.AllNodes;
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
