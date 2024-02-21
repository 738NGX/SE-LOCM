using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    private LocalSaveData localSaveData;
    private int rewardCoins=0;
    private List<List<Card>> rewardCards=new();
    private Book rewardBook=null;
    public Button coinRewardButton;
    public Button cardRewardButton;
    public Button bookRewardButton;
    public Button continueButton;

    private void Start()
    {
        // 读取存档
        localSaveData=LocalSaveDataManager.LoadLocalData();
        
        // 确定当前位置并且生成奖励
        MapNodeData currentNode=MapDatabase.data[localSaveData.route[^1]];
        rewardCoins=currentNode.RewardCoins();
        for(int i=0;i<currentNode.rewardCardsNum;i++)
        {
            rewardCards.Add(currentNode.RewardCards(localSaveData.ExportCardsPool()));
        }
        rewardBook=currentNode.RewardBook(localSaveData.ExportBooksData());

        // 产生奖励按钮
        if(rewardCoins>0) Instantiate(coinRewardButton,transform);
    }

    public void RewardClicked(Button button)
    {
        // 调用你想要执行的方法

        // 禁用按钮
        button.gameObject.SetActive(false);
    }
}
