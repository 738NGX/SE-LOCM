using UnityEngine;
using System.Collections.Generic;

public class DrawPile : CardPile
{
    public void Init(List<(int,bool)> database)
    {
        foreach(var data in database) AddCard(new Card(data.Item1,data.Item2));
        Shuffle(); // 初始时洗牌
    }
    
    public override void AddCards(List<Card> cards)
    {
        base.AddCards(cards);
        Shuffle();
    }

    // 洗牌方法
    private void Shuffle()
    {
        for (int i=0;i<Cards.Count;i++)
        {
            Card temp=Cards[i];
            int randomIndex=Random.Range(0,Cards.Count);
            Cards[i]=Cards[randomIndex];
            Cards[randomIndex]=temp;
        }
    }

    // 从摸牌堆中摸牌
    private Card DrawCard()
    {
        if(Cards.Count<1) return null;
        Card cardToDraw=Cards[0];
        Cards.RemoveAt(0);
        return cardToDraw;
    }
    public List<Card> DrawCards(int n=1)
    {
        if(n<1) return null;
        List<Card> result=new();
        for(int i=0;i<n;i++) result.Add(DrawCard());
        return result;
    }
}
