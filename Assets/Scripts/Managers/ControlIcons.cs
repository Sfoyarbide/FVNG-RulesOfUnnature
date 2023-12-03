using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlIcons : MonoBehaviour
{
    public Sprite[] sprites;
    public Image[] icons;
    VisualNovelManager VNM;

    private void Start() 
    {
        VNM = FindObjectOfType<VisualNovelManager>();
    }

    private void Update() 
    {
        if(VNM.NotCanAdvance)
        {
            SetIcon(sprites[3]);
        }
    }

    public void SetIcon(Sprite sprite)
    {
        icons[0].sprite = sprite;
    }
    
    public void OptionsIconOn()
    {
        icons[0].sprite = sprites[2];
        icons[1].gameObject.SetActive(true);
        icons[2].gameObject.SetActive(true);        
    }

    public void OptionsIconOff()
    {
        icons[0].sprite = sprites[2];
        icons[1].gameObject.SetActive(false);
        icons[2].gameObject.SetActive(false);        
    }

}
