using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    private GameObject UiCanvas;
    private GameObject SideMenu;
    private GameObject MiniMap;
    private GameObject HealthBar;
    private GameObject MainMenu;
    private GameObject ControllerMenu;
    private GameObject playerObject;
    private GameObject PlayerCamera;
    public float testdd;

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player").gameObject;
        PlayerCamera = playerObject.transform.Find("PlayerCamera").gameObject;
        UiCanvas = GameObject.FindGameObjectWithTag("ScreenCanvas");
        SideMenu = UiCanvas.transform.Find("SideMenu").gameObject;
        MiniMap = UiCanvas.transform.Find("MiniMapHolder").gameObject;
        HealthBar = UiCanvas.transform.Find("HealthBarHolder").gameObject;
        MainMenu = UiCanvas.transform.Find("MainMenuHolder").gameObject;
        ControllerMenu = UiCanvas.transform.Find("ControllerInfoHolder").gameObject;
    }


    public void StartGame()
    {
        playerObject.GetComponent<PlayerMove>().enabled = true;
        PlayerCamera.transform.localPosition = new Vector3(0, 2.0f, -2);
        PlayerCamera.GetComponent<PlayerCamera>().enabled = true;

        MiniMap.SetActive(true);
        HealthBar.SetActive(true);
        MainMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        SideMenu.GetComponent<RectTransform>().position = new Vector3(SideMenu.GetComponent<RectTransform>().position.x - 214.5f, SideMenu.GetComponent<RectTransform>().position.y, 0);
        testdd = SideMenu.GetComponent<RectTransform>().position.x;


    }


    public void ControllerInfo()
    {
        MainMenu.SetActive(false);
        ControllerMenu.SetActive(true);
    }
    public void BackButton()
    {
        MainMenu.SetActive(true);
        ControllerMenu.SetActive(false);
    }


}
