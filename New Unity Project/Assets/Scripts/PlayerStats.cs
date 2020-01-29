using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float Health;
    public float Damage;

    public Image HealthBar;
    public float currentHealth;

    void Start()
    {
        currentHealth = Health;
    }

    void Update()
    {        
        HealthBar.fillAmount = currentHealth / Health;
    }
}
