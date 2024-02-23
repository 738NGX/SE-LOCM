using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RewardType{Coins,Card,Book,Continue}
public class RewardItem : MonoBehaviour
{
    public Reward reward;
    public RewardType type;
    public AudioClip sfx;
    public void RewardClicked()
    {
        MainTheme.PlayAudio(sfx,transform.parent.gameObject);
        gameObject.SetActive(false);
        switch(type)
        {
            case RewardType.Coins: reward.RewardCoins(); break;
            case RewardType.Card: reward.WaitRewardCard(); break;
            case RewardType.Book: reward.RewardBook(); break;
            case RewardType.Continue: reward.Continue(); break;
            default: break;
        }
    }
}
