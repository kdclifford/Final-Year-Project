using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position;
    }
}
