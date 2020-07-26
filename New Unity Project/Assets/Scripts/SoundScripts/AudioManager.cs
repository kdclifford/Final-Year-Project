using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Stores all the sound clips
    public Sound[] soundClips;

    //Checks the current scene used to change the music depending on the scene
    private string currentScene;

    //Makes sure there is only one instace of the audio manager
    public static AudioManager instance;
    void Awake()
    {
        currentScene = SceneManager.GetActiveScene().name;
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        BackGroundMusic();

    }

    ////Used to check and change the audio depending on the scene
    //private void Update()
    //{
    //    if(currentScene != SceneManager.GetActiveScene().name)
    //    {
    //        BackGroundMusic();
    //        currentScene = SceneManager.GetActiveScene().name;
    //    }
        
    //}

    //Called when the scene changes
    void BackGroundMusic()
    {
       
            Play("Background", gameObject);
        
    }
       
    //Plays Sound if sound exists
    public void Play(string name, GameObject agent)
    {
        AudioSource agentAudio;
        if (agent.GetComponent<AudioSource>() == null)
        {
            agentAudio = agent.AddComponent<AudioSource>();
        }
        else
        {
            agentAudio = agent.GetComponent<AudioSource>();
        }

       Sound s = Array.Find(soundClips, Sound => Sound.name == name);       


        agentAudio.clip = s.clip;
        agentAudio.loop = s.loop;
        agentAudio.volume = s.volume;
        agentAudio.pitch = s.pitch;
        if(s.Sound3D)
        {
            agentAudio.spatialBlend = 1;
        }
        else
        {
            agentAudio.spatialBlend = 0;
        }

        agentAudio.Play();
    }
   
}
