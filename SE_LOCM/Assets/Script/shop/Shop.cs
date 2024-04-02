using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public LocalSaveData localSaveData;
    public List<CardInShop> cards;
    public DeleteCard deleteCard;

    private void Start()
    {

    }

    void Update()
    {

    }

    private List<int> GetCardGoodsFromPool(List<int> pool)
    {
        if (pool.Count < 6)
        {
            Debug.LogError("The list must contain at least six elements.");
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
}
