using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatLog : MonoBehaviour
{
    Vector3 savePosText;
    public VisualNovelManager VNM;
    AudioManager AM;
    public GameObject BGtext;
    public TextMeshProUGUI pDialogues;
    public bool FirstTime = true;
    public int count;
    // Start is called before the first frame update
    void Start()
    {
        VNM = FindObjectOfType<VisualNovelManager>();
        AM = FindObjectOfType<AudioManager>();
    }

    private void Update() 
    {
        ChatLogMenu();
    }

    void CheckCount()
    {
        if(count >= 5)
        {
            pDialogues.transform.position += new Vector3(0,110,0);
        }
    }

    public void AddDialogue(string name, string dialogue)
    {
        if(!FirstTime)
        {
            pDialogues.text += "\n\n";
            pDialogues.text += name + ": " + dialogue;
            count++;
        }
        else
        {
            pDialogues.text += name + ": " + dialogue;
            count++;
            FirstTime = false;
        }
        CheckCount();
    }

    public void ChatLogMenu()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(!VNM.OneMenuIsOn && !VNM.NotCanAdvance)
            {
                BGtext.SetActive(true);
                VNM.CantNext = true;
                VNM.OneMenuIsOn = true;
                savePosText = pDialogues.gameObject.transform.position;
                VNM.TurnOffIcons(false, VNM.Icons[0],VNM.Icons[1]);
                Time.timeScale = 0;
                AudioListener.pause = true;
                AM.Play("Confirm");                  
            }
            else if(BGtext.activeInHierarchy)
            {
                BGtext.SetActive(false);              
                VNM.CantNext = false;
                VNM.OneMenuIsOn = false;
                pDialogues.gameObject.transform.position = savePosText;
                VNM.TurnOffIcons(true, VNM.Icons[0],VNM.Icons[1]);                
                Time.timeScale = 1;
                AudioListener.pause = false;
                AM.Play("Cancel");
            }
        }
    }    
}
