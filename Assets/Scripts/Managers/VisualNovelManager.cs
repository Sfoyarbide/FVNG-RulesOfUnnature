using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class VisualNovelManager : MonoBehaviour
{
    #region Vars
    public Novel novel;
    public Animator[] Animators;
    public Image[] Icons;
    public GameObject OptionsBox;
    public Image NameBox;
    public TextMeshProUGUI[] options;
    public int index;
    public int indexOptions;
    public int AuxIndex;
    public bool CantNext;
    public bool NotCanAdvance;
    public bool SelectionOptions;
    public bool SpecialOption;
    public bool[] completedNovel;
    public bool GAME_TEST;
    public float SpeedText;
    float TimeInStart;
    public string BeforeSoundtrack;
    public AudioSource AS;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI text;
    public Image[] SpritesCharacters;
    public Image Background;
    public DifuminationEffect[] CharactersDifumination;
    public EffectManager EM;
    public ChatLog CL;
    public Credits C;
    public ControlIcons CI;
    public bool OneMenuIsOn;
    bool CanStartTime = false;
    #endregion
    #region Init   
    private void Awake() 
    {
        Animators[0] = this.gameObject.transform.GetChild(1).GetComponent<Animator>();
        Animators[1] = this.gameObject.transform.GetChild(5).GetComponent<Animator>();
        Animators[2] = this.gameObject.transform.GetChild(7).GetComponent<Animator>();
        Animators[3] = this.gameObject.transform.GetChild(3).GetComponent<Animator>();
    }

    private void Start() 
    {
        EM = FindObjectOfType<EffectManager>();
        CL = FindObjectOfType<ChatLog>();
        C = FindObjectOfType<Credits>();
        CI = FindObjectOfType<ControlIcons>();
        if(GAME_TEST)
            StartCoroutine(StartDialogue(0));
    }

    private void Update() 
    {
        if(Input.GetMouseButtonDown(0) && !CantNext)
        {
            if(!NotCanAdvance)
            {
                if(text.text == novel.dialogues[index].line)
                {
                    if(!novel.dialogues[index].options.HaveOptions)
                    {
                        if(SpecialOption)
                        {
                            index = AuxIndex-1;
                            SpecialOption = false;
                        }
                        NextVisualNovel();
                    }
                    else
                    {
                        ExecuteOption();
                    }
                }
                else
                {    
                    StopAllCoroutines();
                    EM.StopAllCoroutines();       
                    text.text = novel.dialogues[index].line;
                    CI.SetIcon(CI.sprites[1]);
                    AS.Stop();
                    ResetAlphaImages();   
                    CheckName(); 
                    if(novel.dialogues[index].background != null)
                    {
                        Background.sprite = novel.dialogues[index].background;
                    }
                    CL.AddDialogue(novel.dialogues[index].Name, novel.dialogues[index].line);
                } 
            }
        }
        if(SelectionOptions)
        {
            if(Input.GetKeyDown(KeyCode.DownArrow) && indexOptions != novel.dialogues[index].options.options.Length-1)
            {
                indexOptions++;
                CleanOptions();
            }
            if(Input.GetKeyDown(KeyCode.UpArrow) && indexOptions != 0)
            {
                indexOptions--;
                CleanOptions();
            }
            if(Input.GetMouseButton(1))
            {
                WhatToChange();
            }
        }
        //ReceiveTime();
    }

    void ReceiveTime()
    {
        if(CanStartTime)
        {
            TimeInStart += Time.deltaTime;
            Debug.Log(TimeInStart); 
        }
    }
    #endregion
    #region Visual Novel System
    
    public IEnumerator StartDialogue(float Time)
    {
        // Time
        OneMenuIsOn = true;
        yield return new WaitForSeconds(Time);
        // Settings
        NotCanAdvance = false;
        Animators[0].SetBool("CanOpen",true); // Dialogue Box
        Animators[1].SetBool("CanOpen",true); // Flowchart icon
        Animators[2].SetBool("CanOpen",true); // Settings icon
        Animators[3].SetBool("CanOpen",true); // Chat log icon
        index = 0;
        SpeedText = novel.dialogues[index].speed;
        text.text = string.Empty;
        NameText.text = novel.dialogues[index].Name;
        // Functions
        if(novel.dialogues[index].background != null && novel.dialogues[index].MakeTransition)
        {
            StartCoroutine(BackgroundTransitionEffect(1.4f));
            yield return new WaitForSeconds(2f);
        }
        ResetImages(true);
        CheckName();
        PutImage();
        CheckSoundtrack();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(TypeLine(novel.dialogues[index]));
        CheckVoiceClip();
        CheckEffects(novel.dialogues[index]);
        StartCoroutine(StartDialogueEffects());
    }

    void NextVisualNovel()
    {
        Debug.Log("Next");
        if(index < novel.dialogues.Length-1 && novel.dialogues[index].EndNovel == false)
        {
            index++;
            EM.AM.Play("Dialogue");
            text.text = string.Empty;
            SpeedText = novel.dialogues[index].speed;
            NameText.text = novel.dialogues[index].Name;
            TimeInStart = 0;
            CanStartTime = true;
            CheckEffects(novel.dialogues[index]);
            if(!novel.dialogues[index].KickOtherCharacter)
            {
                StartCoroutine(DefualNextVisualEffect());
            }
            else
            {
                StartCoroutine(KickNextVisualEffect());
            }
        }
        else if(index+1 == novel.dialogues.Length || novel.dialogues[index].EndNovel)
        {
            NotCanAdvance = true;
            completedNovel[novel.idFC] = true;
            StartCoroutine(EndDialogueEffects(novel.nextNovel, novel.nextNovel.timeStartNovel, novel.isEnding));
        }
    }

    public void updateThisNovel()
    {
        StopAllCoroutines();
        Animators[0].SetBool("CanOpen",true); // Dialogue Box
        Animators[1].SetBool("CanOpen",true); // Flowchart icon
        Animators[2].SetBool("CanOpen",true); // Settings icon
        Animators[3].SetBool("CanOpen",true); // Chat log icon
        SpeedText = novel.dialogues[index].speed;
        text.text = string.Empty;
        NameText.text = novel.dialogues[index].Name;
        ResetImages(true);
        CheckName();
        PutImage();
        CheckSoundtrack(true);
        CheckVoiceClip();
        StartCoroutine(TypeLine(novel.dialogues[index]));
        CheckEffects(novel.dialogues[index]);
        StartCoroutine(StartDialogueEffects());
        CantNext = false;
    }

    IEnumerator TypeLine(Dialogue dialogue)
    {
        CI.SetIcon(CI.sprites[0]);
        foreach (char i in dialogue.line.ToCharArray())
        {
            text.text += i;
            yield return new WaitForSeconds(SpeedText);
        }
        CL.AddDialogue(novel.dialogues[index].Name, novel.dialogues[index].line);
        CI.SetIcon(CI.sprites[1]);
        TimeInStart = 0;
        CanStartTime = false;
    }
    #endregion
    #region Show Characters System
    void ResetImages(bool UseReset)
    {
        if(novel.dialogues[index].KickOtherCharacter || UseReset)
        {
            SpritesCharacters[0].gameObject.SetActive(false);
            SpritesCharacters[1].gameObject.SetActive(false);
            SpritesCharacters[2].gameObject.SetActive(false);
        }
    }

    void PutImage()
    {   
        if(novel.dialogues[index].background != null && !novel.dialogues[index].MakeTransition)
        {
            Background.sprite = novel.dialogues[index].background;
        }
        for(int x = 0; x < novel.dialogues[index].character.Length; x++)
        {
            if(novel.dialogues[index].character[x] != null)
            {
                SpritesCharacters[x].sprite = novel.dialogues[index].character[x];
                SpritesCharacters[x].gameObject.SetActive(true);
            }
        }
    }
    #endregion
    #region Options System  
    void ExecuteOption()
    {
        CleanOptions();
        OptionsBox.SetActive(true);
        CI.OptionsIconOn();
        for(int x = 0; x < novel.dialogues[index].options.options.Length; x++)
        {
            options[x].gameObject.SetActive(true);
            options[x].text = novel.dialogues[index].options.options[x];
        }
        SelectionOptions = true;
    }
    void WhatToChange()
    {
        CL.AddDialogue("Protagonist", novel.dialogues[index].options.options[indexOptions]);
        switch (novel.dialogues[index].options.WhatToChange)
        {
            case 0:
                OptionsBox.SetActive(false);
                novel.nextNovel = novel.dialogues[index].options.novel[indexOptions];
                NextVisualNovel();
                break;
            case 1:
                index = novel.dialogues[index].options.index[indexOptions]-1;
                OptionsBox.SetActive(false);
                NextVisualNovel();
                break;
            case 2:
                SceneManager.LoadScene(index = novel.dialogues[index].options.index[indexOptions]-1);
                break;
        }
        EM.AM.Play("Dialogue");
        CI.OptionsIconOff();
        SelectionOptions = false;
        indexOptions = 0;
    }
    #endregion
    #region Effects System
    // Check differents effects
    void CheckEffects(Dialogue dialogue)
    {
        for(int x = 0; x < dialogue.effect.Length; x++)
        {
            EM.ExecuteEffect(dialogue.effect[x]);
        }
    }
    void CleanOptions()
    {
        options[0].gameObject.GetComponentInChildren<Image>().color = new Color32(255,255,255,0);
        options[1].gameObject.GetComponentInChildren<Image>().color = new Color32(255,255,255,0);
        options[2].gameObject.GetComponentInChildren<Image>().color = new Color32(255,255,255,0);
        options[indexOptions].gameObject.GetComponentInChildren<Image>().color = new Color32(255,255,255,120);
    }
    void CheckSoundtrack(bool isLoad=false)
    {
        if(novel.dialogues[index].pauseSoundtrack)
        {
            StartCoroutine(EM.AM.QuitSoundtrackSlow(BeforeSoundtrack));
        }
        if(novel.dialogues[index].soundtrack != "")
        {
            if(BeforeSoundtrack != "")
            {
                StartCoroutine(EM.AM.QuitSoundtrackSlow(BeforeSoundtrack));
            }
            BeforeSoundtrack = novel.dialogues[index].soundtrack;
            EM.AM.Play(BeforeSoundtrack);
        }
        else if(isLoad)
        {
            EM.AM.Play(BeforeSoundtrack);
        }
    }
    void CheckVoiceClip()
    {
        if(novel.dialogues[index].voice != null)
        {
            AS.clip = novel.dialogues[index].voice;
            AS.Play();
        }
    }
    void CheckName()
    {
        if(novel.dialogues[index].Name == "")
        {
            NameBox.gameObject.SetActive(false);
        }
        else if(novel.dialogues[index].Name != "")
        {
            NameBox.gameObject.SetActive(true);     
        }
    }
    // Next Visual Funcion effects
    IEnumerator DefualNextVisualEffect()
    {
        NotCanAdvance = true;
        if(novel.dialogues[index].background != null && novel.dialogues[index].MakeTransition)
        {
            StartCoroutine(BackgroundTransitionEffect(1.5f));
            yield return new WaitForSeconds(2f);
        }
        NotCanAdvance = false;
        StartCoroutine(TypeLine(novel.dialogues[index]));
        CheckVoiceClip();
        CheckSoundtrack();
        CheckName();
        PutImage();
    }
    IEnumerator KickNextVisualEffect()
    { 
        NotCanAdvance = true;
        for(int x = CharactersDifumination.Length-1; x > -1; x--)
        {
            if(CharactersDifumination[x].gameObject.activeInHierarchy != false)
            {
                CharactersDifumination[x].image.CrossFadeAlpha(0, 0.5f, false);
                yield return new WaitForSeconds(0.5f);
                SpritesCharacters[x].gameObject.SetActive(false);
            }
            else
            {
                CharactersDifumination[x].image.color -= new Color(0,0,0,255);
                SpritesCharacters[x].gameObject.SetActive(false);
            }            
        }
        if(novel.dialogues[index].background != null && novel.dialogues[index].MakeTransition)
        {
            StartCoroutine(BackgroundTransitionEffect(1.4f));
            yield return new WaitForSeconds(2f);
        }
        NotCanAdvance = false;
        StartCoroutine(TypeLine(novel.dialogues[index]));
        CheckVoiceClip();
        CheckSoundtrack();
        CheckName();
        yield return new WaitForSeconds(0.1f);
        PutImage();            
        StartCoroutine(DisableAllDifumination());
    }
    // Background effects
    IEnumerator BackgroundTransitionEffect(float timer)
    {
        Background.gameObject.GetComponent<Animator>().SetTrigger("Changing");
        yield return new WaitForSeconds(timer);
        Background.sprite = novel.dialogues[index].background;
    }
    // Kick other characters effects
    IEnumerator DisableAllDifumination(float timer = 0)
    {
        for(int x = 0; x < CharactersDifumination.Length; x++)
        {
            if(CharactersDifumination[x].gameObject.activeInHierarchy != false)
            {
                CharactersDifumination[x].DisableDifumination();
                yield return new WaitForSeconds(timer);
            }
            else
            {
                CharactersDifumination[x].image.color = new Color(CharactersDifumination[x].image.color.r, CharactersDifumination[x].image.color.g,CharactersDifumination[x].image.color.b,255);
            }
        }
        yield return new WaitForSeconds(0.2f);
        NotCanAdvance = false;
    }
    // Start and ending of a Novel
    IEnumerator StartDialogueEffects()
    {
        for(int x = 0; x < CharactersDifumination.Length; x++)
        {
            if(CharactersDifumination[x].gameObject.activeInHierarchy != false)
            {
                CharactersDifumination[x].DisableDifumination();
                yield return new WaitForSeconds(1f);
            }
            else
            {
                CharactersDifumination[x].image.color = new Color(CharactersDifumination[x].image.color.r, CharactersDifumination[x].image.color.g,CharactersDifumination[x].image.color.b,255);
            }
        }
        OneMenuIsOn = false;
    } 
    public IEnumerator EndDialogueEffects(Novel nextNovel, float time, bool isEnding)
    {
        if(!isEnding)
        {
            for(int x = CharactersDifumination.Length-1; x > -1; x--)
            {
                if(CharactersDifumination[x].gameObject.activeInHierarchy != false)
                {
                    CharactersDifumination[x].image.CrossFadeAlpha(0, 0.5f, false);
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    CharactersDifumination[x].image.color = new Color(CharactersDifumination[x].image.color.r, CharactersDifumination[x].image.color.g,CharactersDifumination[x].image.color.b,0);
                }
            } 
            StartCoroutine(FadeText(text, 0.02f));
            StartCoroutine(FadeText(NameText, 0.05f));    
            Animators[0].SetBool("CanOpen",false); // Dialogue Box
            Animators[1].SetBool("CanOpen",false); // Flowchart icon
            Animators[2].SetBool("CanOpen",false); // Settings icon
            Animators[3].SetBool("CanOpen",false); // Chat log icon
            novel = nextNovel;
            StartCoroutine(StartDialogue(time));
        }
        else
        {
            yield return new WaitForSeconds(novel.TimeToCredits);
            C.parentGO.SetActive(true);
            StartCoroutine(C.ReproduceCredits());
        }
    }    
    // Reset Alpha Images
    void ResetAlphaImages()
    {
        CharactersDifumination[0].image.color += new Color(0,0,0,255);
        CharactersDifumination[1].image.color += new Color(0,0,0,255);
        CharactersDifumination[2].image.color += new Color(0,0,0,255);
    }    
    // For icons in the ui
    public void TurnOffIcons(bool setbool, Image icon1, Image icon2)
    {
        icon1.enabled = setbool;
        icon1.transform.GetChild(0).gameObject.SetActive(setbool);
        icon1.transform.GetChild(1).gameObject.SetActive(setbool);
        icon2.enabled = setbool;  
        icon2.transform.GetChild(0).gameObject.SetActive(setbool);
        icon2.transform.GetChild(1).gameObject.SetActive(setbool);         
    }
    // Text fade
    IEnumerator FadeText(TextMeshProUGUI text, float time)
    {
        foreach(int i in text.text)
        {
            if(text.text != "")
            {
                text.text = text.text.Remove(text.text.Length - 1);
                yield return new WaitForSeconds(time);
            }
        }
    }

    #endregion
    #region Save And Load System
    public void SavePlayerData(string NameFile)
    {
        SaveLoadManager.SavePlayerData(this, NameFile);
    }

    public VNMData LoadPlayerData(string NameFile, bool CheckData=false)
    {
        VNMData data = SaveLoadManager.LoadPlayerData(this, NameFile);

        if(data != null && !CheckData)
        {
            novel = Resources.Load("Novel/" + data.IdNovel) as Novel;
            Background.sprite = Resources.Load<Sprite>("Background/" + data.IdBackground);
            index = data.index;
            indexOptions = data.indexOptions;
            AuxIndex = data.AuxIndex;
            CantNext = data.CantNext;
            NotCanAdvance = data.NotCanAdvance;
            SelectionOptions = data.SelectionOptions;
            SpecialOption = data.SpecialOption;
            SpeedText = data.SpeedText;
            BeforeSoundtrack = data.BeforeSoundtrack;
            completedNovel = data.completedNovel;
            return data;
        }
        else
        {
            return data;
        }
    }
    #endregion
}