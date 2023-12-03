using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VNMData
{
    public string IdNovel;
    public string IdBackground;
    public int index;
    public int indexOptions;
    public int AuxIndex;
    public bool CantNext;
    public bool NotCanAdvance;
    public bool SelectionOptions;
    public bool SpecialOption;
    public float SpeedText;
    public string BeforeSoundtrack;
    public bool[] completedNovel;

    public VNMData(VisualNovelManager VNM)
    {
        //Name of novel
        IdNovel = VNM.novel.idLoad;
        IdBackground = VNM.Background.sprite.name;
        index = VNM.index;
        indexOptions = VNM.indexOptions;
        AuxIndex = VNM.AuxIndex;
        CantNext = VNM.CantNext;
        NotCanAdvance = VNM.NotCanAdvance;
        SelectionOptions = VNM.SelectionOptions;
        SpecialOption = VNM.SpecialOption;
        SpeedText = VNM.SpeedText;
        BeforeSoundtrack = VNM.BeforeSoundtrack;
        completedNovel = VNM.completedNovel;
    }
}
