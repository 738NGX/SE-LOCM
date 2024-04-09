using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class WikiScrollView : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform contentPanel;
    public RectTransform contentTransform;
    public Wiki wiki;
    public float itemHeight = 100f;
    public float spacing = 10f;
    public int itemCount=0;

    private void Update()
    {
        AdjustHeight();
    }
    private void AdjustHeight()
    {
        float totalHeight=itemCount*itemHeight+(itemCount-1)*spacing;
        contentTransform.sizeDelta=new Vector2(contentTransform.sizeDelta.x,totalHeight);
    }
    public void PopulateList<T>(Dictionary<int,T> dict) where T : DisplayInfo
    {
        foreach (var item in dict.Values)
        {
            GameObject newItem=Instantiate(itemPrefab,contentPanel);
            newItem.SetActive(true);
            newItem.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text=item.name;
            if(newItem.TryGetComponent<Button>(out var button))
            {
                button.onClick.RemoveAllListeners();
                switch (item)
                {
                    case CardDisplayInfo cardInfo:
                        button.onClick.AddListener(() => wiki.UpdateCardDisplayInfo(cardInfo.id));
                        break;
                    case BookDisplayInfo bookInfo:
                        button.onClick.AddListener(() => wiki.UpdateBookDisplayInfo(bookInfo.id));
                        break;
                    case CharacterInfo charInfo:
                        button.onClick.AddListener(() => wiki.UpdateCharacterDisplayInfo(charInfo.id));
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported DisplayInfo type");
                }
            }
            itemCount++;
        }
    }
}
