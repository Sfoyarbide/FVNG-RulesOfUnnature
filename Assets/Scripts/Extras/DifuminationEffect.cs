using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifuminationEffect : MonoBehaviour
{
    public Image image;
    public bool cannD;
    public float timer = 1.5f;
    // Start is called before the first frame update

    private void Start() 
    {
        image = GetComponent<Image>();
    }

    private void Awake() 
    {
        if(image == null)
        {
            image = GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        disableDifuminationEffect();
        CheckColorState();
    }

    // Up alpha (255)
    public void disableDifuminationEffect()
    {
        if(cannD)
        {
            image.color += new Color(0,0,0, timer * Time.deltaTime);
        }
    }

    void CheckColorState()
    { 
        if(image.color == new Color32(255, 255, 255, 255))
        {
            cannD = false;
        }
    }

    public void setDifumination(float time = 0, bool DestroyObject = false)
    {
        image.CrossFadeAlpha(0, time, false);
        if(DestroyObject)
        {
            Destroy(this.gameObject, 5f);
        }    
    }

    public void DisableDifumination(float time = 0, bool DestroyObject = false)
    {
        if(time != 0)
        {
            timer = time;
        }
        cannD = true;
        if(DestroyObject)
        {
            Destroy(this.gameObject, 5f);
        }    
    }
}