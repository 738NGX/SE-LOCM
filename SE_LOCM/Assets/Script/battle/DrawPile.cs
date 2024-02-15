using UnityEngine;
using System.Collections.Generic;

public class DrawPile : MonoBehaviour
{
    public List<Card> drawPile;

    void Start()
    {
        drawPile=new()
        {
            /*
            new Card(100),new Card(101),new Card(102),new Card(103),new Card(104),
            new Card(105),new Card(106),new Card(107),new Card(108),new Card(109),
            new Card(110),new Card(111),new Card(112),new Card(113),new Card(114),
            new Card(115),new Card(116),new Card(117),new Card(118),new Card(119),
            new Card(120),new Card(121)*/
            
            new Card(100),new Card(103),new Card(104),
            new Card(101),new Card(101),new Card(101),new Card(101),
            new Card(102),new Card(102),new Card(102),new Card(102),
            /*
            new Card(103),new Card(101),
            new Card(101),new Card(101),new Card(101),new Card(101),
            new Card(101),new Card(101),new Card(101),new Card(101),
            */
        };
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
