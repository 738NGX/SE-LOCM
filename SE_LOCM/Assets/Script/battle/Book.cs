using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book
{
    public int id;
    public BookRarity rarity;
    public Book(int id)
    {
        this.id=id;
        rarity=BookDatabase.data[id].rarity switch
        {
            "普通"=>BookRarity.Ordinary,
            "稀有"=>BookRarity.Rare,
            "限定"=>BookRarity.Epic,
            _=>BookRarity.Ordinary
        };
    }
}
