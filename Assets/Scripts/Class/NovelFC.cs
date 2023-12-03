using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NovelFC : MonoBehaviour, IPointerClickHandler
{
    Flowchart FC;
    public bool isNovel;
    public string[] texts;
    public Novel novel;

    private void Start() 
    {
        FC = FindObjectOfType<Flowchart>();
    }

    public void OnPointerClick (PointerEventData eventData) 
	{
		if(eventData.button == PointerEventData.InputButton.Left)
        {
            FC.UpdateConfirmationBox(this); 
        }
	}
}
