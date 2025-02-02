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
    private int rewardCoins = 0;
    private Stack<List<Card>> rewardCards = new();
    private int selectingRewardCard = -1;
    private Book rewardBook = null;
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
        page.DOMove(new Vector3(0, 0), 1f).From(new Vector3(0, -10));

        // 读取存档
        localSaveData = LocalSaveDataManager.LoadLocalData();

        if (localSaveData.ContainsBook(12))
        {
            // 海岛算经效果:每进入一次奖励界面获得15银币。
            localSaveData.coins += 15;
        }
        if (localSaveData.ContainsBook(1))
        {
            // 方田章残卷
            localSaveData.AdjustHP(5);
        }
        if (localSaveData.ContainsBook(7))
        {
            // 盈不足章残卷
            localSaveData.AdjustHP(10);
        }

        // 确定当前位置并且解锁新卡池
        MapNodeData currentNode = MapDatabase.data[localSaveData.route[^1]];
        localSaveData.AddCardsPool(currentNode.unlockCardsPool);

        // 生成奖励
        rewardCoins = currentNode.RewardCoins();
        for (int i = 0; i < currentNode.RewardCardsNum; i++)
        {
            rewardCards.Push(currentNode.RewardCards(localSaveData.ReadCardsPool()));
        }
        rewardBook = currentNode.RewardBook(localSaveData.ReadBooksData());

        // 产生奖励按钮
        if (rewardCoins > 0)
        {
            coinRewardButton.gameObject.SetActive(true);
            coinRewardButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "银币+" + rewardCoins;
        }
        for (int i = 0; i < currentNode.RewardCardsNum; i++)
        {
            GameObject obj = Instantiate(cardRewardButton.gameObject, transform);
            obj.SetActive(true);
        }
        if (rewardBook != null)
        {
            bookRewardButton.gameObject.SetActive(true);
            bookRewardButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = BookDatabase.data[rewardBook.id].name;
            bookRewardButton.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/books/" + rewardBook.id.ToString("D3"));
        }
        // 产生继续冒险按钮
        GameObject newContinue = Instantiate(continueButton.gameObject, transform);
        newContinue.SetActive(true);
    }
    public void RewardCoins()
    {
        localSaveData.coins += rewardCoins;
    }
    public void WaitRewardCard()
    {
        cardSelector.gameObject.SetActive(true);
        List<Card> rewardCard = rewardCards.Pop();
        for (int i = 0; i < rewardCard.Count; i++)
        {
            GameObject card = Instantiate(cardTemplate.gameObject, cardSelector);
            card.SetActive(true);
            int currentCardId = rewardCard[i].id;
            card.AddComponent<Button>().onClick.AddListener(() => ChangeSelectingRewardCard(currentCardId));
            float cardPositionX = i * 800f - (rewardCard.Count - 1) * 800f / 2;
            card.transform.localPosition = new Vector3(cardPositionX, 0, 0);
            card.GetComponent<CardDisplay>().UpdateCardDisplayInfo(rewardCard[i].id);
            card.transform.DOMove(new Vector3(0, 0), 0.25f).From();
        }
    }
    public void ChangeSelectingRewardCard(int id)
    {
        selectingRewardCard = id;
        foreach (Transform child in cardSelector)
        {
            if (!child.TryGetComponent<CardDisplay>(out var card)) continue;
            if (card.id == selectingRewardCard && child.localScale != new Vector3(1.6f, 1.6f))
            {
                child.DOScale(new Vector3(1.6f, 1.6f), 0.1f);
            }
            else if (card.id != selectingRewardCard && child.localScale == new Vector3(1.6f, 1.6f))
            {
                child.DOScale(new Vector3(1.5f, 1.5f), 0.1f);
            }
            else continue;
        }
    }
    public void RewardCard()
    {
        if (selectingRewardCard == -1) return;
        localSaveData.AddCardsData(new() { new Card(selectingRewardCard, false) });
        selectingRewardCard = -1;
        foreach (Transform child in cardSelector)
        {
            if (!child.gameObject.activeInHierarchy) continue;
            if (!child.TryGetComponent<CardDisplay>(out var card)) continue;
            Destroy(card.gameObject);
        }
        cardSelector.gameObject.SetActive(false);
    }
    public void SkipRewardCard()
    {
        selectingRewardCard = -1;
        foreach (Transform child in cardSelector)
        {
            if (!child.gameObject.activeInHierarchy) continue;
            if (!child.TryGetComponent<CardDisplay>(out var card)) continue;
            Destroy(card.gameObject);
        }
        cardSelector.gameObject.SetActive(false);
    }
    public void RewardBook()
    {
        localSaveData.AddBooksData(new() { rewardBook });
        switch (rewardBook.id)
        {
            case 1:
                localSaveData.hpLimit += 10;
                localSaveData.hp += 10;
                localSaveData.friends[1]=0;
                break;
            case 2:
                localSaveData.initSp++;
                localSaveData.friends[2]=0;
                break;
            case 3:
                localSaveData.friends[3]=0;
                break;
            case 4:
                localSaveData.friends[4]=0;
                break;
            case 5:
                localSaveData.friends[5]=0;
                break;
            case 6:
                localSaveData.friends[6]=0;
                break;
            case 7:
                localSaveData.friends[7]=0;
                localSaveData.hpLimit += 20;
                localSaveData.hp += 20;
                break;
            case 8:
                localSaveData.friends[8]=0;
                localSaveData.initSp += 2;
                break;
            case 9: 
                break;
            default: break;
        }
    }
    public void Continue()
    {
        page.DOMove(new Vector3(0, -10), 1f);
        LocalSaveDataManager.SaveLocalData(localSaveData);
        sf.FadeOut(MainTheme.NextScene(localSaveData));
    }
}
