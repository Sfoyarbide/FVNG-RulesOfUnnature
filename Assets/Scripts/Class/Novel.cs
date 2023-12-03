using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Novel", menuName = "Novel")]
public class Novel : ScriptableObject
{
    [Header("Novel Settings")]
    public Dialogue[] dialogues;
    public Novel nextNovel;
    [Header("Flowchart Settings")]
    public string idLoad; // Name to load the novel.
    public int idFC; // Name to identify the novel in the FC
    public float timeStartNovel;
    [Header("Ending Settings")]
    public bool isEnding; // To check if it this a ending.
    public float TimeToCredits;
}