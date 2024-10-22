using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public int id;
    public bool isPlused;
    public int displayIndex;
    private bool idSet = false;

    private void Start()
    {
        if (!idSet)
        {
            UpdateCardDisplayInfo(100);
        }
    }

    public void UpdateCardDisplayInfo(int id, bool isPlused = false)
    {
        this.id = id;
        this.isPlused = isPlused;
        idSet = true;
        HandCardsUI.CardDisplayInfoUpdate(gameObject, new Card(id, isPlused));
    }

    public void UpgradeExchange()
    {
        UpdateCardDisplayInfo(id,!isPlused);
    }
}
