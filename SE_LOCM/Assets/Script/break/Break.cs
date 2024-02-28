using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Break : MonoBehaviour
{
    public SceneFader sf;
    public RectTransform page;
    public Text resultStringUI;
    public Transform cardSelector;
    public Transform breakContent;
    public Transform cardContent;
    public CardDisplay cardDisplay;
    public AudioClip sfxRecover;
    public AudioClip sfxCoins;
    public AudioClip sfxBuff;
    public AudioClip sfxBook;
    public AudioClip sfxCard;
    public AudioClip sfxEnd;
    private LocalSaveData localSaveData;
    private string resultString;
    private void Start()
    {
        page.DOMoveY(0,1f).From(-10);

        localSaveData=LocalSaveDataManager.LoadLocalData();
    }
    public void ExecuteBreak(int breakType)
    {
        // 0 客房;1 论战堂;2 棋室;3 书室;4 酒室;5 茶室
        switch(breakType)
        {
            case 0: Recover(); break;
            case 1: CallCardUpgrade(); break;
            case 2: AdjustCoins(); break;
            case 3: TryGetRewardBook(); break;
            case 4: AddAttack(); break;
            case 5: AddDefence(); break;
            default: break; 
        }
        
        WaitEndBreak();
    }
    private void Recover()
    {
        MainTheme.PlayAudio(sfxRecover,gameObject);
        int val=localSaveData.hpLimit/5;
        localSaveData.AdjustHP(val);
        resultString="倒头就睡睡到了天亮,回复了"+val+"点体力值.";
    }
    private void CallCardUpgrade()
    {
        MainTheme.PlayAudio(sfxCard,gameObject);
        cardSelector.gameObject.SetActive(true);

        List<Card> notUpdatedCards=localSaveData.ReadNotUpdatedCardsData();

        for(int i=0;i<notUpdatedCards.Count;i++)
        {
            int currentIndex=i;
            Card card=notUpdatedCards[i];
            
            GameObject obj=Instantiate(cardDisplay.gameObject,cardContent);
            obj.SetActive(true);
            obj.GetComponent<CardDisplay>().UpdateCardDisplayInfo(card.id);
            obj.GetComponent<CardDisplay>().displayIndex=i;
            obj.AddComponent<Button>().onClick.AddListener(() => ChangeSelectingCardIndex(currentIndex));
        }

        resultString="成功升级了一张卡牌";
    }
    public void ChangeSelectingCardIndex(int index)
    {
        foreach(Transform child in cardSelector)
        {
            if(!child.TryGetComponent<CardDisplay>(out var card)) continue;
            if(card.displayIndex==index&&child.localScale!=new Vector3(1.1f,1.1f))
            {
                child.DOScale(new Vector3(1.1f,1.1f),0.1f);
            }
            else if(card.displayIndex!=index&&child.localScale==new Vector3(1.1f,1.1f))
            {
                child.DOScale(new Vector3(1f,1f),0.1f);
            }
            else continue;
        }
    }
    private void AdjustCoins()
    {
        MainTheme.PlayAudio(sfxCoins,gameObject);
        int val=Random.Range(-100,101);
        localSaveData.AdjustCoins(val);
        if(val>0) resultString="对弈胜利,赢下了"+val+"银币.";
        else if(val<0) resultString="对弈失败,输掉了"+(-val)+"银币.";
        else resultString="对弈平局,没有赢得也没有输掉银币.";
    }
    private void TryGetRewardBook()
    {
        MainTheme.PlayAudio(sfxBook,gameObject);
        List<Book> achievedBooks=localSaveData.ReadBooksData();
        Book rewardBook=null;
        if(Random.Range(0,100)<1)
        {
            int seed=Random.Range(10,21);
            rewardBook=achievedBooks.Contains(new Book(seed)) ? null : new Book(seed);
        }
        else if(Random.Range(0,100)<5)
        {
            int seed=Random.Range(21,35);
            rewardBook=achievedBooks.Contains(new Book(seed)) ? null : new Book(seed);
        }
        if(rewardBook==null)
        {
            resultString="阅读了很多典籍,但是并没有什么特别的发现.";
            return;
        }
        localSaveData.AddBooksData(new(){rewardBook});
        resultString="阅读了很多典籍,其中《"+rewardBook.displayInfo.name+"》令人尤为印象深刻.";
    }
    private void AddAttack()
    {
        MainTheme.PlayAudio(sfxBuff,gameObject);
        localSaveData.initAp++;
        resultString="痛饮了客栈的陈年老酒,感觉自己的力量得到了提升.此后的战斗中基础攻击力+1.";
    }
    private void AddDefence()
    {
        MainTheme.PlayAudio(sfxBuff,gameObject);
        localSaveData.initDp++;
        resultString="品鉴了客栈的上等好茶,感觉自己的身心得到了放松,此后的战斗中基础防御力+1.";
    }
    private void WaitEndBreak()
    {
        resultStringUI.DOText(resultString,1f);
        foreach(Transform child in breakContent)
        {
            if(child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
            }
            else child.gameObject.SetActive(true);
        }
    }
    public void EndBreak()
    {
        MainTheme.PlayAudio(sfxEnd,gameObject);
        page.DOMoveY(-10,1f);
        LocalSaveDataManager.SaveLocalData(localSaveData);
        sf.FadeOut(MainTheme.NextScene(localSaveData));
    }
}