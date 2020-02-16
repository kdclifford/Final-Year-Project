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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SwitchedCamera = GameObject.FindGameObjectWithTag("PlayerCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchedCamera.GetComponent<PlayerCamera>().enabled = IsSwitched;
            player.GetComponent<PlayerMove>().enabled = IsSwitched;
            SwitchedCamera.GetComponent<CameraMove>().enabled = !IsSwitched;

            if (!IsSwitched)
            {
                Cursor.lockState = CursorLockMode.Confined;
                SwitchedCamera.transform.parent = null;

            }

            IsSwitched = !IsSwitched;
        }
    }
}
