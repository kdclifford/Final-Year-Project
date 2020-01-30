using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Animator PlayerModel;
    public float Health;
    public float Damage;

    public Image HealthBar;
    public float currentHealth;
    private float oldHealth;

    void Start()
    {
        currentHealth = Health;
        oldHealth = Health;
    }

    void Update()
    {        
        if(oldHealth > currentHealth)
        {
            PlayerModel.SetInteger("Animation", 7);
            oldHealth = currentHealth;
        }

        HealthBar.fillAmount = currentHealth / Health;
    }
}
