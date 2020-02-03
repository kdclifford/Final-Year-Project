using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public Text enemyNameText;

    // Start is called before the first frame update
    void Start()
    {
        enemyNameText.text = transform.name;
    }   
}
