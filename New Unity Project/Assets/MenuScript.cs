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
    private GameObject playerObject;
    private GameObject PlayerCamera;

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player").gameObject;
        PlayerCamera = playerObject.transform.Find("PlayerCamera").gameObject;
        UiCanvas = GameObject.FindGameObjectWithTag("ScreenCanvas");
        SideMenu = UiCanvas.transform.Find("SideMenu").gameObject;
        MiniMap = UiCanvas.transform.Find("MiniMapHolder").gameObject;
        HealthBar = UiCanvas.transform.Find("HealthBarHolder").gameObject;
        MainMenu = UiCanvas.transform.Find("MainMenuHolder").gameObject;

    }


    public void StartGame()
    {
        playerObject.GetComponent<PlayerMove>().enabled = true;
        PlayerCamera.transform.localPosition = new Vector3(0, 2.0f, -2);
        PlayerCamera.GetComponent<PlayerCamera>().enabled = true;

        SideMenu.SetActive(true);
        MiniMap.SetActive(true);
        HealthBar.SetActive(true);
        MainMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }


}
