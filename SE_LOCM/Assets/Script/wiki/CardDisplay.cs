using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    private int id;

    private void Start()
    {
        id=100;
        UpdateCardDisplayInfo(id);
    }
    public void UpdateCardDisplayInfo(int id)
    {
        HandCardsUI.CardDisplayInfoUpdate(gameObject,new Card(id));
    }
}
