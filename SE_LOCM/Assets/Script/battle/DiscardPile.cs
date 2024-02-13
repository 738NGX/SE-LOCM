using UnityEngine;
using System.Collections.Generic;

public class DiscardPile : MonoBehaviour
{
    public List<Card> discardPile = new List<Card>(); // 存储弃牌堆中的卡牌

    // 向弃牌堆中添加卡牌
    public void AddCard(Card card)
    {
        discardPile.Add(card);
    }

    // 可以添加更多管理弃牌堆的方法，如将弃牌堆的卡牌重新洗入手牌堆等
}
