using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardPileViewer : MonoBehaviour
{
    public GameController gc;
    public Transform cardContent;
    public TextMeshProUGUI title;
    public CardDisplay cardDisplay;

    public void OpenPage(int index)
    {
        if(gc.dc.isOpeningPage) return;
        gc.dc.isOpeningPage=true;
        gameObject.SetActive(true);
        List<Card> cards=index switch
        {
            0=>gc.drawPile.Cards,
            1=>gc.discardPile.discards,
            2=>gc.discardPile.disposedCards,
            _=>gc.handCards.Cards,
        };
        title.text=index switch
        {
            0=>"摸牌堆",
            1=>"弃牌堆",
            2=>"消耗牌堆",
            _=>"手牌堆",
        };
        for(int i=0;i<cards.Count;i++)
        {
            GameObject obj=Instantiate(cardDisplay.gameObject,cardContent);
            obj.SetActive(true);
            obj.GetComponent<CardDisplay>().UpdateCardDisplayInfo(cards[i].id,cards[i].isPlused);
        }
    }
    public void ClosePage()
    {
        gc.dc.isOpeningPage=false;
        foreach(Transform child in cardContent)
        {
            if(!child.gameObject.activeInHierarchy) continue;
            Destroy(child.gameObject);
        }
        gameObject.SetActive(false);
    }
}
