using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapMove : MonoBehaviour
{
    public Transform player;    

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = player.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
       
    }
}
