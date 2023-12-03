using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public VisualNovelManager VNM;
    MainMenu MM;
    AudioManager AM;
    ChatLog CL;
    public GameObject interfaceGO;
    public GameObject[] GOs;
    bool HaveLoad;
    bool GoMainMenu;
    // Start is called before the first frame update
    void Start()
    {
        VNM = GameObject.FindObjectOfType<VisualNovelManager>();
        AM = GameObject.FindObjectOfType<AudioManager>();
        MM = FindObjectOfType<MainMenu>();
        CL = FindObjectOfType<ChatLog>();
    }

    // Update is called once per frame
    void Update()
    {
        OpenSettingsMenu();
    }

    public void OpenSettingsMenu(bool IsReturn=false)
    {
        if(Input.GetKeyDown(KeyCode.X) || IsReturn)
        {
            if(!VNM.OneMenuIsOn && !VNM.NotCanAdvance)
            {
                interfaceGO.SetActive(true);
                VNM.OneMenuIsOn = true;
                VNM.CantNext = true;
                VNM.TurnOffIcons(false, VNM.Icons[0], VNM.Icons[2]);
                Time.timeScale = 0;
                AudioListener.pause = true;
                AM.Play("Confirm");
            }
            else if(interfaceGO.activeInHierarchy || IsReturn)
            {
                interfaceGO.SetActive(false);
                GOs[0].SetActive(true);
                GOs[1].SetActive(true);
                GOs[2].SetActive(true);
                GOs[3].SetActive(false);
                GOs[4].SetActive(false);
                GOs[5].SetActive(false);
                VNM.OneMenuIsOn = false;
                VNM.CantNext = false;
                AudioListener.pause = false;  
                VNM.TurnOffIcons(true, VNM.Icons[0], VNM.Icons[2]);       
                Time.timeScale = 1;
                if(HaveLoad)
                {
                    CL.pDialogues.text = "";
                    VNM.updateThisNovel();
                    HaveLoad = false;
                }
                else if(GoMainMenu)
                {
                    MM.OpenmainMenu();
                    GoMainMenu = false;
                }
                AM.Play("Cancel");
            }
        }
    }

    public void SaveGameButton()
    {
        VNM.SavePlayerData("PlayerData.data");
    }

    public void LoadGameButton()
    {
        VNM.LoadPlayerData("PlayerData.data");
        HaveLoad = true;
    }

    public void ReturnToMainMenu()
    {
        GoMainMenu = true;
        OpenSettingsMenu(true);       
    }
}
