using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum WikiStatus{Menu,Card,Book};

public class Wiki : MonoBehaviour
{
    public MainTheme mt;
    public WikiStatus wikiStatus=WikiStatus.Menu;
    public Transform buttons;
    public Transform cardwiki;
    public Transform bookwiki;
    public CardDisplay cardDisplay;
    public BookDisplay bookDisplay;

    private void Start()
    {
        cardwiki.gameObject.SetActive(true);
        cardwiki.position=new Vector3(0,-12);
        bookwiki.gameObject.SetActive(true);
        bookwiki.position=new Vector3(0,-12);
    }

    public void ChangeWikiStatus(WikiStatus wikiStatus)
    {
        switch(this.wikiStatus)
        {
            case WikiStatus.Menu: buttons.DOMove(new Vector3(buttons.position.x+5,buttons.position.y),1f); break;
            case WikiStatus.Card: cardwiki.DOMove(new Vector3(0,-12),1f); break;
            case WikiStatus.Book: bookwiki.DOMove(new Vector3(0,-12),1f); break;
        }
        this.wikiStatus=wikiStatus;
        switch(wikiStatus)
        {
            case WikiStatus.Menu: buttons.DOMove(new Vector3(buttons.position.x-5,buttons.position.y),1f); break;
            case WikiStatus.Card: cardwiki.DOMove(Vector3.zero,1f); break;
            case WikiStatus.Book: bookwiki.DOMove(Vector3.zero,1f); break;
        }
    }
    public void ChangeWikiStatus(int wikiStatus)
    {
        switch(wikiStatus)
        {
            case 0: ChangeWikiStatus(WikiStatus.Menu); break;
            case 1: ChangeWikiStatus(WikiStatus.Card); break;
            case 2: ChangeWikiStatus(WikiStatus.Book); break;
        }
    }
    public void UpdateCardDisplayInfo(int id)
    {
        cardDisplay.UpdateCardDisplayInfo(id);
    }
    public void UpdateBookDisplayInfo(int id)
    {
        bookDisplay.UpdateBookDisplayInfo(id);
    }
}
