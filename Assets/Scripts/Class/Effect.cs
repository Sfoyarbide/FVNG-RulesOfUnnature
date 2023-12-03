using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TypeEffect
{
    None,
    Earthquake,
    Lightning,
    TriggerSound,
    StopSound,
    ChangeSpeed,
    ChangeIndex
}

[System.Serializable]
public class Effect
{
    public TypeEffect Type;
    public string audioclip;
    public float[] Floats;
}
