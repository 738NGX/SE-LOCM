using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : WikiScrollView
{
    private void Start()
    {
        PopulateList(CharacterDatabase.data);
    }
}
