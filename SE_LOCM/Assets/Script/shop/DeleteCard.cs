using UnityEngine.UI;

public class DeleteCard : ShopItem
{
    protected override void Start()
    {
        base.Start();
        id = 7;
        Price = 50 / countRate;
        gameObject.GetComponent<Button>().onClick.AddListener(() => shop.Purchase(7));
    }
}
