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

    private void Start()
    {
        MainCam = GameObject.FindWithTag("PlayerCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(MainCam.ScreenPointToRay(Input.mousePosition), out hitInfo);




            if (results.Count > 0)
            {
                //WorldUI is my layer name
                if (results[0].gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    Debug.Log("Hit UI");
                    string dbg = "Root Element: {0} \n GrandChild Element: {1}";
                    //Debug.Log(string.Format(dbg, results[results.Count - 1].gameObject.name, results[0].gameObject.name));
                    results.Clear();
                }
            }
            else
            {
                if (hit)
                {
                //Debug.Log("Hit " + hitInfo.transform.gameObject.name);

                    Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                    foreach (var s in ClickTagList)
                    {
                        if (hitInfo.transform.gameObject.tag == s)
                        {

                           enemyCanvas = hitInfo.transform.Find("EnemyCanvas").GetComponent<Canvas>();

                            selectedIcon.transform.parent = enemyCanvas.transform;

                            selectedIcon.transform.localRotation = Quaternion.identity;
                            selectedIcon.transform.localPosition = IconPosistion;

                            MainCam.GetComponent<CameraMove>().enabled= false;

                            MainCam.transform.parent = hitInfo.transform.gameObject.transform;

                            MainCam.transform.localPosition = new Vector3(0, 4, -8);
                            MainCam.transform.rotation = MainCam.transform.parent.rotation;

                            menuCanvas = GameObject.FindGameObjectWithTag("SideMenu");
                            canvasInfo = hitInfo.transform.GetComponent<EnemyInfo>();
                            enemyName = menuCanvas.transform.Find("Name").GetComponent<Text>();
                            enemyName.text = canvasInfo.enemyNameText.text;


                            Debug.Log("It's Working!");
                            break;
                        }

                    }

                }
            }


            Debug.Log("Mouse is down");
        }

     
    }

}
