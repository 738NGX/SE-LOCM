using UnityEngine;
using System.Collections.Generic;

public class HandCards : CardPile
{
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
}
