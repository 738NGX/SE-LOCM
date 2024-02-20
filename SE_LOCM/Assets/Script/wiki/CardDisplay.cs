using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    private void Start()
    {
        UpdateCardDisplayInfo(100);
    }
    public void UpdateCardDisplayInfo(int id)
    {
        HandCardsUI.CardDisplayInfoUpdate(gameObject,new Card(id));
    }
}
