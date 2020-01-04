using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BehaviourTree : MonoBehaviour
{
    
    public GameObject[,] Map;
    private GameObject gameManager;
    private GameObject player;
    public float hi;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        Map = gameManager.GetComponent<GameManager>().floorList;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
