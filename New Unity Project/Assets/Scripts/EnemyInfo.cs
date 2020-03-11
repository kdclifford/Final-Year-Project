using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public Text enemyNameText;
    public Slider enemyHealthSlider;
    public int maxHealth;
    public int currentHealth;
    public GameObject HealthPack;
    float staminaTimer = 0.0f;
    float seconds;
    public int staminaMuliplier  = 1;
    public int staminaDamage = 3;

    // Start is called before the first frame update
    void Start()
    {
        enemyNameText.text = transform.name;
        enemyHealthSlider = transform.Find("EnemyCanvas").Find("Slider").GetComponent<Slider>();
        //Health = 10;
    }

    void Update()
    {
        staminaTimer += Time.deltaTime;
        seconds = staminaTimer % 60;
        if ( seconds > 1)
            {
            staminaTimer = 0;
            currentHealth -= staminaDamage * staminaMuliplier;

        }


        float sliderValue = (float)currentHealth / maxHealth;
        enemyHealthSlider.value = sliderValue;
    }


    //void Start()
    //{
    //    enemyNameText.text = transform.name;
    //    Health = 10;
    //}


}
