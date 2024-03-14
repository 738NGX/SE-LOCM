using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using Fungus;

public class BookViewer : MonoBehaviour
{
    public GameController gc;
    public Transform content;
    public GameObject itemPrefab;
    public Image image;
    public TextMeshProUGUI bookName;
    public TextMeshProUGUI effect;
    public TextMeshProUGUI introduction;
    public Button bookButton;
    private bool inited=false;
    private bool instructionOn=false;
    private void Init()
    {
        var books=gc.player.books;
        foreach(var book in books)
        {
            GameObject newItem=Instantiate(itemPrefab,content);
            newItem.SetActive(true);
            newItem.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text=BookDatabase.data[book].name;
            if(newItem.TryGetComponent<Button>(out var button))
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => UpdateBookDisplayInfo(book));
            }
        }
        if(books.Count>0) UpdateBookDisplayInfo(books[0]);
    }
    public void OpenPage()
    {
        if(gc.dc.isAnimating||gc.dc.isOpeningPage) return;
        gc.dc.isOpeningPage=true;
        gameObject.SetActive(true);
        if(!inited)
        {
            Init();
            inited=true;
        }
    }
    public void ClosePage()
    {
        gc.dc.isOpeningPage=false;
        gameObject.SetActive(false);
    }
    public void UpdateBookDisplayInfo(int id)
    {
        image.sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/books/"+id.ToString("D3")+".png");
        bookName.text=BookDatabase.data[id].name;
        effect.text=BookDatabase.data[id].effect;
        introduction.text=BookDatabase.data[id].introduction;
    }
    public void DisplayInstruction()
    {
        var textObj=bookButton.transform.Find("Text (TMP)").gameObject;
        if(instructionOn) textObj.SetActive(false);
        else textObj.SetActive(true);
        instructionOn=!instructionOn;
    }
}
