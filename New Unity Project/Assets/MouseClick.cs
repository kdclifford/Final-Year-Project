﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    public List<string> ClickTagList;

    private Camera MainCam;


    private void Start()
    {
        MainCam = GameObject.FindWithTag("PlayerCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //PointerEventData pointerData = new PointerEventData(EventSystem.current);
            //pointerData.position = Input.mousePosition;

            //List<RaycastResult> results = new List<RaycastResult>();
            //EventSystem.current.RaycastAll(pointerData, results);

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(MainCam.ScreenPointToRay(Input.mousePosition), out hitInfo);

            //if (results.Count > 0)
            //{
            //    //WorldUI is my layer name
            //    if (results[0].gameObject.layer == LayerMask.GetMask("UI"))
            //    {
            //        string dbg = "Root Element: {0} \n GrandChild Element: {1}";
            //        Debug.Log(string.Format(dbg, results[results.Count - 1].gameObject.name, results[0].gameObject.name));
            //        results.Clear();
            //    }
            //}
            //else
            //{
            //Debug.Log("Hit " + hitInfo.transform.gameObject.name);
            if (hit)
                {

                    Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                    foreach (var s in ClickTagList)
                    {
                        if (hitInfo.transform.gameObject.tag == s)
                        {
                           // CurrentSelected = hitInfo.transform.gameObject;

                            Debug.Log("It's Working!");
                            break;
                        }

                    }

                }
          //  }


            Debug.Log("Mouse is down");
        }

     
    }

}
