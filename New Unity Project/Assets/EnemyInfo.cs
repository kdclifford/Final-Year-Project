using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public Text enemyNameText;
    public int maxHealth;
    public int currentHealth;
    public GameObject HealthPack;



    // Start is called before the first frame update
    void Start()
    {
        enemyNameText.text = transform.name;
        //Health = 10;
    }

    //void Start()
    //{
    //    enemyNameText.text = transform.name;
    //    Health = 10;
    //}


}
