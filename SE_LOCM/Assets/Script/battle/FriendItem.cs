using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FriendItem : MonoBehaviour
{
    public int index;
    public int WaitRound{get; private set;}
    
    private Transform button;
    private Transform waitImage;

    private void Start()
    {
        if(WaitRound<0) gameObject.SetActive(false);
        waitImage=transform.Find("wait");
        button=transform.Find("Image");
        
        EventTrigger trigger=button.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entryEnter=new()
        {
            eventID = EventTriggerType.PointerEnter
        };
        EventTrigger.Entry entryExit=new()
        {
            eventID = EventTriggerType.PointerExit
        };
        entryEnter.callback.AddListener((data)=>{OnPointerEnter((PointerEventData)data);});
        entryExit.callback.AddListener((data)=>{OnPointerExit((PointerEventData)data);});
        trigger.triggers.Add(entryEnter);
        trigger.triggers.Add(entryExit);
    }
    public void OnPointerEnter(PointerEventData data)
    {
        DisplayInstruction();
    }
    public void OnPointerExit(PointerEventData data)
    {
        DisplayInstruction();
    }
    private void Update()
    {
        if(WaitRound==0&&waitImage.gameObject.activeInHierarchy)
        {
            waitImage.gameObject.SetActive(false);
        }
        if(WaitRound!=0&&!waitImage.gameObject.activeInHierarchy)
        {
            waitImage.gameObject.SetActive(true);
        }
        waitImage.GetComponentInChildren<TextMeshProUGUI>().text=WaitRound.ToString();
    }
    public void DisplayInstruction()
    {
        var textObj=button.Find("Text (TMP)").gameObject;
        if(textObj.activeInHierarchy) textObj.SetActive(false);
        else textObj.SetActive(true);
    }
}
