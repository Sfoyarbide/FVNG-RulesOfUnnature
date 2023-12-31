using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    [Range(0f,1f)]
    public float spatialBlend;
    [Range(0f,256f)]
    public int priority;
    [HideInInspector]
    public AudioSource source;
    public bool loop;
    public bool ignoreListenerPause;
}
