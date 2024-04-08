using UnityEngine;
using System.Collections.Generic;

public class HandCards : CardPile
{
    public GameController gc;
    // 向手牌中添加卡牌
    public override void AddCards(List<Card> cards)
    {
        base.AddCards(cards);
    }
    // 从手牌中移除卡牌
    public override void RemoveCards(List<Card> cards)
    {
        base.RemoveCards(cards);
    }
    public void AddExtraCards(List<Card> cards)
    {
        int overflow = cards.Count + Count - 10;

        List<Card> overflowCards = new();
        if (overflow > 0)
        {
            overflowCards = cards.GetRange(cards.Count - overflow, overflow);
            cards.RemoveRange(cards.Count - overflow, overflow);
            gc.discardPile.AddCardsToDiscard(overflowCards);
        }
        AddCards(cards);

        gc.hcui.DrawCards();
        StartCoroutine(gc.hcui.CardDisplayUpdate());
    }
    public int DisposeNonAttackCards()
    {
        gc.hcui.DisposeNonAttackCards();
        var list=cardList.DisposeNonAttackCards();
        gc.discardPile.AddCardsToDisposable(list);
        return list.Count;
    }
}
