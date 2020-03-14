using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{


    [SerializeField]
    public AudioClip stepClip;
    public AudioClip AttackClip;
    public AudioClip[] IdleClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Step()
    {

        audioSource.PlayOneShot(stepClip);
    }

    private void Bite()
    {

        audioSource.PlayOneShot(AttackClip);
    }

    private void Punch()
    {

        audioSource.PlayOneShot(AttackClip);
    }


    private void Idle()
    {
        AudioClip clip = GetidleClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetidleClip()
    {
        return IdleClip[UnityEngine.Random.Range(0, IdleClip.Length)];
    }



}

