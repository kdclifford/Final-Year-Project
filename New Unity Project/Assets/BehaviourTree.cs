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
    int playerList;
    private List<Transform> visibleTargets = new List<Transform>();


    bool HearThePlayer()
    {
        if (playerFootSteps > 0)
        {
            for (int i = 0; i < playerFootSteps; i++)
            {
                if (playerTargetList.footStepTargets[i].transform == this.transform)
                {
                    // Message with a GameObject name.
                    Debug.Log("I Hear the Player " + this.gameObject.name);

                    //transform.LookAt(playerObject.transform);
                    //coneColour.material = alertColour;
                    return true;
                }
            }
        }
        return false;
    }

    bool SeeThePlayer()
    {
        if (playerList > 0)
        {
            Debug.Log("I see the Player " + this.gameObject.name);
            return true;
        }
        return false;
    }


    // Start is called before the first frame update
    void Start()
    {
        enemyObject = this.gameObject;
        //gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        //Map = gameManager.GetComponent<GameManager>().floorList;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        // fieldOfViewAI = gameManager.GetComponent<GameManager>()
    }

    // Update is called once per frame
    void LateUpdate()
    {
        visibleTargets = enemyObject.GetComponent<FieldOfView>().visibleTargets;
        playerTargetList = enemyObject.GetComponent<FieldOfView>().playerTargetList;

        playerList = visibleTargets.Count;
        playerFootSteps = playerTargetList.footStepTargets.Count;

        if (SeeThePlayer() || HearThePlayer())
        {

        }


    }
}
