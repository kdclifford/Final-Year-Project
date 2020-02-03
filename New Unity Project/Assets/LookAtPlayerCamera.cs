using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayerCamera : MonoBehaviour
{
    private Camera PlayerCamera;


    private void Start()
    {
        PlayerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>(); ;
    }


    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + PlayerCamera.transform.rotation * Vector3.back,
            PlayerCamera.transform.rotation * Vector3.up);
    }
}
