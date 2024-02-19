using UnityEngine;
using System.Collections.Generic;

public class DrawPile : MonoBehaviour
{
    public List<Card> drawPile=new(){};

    public void Init(List<(int,bool)> database)
    {
        foreach(var data in database) drawPile.Add(new Card(data.Item1,data.Item2));
        Shuffle(); // 初始时洗牌
    }
    
    public void AddCardToDraw(Card card)
    {
        drawPile.Add(card);
    }
    public void AddCardToDraw(List<Card> cards)
    {
        foreach(Card card in cards) AddCardToDraw(card);
        Shuffle();
    }

    // 洗牌方法
    public void Shuffle()
    {
        for (int i=0;i<drawPile.Count;i++)
        {
            Card temp=drawPile[i];
            int randomIndex=Random.Range(0,drawPile.Count);
            drawPile[i]=drawPile[randomIndex];
            drawPile[randomIndex]=temp;
        }
    }
    // 从摸牌堆中摸牌
    private Card DrawCard()
    {
        if(drawPile.Count<1) return null;
        Card cardToDraw=drawPile[0];
        drawPile.RemoveAt(0);
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
