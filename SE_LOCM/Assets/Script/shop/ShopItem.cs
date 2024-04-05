using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public int id;
    public Shop shop;
    public int Price { get; protected set; } = 0;
    public bool IsPurchased { get; private set; } = false;
    public TextMeshProUGUI priceText;
    protected int countRate;

    protected virtual void Start()
    {
        priceText = transform.Find("价格").GetComponent<TextMeshProUGUI>();
        countRate = Random.Range(0, 10) switch
        {
            0 => 2,
            _ => 1,
        };
    }
    protected virtual void Update()
    {
        priceText.text = Price.ToString();
    }
}
