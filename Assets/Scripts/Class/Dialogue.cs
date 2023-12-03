using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string dialogueId;
    public string Name;
    [TextArea]
    public string line;
    public float speed; 
    public AudioClip voice;
    public string soundtrack;
    public bool pauseSoundtrack;
    public bool MakeTransition;
    public Sprite background;
    public bool EndNovel;
    public bool KickOtherCharacter;
    public Sprite[] character; // 0 = Cen, 1 = Izq, 2 = Der, 2 < n = Nothing
    public Effect[] effect;
    public Options options;
}