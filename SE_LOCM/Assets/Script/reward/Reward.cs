using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;

public class Reward : MonoBehaviour
{
    private LocalSaveData localSaveData;
    private int rewardCoins=0;
    private Stack<List<Card>> rewardCards=new();
    private int selectingRewardCard=-1;
    private Book rewardBook=null;
    public Transform page;
    public Button coinRewardButton;
    public Button cardRewardButton;
    public Button bookRewardButton;
    public Button continueButton;
    public SceneFader sf;
    public Transform cardSelector;
    public CardDisplay cardTemplate;

    private void Start()
    {
        page.DOMove(new Vector3(0,0),1f).From(new Vector3(0,-10));

        // 读取存档
        localSaveData=LocalSaveDataManager.LoadLocalData();

        if(localSaveData.ContainsBook(12))
        {
            // 海岛算经效果:每进入一次奖励界面获得15银币。
            localSaveData.coins+=15;
        }

        // 确定当前位置并且解锁新卡池
        MapNodeData currentNode=MapDatabase.data[localSaveData.route[^1]];
        localSaveData.AddCardsPool(currentNode.unlockCardsPool);
        
        // 生成奖励
        rewardCoins=currentNode.RewardCoins();
        for(int i=0;i<currentNode.RewardCardsNum;i++)
        {
            rewardCards.Push(currentNode.RewardCards(localSaveData.ReadCardsPool()));
        }
        rewardBook=currentNode.RewardBook(localSaveData.ReadBooksData());

        // 产生奖励按钮
        if(rewardCoins>0)
        {
            coinRewardButton.gameObject.SetActive(true);
            coinRewardButton.transform.GetComponentInChildren<TextMeshProUGUI>().text="银币+"+rewardCoins;
        }
        for(int i=0;i<currentNode.RewardCardsNum;i++)
        {
            GameObject obj=Instantiate(cardRewardButton.gameObject,transform);
            obj.SetActive(true);
        }
        if(rewardBook!=null)
        {
            bookRewardButton.gameObject.SetActive(true);
            bookRewardButton.transform.GetComponentInChildren<TextMeshProUGUI>().text=BookDatabase.data[rewardBook.id].name;
            bookRewardButton.transform.Find("Image").GetComponent<Image>().sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/books/"+rewardBook.id.ToString("D3")+".png");
        }
        // 产生继续冒险按钮
        GameObject newContinue=Instantiate(continueButton.gameObject,transform);
        newContinue.SetActive(true);
    }
    public void RewardCoins()
    {
        localSaveData.coins+=rewardCoins;
    }
    public void WaitRewardCard()
    {
        cardSelector.gameObject.SetActive(true);
        List<Card> rewardCard=rewardCards.Pop();
        for(int i=0;i<rewardCard.Count;i++)
        {
            GameObject card=Instantiate(cardTemplate.gameObject,cardSelector);
            card.SetActive(true);
            int currentCardId=rewardCard[i].id;
            card.AddComponent<Button>().onClick.AddListener(() => ChangeSelectingRewardCard(currentCardId));
            float cardPositionX=i*800f-(rewardCard.Count-1)*800f/2;
            card.transform.localPosition=new Vector3(cardPositionX,0,0);
            card.GetComponent<CardDisplay>().UpdateCardDisplayInfo(rewardCard[i].id);
            card.transform.DOMove(new Vector3(0,0),0.25f).From();
        }
    }
    public void ChangeSelectingRewardCard(int id)
    {
        selectingRewardCard=id;
        foreach(Transform child in cardSelector)
        {
            if(!child.TryGetComponent<CardDisplay>(out var card)) continue;
            if(card.id==selectingRewardCard&&child.localScale!=new Vector3(1.6f,1.6f))
            {
                child.DOScale(new Vector3(1.6f,1.6f),0.1f);
            }
            else if(card.id!=selectingRewardCard&&child.localScale==new Vector3(1.6f,1.6f))
            {
                child.DOScale(new Vector3(1.5f,1.5f),0.1f);
            }
            else continue;
        }
    }
    public void RewardCard()
    {
        if(selectingRewardCard==-1) return;
        localSaveData.AddCardsData(new(){new Card(selectingRewardCard,false)});
        selectingRewardCard=-1;
        foreach(Transform child in cardSelector)
        {
            if(!child.gameObject.activeInHierarchy) continue;
            if(!child.TryGetComponent<CardDisplay>(out var card)) continue;
            Destroy(card.gameObject);
        }
        cardSelector.gameObject.SetActive(false);
    }
    public void RewardBook()
    {
        localSaveData.AddBooksData(new(){rewardBook});
    }
    public void Continue()
    {
        page.DOMove(new Vector3(0,-10),1f);
        LocalSaveDataManager.SaveLocalData(localSaveData);
        sf.FadeOut(MainTheme.NextScene(localSaveData));
    }
}
