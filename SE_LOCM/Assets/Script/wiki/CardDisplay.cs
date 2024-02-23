using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public int id;
    private bool idSet=false;

    private void Start()
    {
        if (!idSet)
        {
            UpdateCardDisplayInfo(100);
        }
    }

    public void UpdateCardDisplayInfo(int id)
    {
        this.id=id;
        idSet=true;
        HandCardsUI.CardDisplayInfoUpdate(gameObject,new Card(id));
    }
}
