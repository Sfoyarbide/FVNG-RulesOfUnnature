using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public VisualNovelManager VNM;
    public AudioManager AM;
    public GameObject mainMenu;
    public GameObject settingsGO;
    public GameObject[] effectsGO;
    public bool haveUse;
    public Image[] pngs;
    ChatLog CL;
    bool canAlpha;
    Image alpha;

    // Start is called before the first frame update
    void Start()
    {
        VNM = FindObjectOfType<VisualNovelManager>();
        AM = FindObjectOfType<AudioManager>();
        CL = FindObjectOfType<ChatLog>();
        if(!VNM.GAME_TEST)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            VNM.OneMenuIsOn = true;
            if(VNM.LoadPlayerData("PlayerData.data", true) != null)
            {
                effectsGO[1].SetActive(true);
            }
        }
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.X) && !haveUse && mainMenu.activeInHierarchy && !settingsGO.activeInHierarchy)
        {
            AM.Play("Confirm");
            Debug.Log("nEWgAME");
            StartCoroutine(loadingNewGame());
            haveUse = true;
        }
        if(Input.GetKeyDown(KeyCode.Z) && !haveUse && mainMenu.activeInHierarchy && VNM.LoadPlayerData("PlayerData.data", true) != null)
        {
            AM.Play("Confirm");            
            StartCoroutine(loading());
            haveUse = true;
        }
        if(Input.GetKeyDown(KeyCode.C) && !haveUse && mainMenu.activeInHierarchy)
        {
            AM.Play("Confirm");
            Application.Quit();
            haveUse = true;
        }
        if(canAlpha)
        {
            alpha.color += new Color32(255,255,255,1); 
        }
    }

    public void OpenmainMenu()
    {        
        Time.timeScale = 0;
        VNM.AS.Stop();
        AudioListener.pause = true;
        CL.pDialogues.text = "";
        VNM.OneMenuIsOn = true;
        mainMenu.SetActive(true);
        haveUse = false;
        VNM.StopAllCoroutines();
        effectsGO[0].SetActive(true);
        effectsGO[2].SetActive(true);  
        effectsGO[3].SetActive(true); 
        canAlpha = false;
        pngs[0].color = new Color32(255,255,255,255);
        pngs[2].color = new Color32(255,255,255,0);
        pngs[3].color = new Color32(36,40,43,255);
        if(VNM.LoadPlayerData("PlayerData.data", true) != null)
        {
            effectsGO[1].SetActive(true);
        }  
    }

    IEnumerator loading()
    {
        // EFFECTS
        effectsGO[0].SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        effectsGO[1].SetActive(false);
        yield return new WaitForSecondsRealtime(1f);        
        effectsGO[2].SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        pngs[0].CrossFadeAlpha(0,2,true);
        yield return new WaitForSecondsRealtime(2f);  
        effectsGO[4].SetActive(true);           
        yield return new WaitForSecondsRealtime(4f);
        Time.timeScale = 0;
        VNM.LoadPlayerData("PlayerData.data");
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
        AudioListener.pause = false;
        VNM.updateThisNovel();
        mainMenu.SetActive(false);
    }

    IEnumerator loadingNewGame()
    {
        // EFFECTS
        effectsGO[0].SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        effectsGO[1].SetActive(false);
        yield return new WaitForSecondsRealtime(1f);        
        effectsGO[2].SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        pngs[0].CrossFadeAlpha(0,2,true);
        yield return new WaitForSecondsRealtime(2f);
        pngs[3].CrossFadeColor(new Color32(0,0,0,255),1f,true,false);
        yield return new WaitForSecondsRealtime(1f);
        alpha = pngs[1];
        canAlpha = true; 
        yield return new WaitForSecondsRealtime(4f);
        pngs[1].color = new Color32(255,255,255,0);    
        alpha = pngs[2];      
        // Title
        yield return new WaitForSecondsRealtime(6f);
        VNM.NotCanAdvance = true;
        StartCoroutine(VNM.StartDialogue(0));
        yield return new WaitForSecondsRealtime(2f);
        mainMenu.SetActive(false);
        mainMenu.SetActive(false);
        AudioListener.pause = false;
        Time.timeScale = 1;       
    }
}
