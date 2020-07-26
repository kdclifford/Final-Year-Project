using UnityEngine.Audio;
using UnityEngine;

//Class used to store all sound variables
[System.Serializable]
public class Sound
{
    public string name; //Clip Name
    public AudioClip clip; //Sound Clip
    public bool loop; //Loop Sound
    public bool Sound3D; 
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;   
}
