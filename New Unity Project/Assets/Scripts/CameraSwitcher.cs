using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject SwitchedCamera;
    [SerializeField]
    private GameObject player;
    private bool IsSwitched = false;
    private Vector3 oldCameraPos = new Vector3();

    private GameObject UiCanvas;
    private GameObject SideMenu;
    private GameObject MiniMap;
    private GameObject HealthBar;
    private GameObject GameManager;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SwitchedCamera = GameObject.FindGameObjectWithTag("PlayerCamera");
        oldCameraPos = SwitchedCamera.transform.localPosition;
        UiCanvas = GameObject.FindGameObjectWithTag("ScreenCanvas");
        SideMenu = UiCanvas.transform.Find("SideMenu").gameObject;
        MiniMap = UiCanvas.transform.Find("MiniMapHolder").gameObject;
        HealthBar = UiCanvas.transform.Find("HealthBarHolder").gameObject;
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        SwitchedCamera.GetComponent<PlayerCamera>().enabled = IsSwitched;
    //        player.GetComponent<PlayerMove>().enabled = IsSwitched;
    //        SwitchedCamera.GetComponent<CameraMove>().enabled = !IsSwitched;
    //        //GameManager.GetComponent<MouseClick>().enabled = !IsSwitched;

    //        // if cam free roam
    //        if (!IsSwitched)
    //        {
    //            Cursor.lockState = CursorLockMode.Confined;
    //            SwitchedCamera.transform.parent = null;
    //            //SideMenu.SetActive(true);
    //            MiniMap.SetActive(false);
    //            HealthBar.SetActive(false);
    //        }
    //        else // locked camera
    //        {
    //            //SideMenu.SetActive(false);
    //            MiniMap.SetActive(true);
    //            HealthBar.SetActive(true);
    //            Cursor.lockState = CursorLockMode.Locked;
    //            SwitchedCamera.transform.parent = player.transform;
    //            SwitchedCamera.transform.localPosition = oldCameraPos;
    //            SwitchedCamera.transform.rotation = SwitchedCamera.transform.parent.rotation;
    //        }

    //        IsSwitched = !IsSwitched;
    //    }
    //}
}
