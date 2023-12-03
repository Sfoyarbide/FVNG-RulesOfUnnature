using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Credits : MonoBehaviour
{   
    public GameObject parentGO;
    public RectTransform credits;
    public TextMeshProUGUI end;
    public Flowchart FC; 
    AudioManager AM;
    public VisualNovelManager VNM;
    public Vector3 startPos;
    public Vector3 endPos;
    public float speed;
    bool CanMove = false;
    // Start is called before the first frame update
    void Start()
    {
        VNM = FindObjectOfType<VisualNovelManager>();
        FC = FindObjectOfType<Flowchart>();
        AM = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        canMove();
    }

    void canMove()
    {
        if(CanMove)
        {
            if(credits.localPosition.y >= endPos.y)
            {
                Debug.Log("Here");
                CanMove = false;
                StartCoroutine(EndCredits());
            }
            else
            {
                credits.position += new Vector3(0,speed * Time.deltaTime);
            }
        }
    }

    IEnumerator EndCredits()
    {
        AM.QuitSoundtrackSlow("track_05");
        yield return new WaitForSeconds(6f);
        VNM.OneMenuIsOn = false;
        VNM.NotCanAdvance = false;
        FC.IsEnding = true;
        FC.Openflowchart(true);
        parentGO.SetActive(false);
        credits.gameObject.SetActive(false);
        credits.position = startPos;
        end.color = new Color32(255,255,255,255);
    }

    public IEnumerator ReproduceCredits()
    {
        StartCoroutine(AM.QuitSoundtrackSlow(VNM.BeforeSoundtrack));
        VNM.OneMenuIsOn = true;
        yield return new WaitForSeconds(4f);
        end.CrossFadeColor(new Color(255,255,255,0),2.5f,true,true);
        yield return new WaitForSeconds(5f);
        AM.Play("track_05");
        credits.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        CanMove = true;
    }
}
