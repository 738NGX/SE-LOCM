using UnityEngine;
using System.Collections.Generic;

public class HandCards : MonoBehaviour
{
    public List<Card> handCards=new();

    // 向手牌中添加卡牌
    public void AddCardToHand(Card card)
    {
        if(card!=null)
        {
            handCards.Add(card);
        }
    }
    public void DrawCard(List<Card> cards)
    {
        foreach(var card in cards) AddCardToHand(card);
    }
    // 从手牌中移除卡牌
    public void RemoveCard(Card card)
    {
        handCards.Remove(card);
    }
    public void RemoveCard(List<Card> cards)
    {
        foreach(var card in cards) RemoveCard(card);
    }
}
