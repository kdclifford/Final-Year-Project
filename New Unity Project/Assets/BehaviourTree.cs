using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BehaviourTree : MonoBehaviour
{
    
    public GameObject[,] Map;
    private GameObject gameManager;
    private GameObject playerObject;
    private GameObject enemyObject;
    private FieldOfView fieldOfViewAI;
    private FootSteps playerTargetList;


    int playerFootSteps;
    int sizeOfList;
    private List<Transform> visibleTargets = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        enemyObject = this.gameObject;
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        Map = gameManager.GetComponent<GameManager>().floorList;
        playerObject = GameObject.FindGameObjectWithTag("Player");
       // fieldOfViewAI = gameManager.GetComponent<GameManager>()
    }

    // Update is called once per frame
    void LateUpdate()
    {
        visibleTargets = enemyObject.GetComponent<FieldOfView>().visibleTargets;
        playerTargetList = enemyObject.GetComponent<FieldOfView>().playerTargetList;

        sizeOfList = visibleTargets.Count;
        playerFootSteps = playerTargetList.footStepTargets.Count;

        if (sizeOfList > 0)
            {
                //oldRotation = transform.rotation;
                transform.LookAt(visibleTargets[0].transform);
            }
            else if (playerFootSteps > 0)
            {
                for (int i = 0; i < playerFootSteps; i++)
                {
                    if (playerTargetList.footStepTargets[i].transform == this.transform)
                    {
                        transform.LookAt(playerObject.transform);
                        //coneColour.material = alertColour;


                    }
                    else
                    {
                        //coneColour.material = normalColour;
                    }
                }
            }
        
    }
}
