using UnityEngine;
using UnityEngine.UI;

public class CardInShop : ShopItem
{
    public CardDisplay cardDisplay;
    private bool inited = false;
    public int InitId { get; set; } = 0;
    protected override void Start()
    {
        base.Start();
        cardDisplay = gameObject.GetComponent<CardDisplay>();
        gameObject.AddComponent<Button>().onClick.AddListener(() => shop.Purchase(id));
        inited = true;
    }
    protected override void Update()
    {
        base.Update();
        if (inited && InitId != 0)
        {
            UpdateCardGoodInfo(InitId);
            inited = false;
        }
    }
    public Card GetCard()
    {
        return new Card(cardDisplay.id);
    }
    public void UpdateCardGoodInfo(int id)
    {
        cardDisplay.UpdateCardDisplayInfo(id);
        Price = new Card(id).rarity switch
        {
            CardRarity.Ordinary => Random.Range(50, 100) / countRate,
            CardRarity.Rare => Random.Range(100, 200) / countRate,
            CardRarity.Epic => Random.Range(200, 300) / countRate,
            _ => 0
        };
    }
}
