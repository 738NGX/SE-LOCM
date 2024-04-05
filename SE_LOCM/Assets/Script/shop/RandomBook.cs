using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RandomBook : ShopItem
{
    protected override void Start()
    {
        base.Start();
        id = 6;
        Price = 200 / countRate;
        gameObject.GetComponent<Button>().onClick.AddListener(() => shop.Purchase(6));
    }

    public Book GetBook(List<Book> achievedBooks)
    {
        if(Random.Range(0,100)<20)
        {
            int seed=Random.Range(10,21);
            return achievedBooks.Contains(new Book(seed)) ? null : new Book(seed);
        }
        else
        {
            int seed=Random.Range(21,35);
            return achievedBooks.Contains(new Book(seed)) ? null : new Book(seed);
        }
    }
}
