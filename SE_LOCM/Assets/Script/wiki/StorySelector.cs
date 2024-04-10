using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StorySelector : WikiScrollView
{
    private void Start()
    {
        var nameDict=MapNodeIdToStorySceneDatabase.nameData;
        var pathDict=MapNodeIdToStorySceneDatabase.pathData;
        foreach (var key in nameDict.Keys)
        {
            GameObject newItem=Instantiate(itemPrefab,contentPanel);
            newItem.SetActive(true);
            newItem.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text=nameDict[key];
            if(newItem.TryGetComponent<Button>(out var button))
            {
                button.onClick.RemoveAllListeners();
                var path=pathDict[key];
                button.onClick.AddListener(() => wiki.mt.sf.FadeOut(path));     
            }
            itemCount++;
        }
    }
}
