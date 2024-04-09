using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class CharacterDisplay : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI charName;
    public TextMeshProUGUI introduction;
    void Start()
    {
        UpdateCharacterDisplayInfo(1);
    }
    public void UpdateCharacterDisplayInfo(int id)
    {
        image.sprite=Resources.Load<Sprite>(CharacterDatabase.data[id].image);
        charName.text=CharacterDatabase.data[id].name;
        introduction.text=CharacterDatabase.data[id].introduction;
    }
}
