using UnityEngine;
using System.Collections.Generic;

public class DiscardPile : MonoBehaviour
{
    public List<Card> discards=new();           // 存储弃牌堆中的卡牌
    public List<Card> disposedCards=new();      // 存储已经被消耗的卡牌

    // 向弃牌堆中添加卡牌
    public void AddCardToDiscard(Card card)
    {
        discards.Add(card);
    }
    public void AddCardToDisposable(Card card)
    {
        disposedCards.Add(card);
    }
    public void AddCardsToDiscard(List<Card> cards)
    {
        foreach(Card card in cards) AddCardToDiscard(card);
    }
    public void AddCardsToDisposable(List<Card> cards)
    {
        foreach(Card card in cards) AddCardToDisposable(card);
    }
}
