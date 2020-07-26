using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{


  
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Step()
    {
        audioManager.Play("FootStep", gameObject);        
    }

    private void Bite()
    {

        audioManager.Play("Bite", gameObject);
    }

    private void Punch()
    {

        audioManager.Play("Punch", gameObject);
    }


    //private void Idle()
    //{
    //    AudioClip clip = GetidleClip();
    //    audioSource.PlayOneShot(clip);
    //}

    //private AudioClip GetidleClip()
    //{
    //    return IdleClip[UnityEngine.Random.Range(0, IdleClip.Length)];
    //}



}

