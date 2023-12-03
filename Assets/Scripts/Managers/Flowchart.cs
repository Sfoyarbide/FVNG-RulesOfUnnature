using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Flowchart : MonoBehaviour
{
    public VisualNovelManager VNM;
    AudioManager AM;
    public GameObject interfaceGO;
    public GameObject[] Novels;
    public TextMeshProUGUI[] texts;
    public GameObject[] GOs;
    public GameObject EndMenu;
    bool HaveGo;
    public bool notUpdate;
    public bool IsEnding;
    Novel novel;
    public NovelFC defaultN;

    // Start is called before the first frame update
    void Start()
    {
        VNM = FindObjectOfType<VisualNovelManager>();
        AM = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Openflowchart();
    }

    public void Openflowchart(bool FromCredits=false)
    {
        if(Input.GetKeyDown(KeyCode.C) || FromCredits)
        {
            if(!VNM.OneMenuIsOn && !VNM.NotCanAdvance)
            {
                interfaceGO.SetActive(true);
                VNM.OneMenuIsOn = true;
                VNM.CantNext = true;
                VNM.TurnOffIcons(false, VNM.Icons[1], VNM.Icons[2]);
                Time.timeScale = 0;
                AudioListener.pause = true;
                CheckNovelsStatus();
                if(FromCredits)
                {
                    EndMenu.SetActive(true);
                    notUpdate=true;
                }
                AM.Play("Confirm");
            }
            else if(interfaceGO.activeInHierarchy && !notUpdate)
            {
                interfaceGO.SetActive(false);
                GOs[0].SetActive(false);
                GOs[1].SetActive(false);
                texts[0].text = "Desenlace: ?";
                texts[1].text = "Desición: ?";
                texts[2].text = "Identificador: ?";                
                VNM.OneMenuIsOn = false;
                VNM.CantNext = false;
                AudioListener.pause = false;
                VNM.TurnOffIcons(true, VNM.Icons[1], VNM.Icons[2]);
                Time.timeScale = 1;
                if(HaveGo)
                {
                    VNM.StopAllCoroutines();
                    StartCoroutine(VNM.EndDialogueEffects(novel, novel.timeStartNovel, false));
                    VNM.AS.Stop();
                    HaveGo = false;
                    IsEnding = false;
                }
                else if(IsEnding)
                {
                    novel = defaultN.novel;
                    VNM.StopAllCoroutines();
                    StartCoroutine(VNM.EndDialogueEffects(novel, novel.timeStartNovel, false));
                    IsEnding = false;
                }
                AM.Play("Cancel");
            }
        }
    }

    void CheckNovelsStatus()
    {
        for(int x = 0; x < Novels.Length; x++)
        {
            Novels[x].transform.GetChild(0).gameObject.SetActive(!VNM.completedNovel[x]);
        }
    }

    public void UpdateConfirmationBox(NovelFC nfc)
    {
        if(nfc.isNovel && !notUpdate)
        {
            texts[0].text = "Desenlace: " + nfc.texts[0];
            texts[1].text = "Desición: " + nfc.texts[1];
            texts[2].text = "Identificador: " + nfc.texts[2];
            novel = nfc.novel;
            GOs[0].SetActive(true);
        }
        else
        {
            texts[0].text = "Desenlace: ?";
            texts[1].text = "Desición: ?";
            texts[2].text = "Identificador: ?";
            GOs[0].SetActive(false);
        }
    }

    public void TravelTime()
    {
        HaveGo=true;
    }

    public void CanUpdate()
    {
        notUpdate=false;
        EndMenu.SetActive(false);
    }
}