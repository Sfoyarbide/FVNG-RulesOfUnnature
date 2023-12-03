using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Options
{
    public bool HaveOptions;
    [TextArea]
    public string[] options;
    public int WhatToChange; // 0 = Novel, 1 = novel, 2 = Scene;
    [Header("In case of Novel (0)")]
    public Novel[] novel;
    [Header("In case of Index (1)")]
    public int[] index;
    [Header("In case of Scene (2)")]
    public string[] NameScene;
}