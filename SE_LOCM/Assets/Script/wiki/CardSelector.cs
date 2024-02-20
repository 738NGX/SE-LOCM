using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardSelector : MonoBehaviour
{
    public GameObject cardItemPrefab;
    public Transform contentPanel;
    public RectTransform contentTransform;
    public Wiki wiki;
    public float itemHeight = 100f;
    public float spacing = 10f;
    public int itemCount=0;

    private void Start()
    {
        PopulateList();
    }
    private void PopulateList()
    {
        foreach (var card in CardDatabase.data.Values)
        {
            GameObject newItem=Instantiate(cardItemPrefab,contentPanel);
            newItem.SetActive(true);
            newItem.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text=card.name;
            if (newItem.TryGetComponent<Button>(out var button))
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(()=>wiki.UpdateCardDisplayInfo(card.id));
            }
            itemCount++;
        }
    }
    private void Update()
    {
        AdjustHeight();
    }

    private void AdjustHeight()
    {
        float totalHeight=itemCount*itemHeight+(itemCount-1)*spacing;
        contentTransform.sizeDelta=new Vector2(contentTransform.sizeDelta.x,totalHeight);
    }

}
