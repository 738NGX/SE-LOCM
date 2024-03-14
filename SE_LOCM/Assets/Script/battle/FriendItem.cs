using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class FriendItem : MonoBehaviour
{
    public GameController gc;
    public int index;
    public int WaitRound{get; private set;}
    
    private Transform button;
    private Transform waitImage;

    private void Start()
    {
        WaitRound=LocalSaveDataManager.LoadLocalData().friends[index];
        
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
        EventTrigger.Entry entryClick=new()
        {
            eventID = EventTriggerType.PointerClick
        };
        entryEnter.callback.AddListener((data)=>{OnPointerEnter((PointerEventData)data);});
        entryExit.callback.AddListener((data)=>{OnPointerExit((PointerEventData)data);});
        entryClick.callback.AddListener((data)=>{OnPointerClick((PointerEventData)data);});
        trigger.triggers.Add(entryEnter);
        trigger.triggers.Add(entryExit);
        trigger.triggers.Add(entryClick);
    }
    private void OnPointerEnter(PointerEventData data)
    {
        DisplayInstruction();
    }
    private void OnPointerExit(PointerEventData data)
    {
        DisplayInstruction();
    }
    private void OnPointerClick(PointerEventData data)
    {
        CallFriend(index);
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
    private void DisplayInstruction()
    {
        var textObj=button.Find("Text (TMP)").gameObject;
        if(textObj.activeInHierarchy) textObj.SetActive(false);
        else textObj.SetActive(true);
    }
    private void CallFriend(int index)
    {
        if(WaitRound!=0) return;
        switch(index)
        {
            case 0:
                gc.player.sp++;
                gc.DrawCards(3);
                break;
            case 1:
                gc.player.AddHP(25);
                gc.player.AddBuff(new(105,5));
                break;
            case 2:
                gc.player.ap+=2;
                gc.player.dp+=2;
                for(int i=0;i<3;i++) gc.AllAttack(3);
                gc.AddShield(3);
                gc.player.AddBuff(new(108,3));
                gc.player.AddBuff(new(301,3));
                break;
            case 3:
                gc.player.ap++;
                gc.player.sp++;
                gc.player.AddBuff(new(114,5));
                gc.player.AddBuff(new(308,1));
                break;
            case 4:
                gc.AddShield(30);
                gc.player.AddBuff(new(109,5));
                break;
            case 5:
                gc.AllAttack(20);
                for(int i=0;i<5;i++) gc.AllAttack(3);
                break;
            case 6:
                gc.player.sp+=2;
                gc.DrawCards(1);
                gc.AddShield(5);
                gc.player.AddBuff(new(108,3));
                gc.player.AddBuff(new(301,5));
                break;
            case 7:
                gc.player.buffContainer.Clarify();
                gc.player.dp+=2;
                break;
            case 8:
                gc.player.ap++;
                gc.player.AddBuff(new(201,1));
                break;
            default:
                break;
        }
        WaitRound+=5;
    }
}
