using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public SceneFader sf;
    public List<CardInShop> cards;
    public DeleteCard deleteCard;
    public RandomBook randomBook;
    public TextMeshProUGUI coins;
    public Text textbox;
    public AudioClip sfxGold;
    public Transform cardSelector;
    public CardDisplay cardDisplay;
    public Transform cardContent;
    private int selectingCardIndex=-1;
    private LocalSaveData localSaveData;

    private void Start()
    {
        localSaveData = LocalSaveDataManager.LoadLocalData();
        var cardGoodsId = GetCardGoodsFromPool(localSaveData.cardsPool);
        randomBook.shop = this;
        deleteCard.shop = this;
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].shop = this;
            cards[i].id = i;
            cards[i].InitId = cardGoodsId[i];
        }
    }

    private void Update()
    {
        coins.text = localSaveData.coins.ToString();
    }

    public void Purchase(int id)
    {
        ShopItem itemToPurchase = GetItemById(id);
        if (itemToPurchase.Price > localSaveData.coins)
        {
            textbox.DOText("嗯......这个你买不起的啦.", 0.1f);
            return;
        }
        localSaveData.coins -= itemToPurchase.Price;
        ProcessPurchase(itemToPurchase);
    }
    private ShopItem GetItemById(int id)
    {
        return id switch
        {
            6 => randomBook,
            7 => deleteCard,
            _ => cards[id],
        };
    }
    private void ProcessPurchase(ShopItem item)
    {
        MainTheme.PlayAudio(sfxGold, gameObject);
        item.gameObject.SetActive(false);

        if (item is CardInShop carditem)
        {
            var card = carditem.GetCard();
            textbox.DOText($"成功购买了卡牌{card.displayInfo.name}的说.", 0.1f);
            localSaveData.AddCardsData(new() { card });
        }
        else if(item is DeleteCard deleteCarditem)
        {
            CallCardDelete();
        }
        else if (item is RandomBook bookitem)
        {
            var book = bookitem.GetBook(localSaveData.ReadBooksData());
            if (book is not null)
            {
                textbox.DOText($"成功抽到了典籍{book.displayInfo.name}的说.", 0.1f);
                localSaveData.AddBooksData(new() { book });
            }
            else
            {
                textbox.DOText($"什么都没有抽到的说.", 0.1f);
            }
        }
    }
    private List<int> GetCardGoodsFromPool(List<int> pool)
    {
        if (pool.Count < 6)
        {
            return null;
        }

        List<int> selected = new();
        List<int> indices = new();

        while (selected.Count < 6)
        {
            int index = Random.Range(0, pool.Count);
            if (!indices.Contains(index))
            {
                indices.Add(index);
                selected.Add(pool[index]);
            }
        }

        return selected;
    }
    private void CallCardDelete()
    {
        List<Card> waitDeleteCards=localSaveData.ReadCardsData();

        if(waitDeleteCards.Count==0)
        {
            textbox.DOText("你没有可以回收的卡牌的说.",0.1f);
            return;
        }
        cardSelector.gameObject.SetActive(true);
        
        for(int i=0;i<waitDeleteCards.Count;i++)
        {
            int currentIndex=i;
            Card card=waitDeleteCards[i];
            
            GameObject obj=Instantiate(cardDisplay.gameObject,cardContent);
            obj.SetActive(true);
            obj.GetComponent<CardDisplay>().UpdateCardDisplayInfo(card.id,card.isPlused);
            obj.GetComponent<CardDisplay>().displayIndex=i;
            obj.AddComponent<Button>().onClick.AddListener(() => ChangeSelectingCardIndex(currentIndex));
        }
    }
    public void ChangeSelectingCardIndex(int index)
    {
        selectingCardIndex=index;
        foreach(Transform child in cardContent)
        {
            if(!child.TryGetComponent<CardDisplay>(out var card)) continue;
            if(card.displayIndex==index&&child.localScale!=new Vector3(1.1f,1.1f))
            {
                child.DOScale(new Vector3(1.1f,1.1f),0.1f);
            }
            else if(card.displayIndex!=index&&child.localScale==new Vector3(1.1f,1.1f))
            {
                child.DOScale(new Vector3(1f,1f),0.1f);
            }
            else continue;
        }
    }
    public void DeleteCard()
    {
        if(selectingCardIndex==-1) return;
        
        int deleteCardId=0;
        bool deleteCardIsPlused=false;
        
        for(int i=0;i<cardContent.childCount;i++)
        {
            if(!cardContent.GetChild(i).gameObject.activeInHierarchy) continue;
            if(!cardContent.GetChild(i).TryGetComponent<CardDisplay>(out var card)) continue;
            if(card.displayIndex!=selectingCardIndex) continue;
            deleteCardId=card.id;
            deleteCardIsPlused=card.isPlused;
            break;
        }

        selectingCardIndex=-1;
        localSaveData.RemoveCardsData(new(){new(deleteCardId,deleteCardIsPlused)});
        cardSelector.gameObject.SetActive(false);

        string cardname=new Card(deleteCardId,deleteCardIsPlused).displayInfo.name;
        textbox.DOText($"成功回收了卡牌{cardname}的说!",0.1f);
    }
    public void EndShop()
    {
        textbox.DOText("感谢惠顾的说!", 0.1f);
        LocalSaveDataManager.SaveLocalData(localSaveData);
        sf.FadeOut(MainTheme.NextScene(localSaveData));
    }
}
