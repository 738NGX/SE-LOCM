using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BookSelector : WikiScrollView
{
    private void Start()
    {
        PopulateList(BookDatabase.data);
    }
}
