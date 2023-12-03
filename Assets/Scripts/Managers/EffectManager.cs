using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EffectManager : MonoBehaviour
{
    #region Init
    [Header("References")]
    GameObject UI;
    VisualNovelManager VMN;
    [HideInInspector]
    public AudioManager AM;
    public Animator[] Anims; 
    public GameObject[] Prefabs;
    public SpriteRenderer black;

    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("UI");
        VMN = FindObjectOfType<VisualNovelManager>();   
        AM = FindObjectOfType<AudioManager>();
    }

    #endregion
    #region Effect System 
    public void ExecuteEffect(Effect effect) 
    {
        switch (effect.Type)
        {
            case TypeEffect.Earthquake:
                StartCoroutine(Earthquake(effect.Floats[0]));
                break;
            case TypeEffect.Lightning:
                StartCoroutine(Lightning(effect.Floats[0], effect.Floats[1]));
                break;
            case TypeEffect.TriggerSound:
                StartCoroutine(TriggerSound(effect.Floats[0], effect.audioclip));
                break;
            case TypeEffect.StopSound:
                StartCoroutine(StopSound(effect.Floats[0], effect.audioclip));
                break;
            case TypeEffect.ChangeSpeed:
                StartCoroutine(ChangeSpeed(effect.Floats[0], effect.Floats[1]));
                break;
            case TypeEffect.ChangeIndex:
                // Change Index
                ChangeIndex(((int)effect.Floats[0]));
                break;
        }
    }
    #endregion
    #region Types Effect
    public IEnumerator Earthquake(float time)
    {
        black.color = new Color32(255,255,255,255);
        yield return new WaitForSeconds(time);
        Anims[0].SetBool("EarthQuake", true);
        yield return new WaitForSeconds(0.5f);
        Anims[0].SetBool("EarthQuake", false);
        black.color = new Color32(0,0,0,255);
    }

    public IEnumerator Lightning(float time, float timer)
    {
        yield return new WaitForSeconds(time);
        GameObject LightingEffect = Instantiate(Prefabs[0], UI.transform, false);
        LightingEffect.GetComponent<DifuminationEffect>().setDifumination(timer, true);
        AM.Play("Lighting");
    }

    public IEnumerator TriggerSound(float time, string audioclip)
    {
        yield return new WaitForSeconds(time); 
        AM.Play(audioclip);
    }

    public IEnumerator StopSound(float time, string audioclip)
    {
        yield return new WaitForSeconds(time);
        AM.Pause(audioclip);
    }

    public IEnumerator ChangeSpeed(float time, float speed)
    {
        yield return new WaitForSeconds(time);
        VMN.SpeedText = speed;
    }   

    public void ChangeIndex(int index)
    {
        VMN.SpecialOption = true;
        VMN.AuxIndex = index;
    }
    #endregion
}