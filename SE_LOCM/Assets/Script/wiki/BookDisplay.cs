using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class BookDisplay : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI bookName;
    public TextMeshProUGUI effect;
    public TextMeshProUGUI introduction;
    void Start()
    {
        UpdateBookDisplayInfo(0);
    }
    public void UpdateBookDisplayInfo(int id)
    {
        image.sprite=Resources.Load<Sprite>("UI/books/"+id.ToString("D3"));
        bookName.text=BookDatabase.data[id].name;
        effect.text=BookDatabase.data[id].effect;
        introduction.text=BookDatabase.data[id].introduction;
    }
}
