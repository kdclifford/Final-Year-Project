using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    public List<string> ClickTagList;

    private Camera MainCam;
    public GameObject selectedIcon;
    public Canvas enemyCanvas;
    public Vector3 IconPosistion;
    public EnemyInfo canvasInfo;
    public GameObject menuCanvas;
    public Text enemyName;

    public GameObject SelectedAgent;
    private bool isTargetSelected = true;
    public CNode currentNode;
    public BehaviourTree currentTree;

    private GameObject[] allEnemyObjects;
    public GameObject enemy;

    public int currentEnemyAgent = 0;

    [SerializeField]
    private GameObject SwitchedCamera;
    [SerializeField]
    private GameObject player;
    private Vector3 oldCameraPos = new Vector3();

    private GameObject UiCanvas;
    private GameObject MiniMap;
    private GameObject HealthBar;

    bool oneSwitch = false;


    //Set Start Values
    private void Start()
    {
        MainCam = GameObject.FindWithTag("PlayerCamera").GetComponent<Camera>();
        menuCanvas = GameObject.FindGameObjectWithTag("SideMenu");
        allEnemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        player = GameObject.FindGameObjectWithTag("Player");
        SwitchedCamera = GameObject.FindGameObjectWithTag("PlayerCamera");
        oldCameraPos = SwitchedCamera.transform.localPosition;
        UiCanvas = GameObject.FindGameObjectWithTag("ScreenCanvas");
        MiniMap = UiCanvas.transform.Find("MiniMapHolder").gameObject;
        HealthBar = UiCanvas.transform.Find("HealthBarHolder").gameObject;
        SelectedAgent = allEnemyObjects[currentEnemyAgent];
       // Cursor.lockState = CursorLockMode.Locked;
        SwitchedCamera.transform.parent = player.transform;
        SwitchedCamera.transform.localPosition = oldCameraPos;
        SwitchedCamera.transform.rotation = SwitchedCamera.transform.parent.rotation;
    }

    //Update function will check to see if mouse buttton has been clicked
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    PointerEventData pointerData = new PointerEventData(EventSystem.current);
        //    pointerData.position = Input.mousePosition;

        //    List<RaycastResult> results = new List<RaycastResult>();
        //    EventSystem.current.RaycastAll(pointerData, results);

        //    RaycastHit hitInfo = new RaycastHit();
        //    bool hit = Physics.Raycast(MainCam.ScreenPointToRay(Input.mousePosition), out hitInfo);

        //    if (results.Count > 0)
        //    {
        //        //WorldUI is my layer name
        //        if (results[0].gameObject.layer == LayerMask.NameToLayer("UI"))
        //        {
        //            Debug.Log("Hit UI");
        //            string dbg = "Root Element: {0} \n GrandChild Element: {1}";
        //            //Debug.Log(string.Format(dbg, results[results.Count - 1].gameObject.name, results[0].gameObject.name));
        //            results.Clear();
        //        }
        //    }
        //    else
        //    {
        //        if (hit)            //If Ray hits an object
        //        {


        //            Debug.Log("Hit " + hitInfo.transform.gameObject.name);
        //            foreach (var s in ClickTagList)     //Look for matching tag        
        //            {
        //                if (hitInfo.transform.gameObject.tag == s)
        //                {
        //                    SelectedAgent = hitInfo.transform.gameObject;
        //                    enemyCanvas = SelectedAgent.transform.Find("EnemyCanvas").GetComponent<Canvas>();

        //                    selectedIcon.transform.parent = enemyCanvas.transform;

        //                    selectedIcon.transform.localRotation = Quaternion.identity;
        //                    selectedIcon.transform.localPosition = IconPosistion;

        //                    MainCam.GetComponent<CameraMove>().enabled = false;

        //                    MainCam.transform.parent = SelectedAgent.transform.gameObject.transform;

        //                    MainCam.transform.localPosition = new Vector3(0, 4, -8);
        //                    MainCam.transform.rotation = MainCam.transform.parent.rotation;

        //                    // menuCanvas = GameObject.FindGameObjectWithTag("SideMenu");
        //                    canvasInfo = SelectedAgent.transform.GetComponent<EnemyInfo>();
        //                    menuCanvas.transform.Find("Name").GetComponent<Text>().text = canvasInfo.enemyNameText.text;
        //                    //menuCanvas.transform.Find("CurrentNode").GetComponent<Text>().text = "Parent Node: " + SelectedAgent.transform.GetComponent<BehaviourTree>().currentnode.GetParent().GetName();

        //                    currentTree = SelectedAgent.transform.GetComponent<BehaviourTree>();

        //                    isTargetSelected = true;

        //                    Debug.Log("It's Working!");
        //                    break;
        //                }

        //            }

        //        }
        //    }


        //    Debug.Log("Mouse is down");
        //}


        if (SelectedAgent != null)
        {
            currentNode = SelectedAgent.GetComponent<BehaviourTree>().currentnode;

            if (currentNode != null)
            {
                if (currentNode.GetParent() != null)
                {
                    menuCanvas.transform.Find("NodeHolder").transform.Find("ParentNode").GetComponent<Text>().text = currentNode.GetParent().GetName();
                }
                else
                {
                    menuCanvas.transform.Find("NodeHolder").transform.Find("ParentNode").GetComponent<Text>().text = currentNode.GetName();
                }


                Color NodeText = Color.white;
                switch (currentNode.mCurrentNodeState)
                {
                    case ENodeState.Success:
                        NodeText = Color.green;
                        break;
                    case ENodeState.Failure:
                        NodeText = Color.red;
                        break;
                    case ENodeState.Running:
                        NodeText = Color.yellow;
                        break;
                }
                menuCanvas.transform.Find("NodeHolder").transform.Find("ParentNode").GetComponent<Text>().color = NodeText;
            }
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentEnemyAgent++;

            if(currentEnemyAgent > allEnemyObjects.Length - 1)
            {
                currentEnemyAgent = 0;
            }            
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentEnemyAgent--;

            if (currentEnemyAgent < 0)
            {
                currentEnemyAgent = allEnemyObjects.Length - 1;
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isTargetSelected = !isTargetSelected;
            oneSwitch = true;
        }

        if (!isTargetSelected )
        {
           // MainCam.GetComponent<CameraMove>().enabled = true;

            MainCam.transform.parent = SelectedAgent.transform.gameObject.transform;

            MainCam.transform.localPosition = new Vector3(0, 1.5f, -2);
            MainCam.transform.rotation = MainCam.transform.parent.rotation;
     
        }
        else if (isTargetSelected && oneSwitch)
        {
           // MainCam.GetComponent<CameraMove>().enabled = false;
            //MiniMap.SetActive(true);
            //HealthBar.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            SwitchedCamera.transform.parent = player.transform;
            SwitchedCamera.transform.localPosition = new Vector3(0, 2.0f, -2);
            //SwitchedCamera.transform.position = oldCameraPos;
            SwitchedCamera.transform.rotation = SwitchedCamera.transform.parent.rotation;
            oneSwitch = false; 
   
        }




        SelectedAgent = allEnemyObjects[currentEnemyAgent];
        enemyCanvas = SelectedAgent.transform.Find("EnemyCanvas").GetComponent<Canvas>();

        selectedIcon.transform.parent = enemyCanvas.transform;

        selectedIcon.transform.localRotation = Quaternion.identity;
        selectedIcon.transform.localPosition = IconPosistion;



        // menuCanvas = GameObject.FindGameObjectWithTag("SideMenu");
        canvasInfo = SelectedAgent.transform.GetComponent<EnemyInfo>();
        menuCanvas.transform.Find("Name").GetComponent<Text>().text = canvasInfo.enemyNameText.text;
        //menuCanvas.transform.Find("CurrentNode").GetComponent<Text>().text = "Parent Node: " + SelectedAgent.transform.GetComponent<BehaviourTree>().currentnode.GetParent().GetName();

        currentTree = SelectedAgent.transform.GetComponent<BehaviourTree>();

        

        Debug.Log("It's Working!");



    }

}
