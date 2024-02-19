using UnityEngine;
using System.Collections.Generic;

public class DiscardPile : MonoBehaviour
{
    public List<Card> discardPile=new();        // 存储弃牌堆中的卡牌
    public List<Card> disposablePile=new();     // 存储已经被消耗的卡牌

    // 向弃牌堆中添加卡牌
    public void AddCardToDiscard(Card card)
    {
        discardPile.Add(card);
    }
    public void AddCardToDisposable(Card card)
    {
        disposablePile.Add(card);
    }
    public void AddCardToDiscard(List<Card> cards)
    {
        foreach(Card card in cards) AddCardToDiscard(card);
    }
    public void AddCardToDisposable(List<Card> cards)
    {
        foreach(Card card in cards) AddCardToDisposable(card);
    }
    public Card Top(){return discardPile[^1];}
}
