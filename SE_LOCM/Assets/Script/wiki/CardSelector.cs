using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardSelector : WikiScrollView
{
    private void Start()
    {
        PopulateList(CardDatabase.data);
    }
}
