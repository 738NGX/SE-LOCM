using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class DisplayController : MonoBehaviour
{
    public GameController gc;
    public TextMeshProUGUI drawPileUI;
    public TextMeshProUGUI discardPileUI;
    public TextMeshProUGUI HPUI;
    public TextMeshProUGUI SPUI;
    public Transform canvasTransform;
    public TMP_FontAsset usingFont;
    public bool isAnimating=false;

    private GameObject panel;
    private readonly float textWidth=100f;

    void Update()
    {
        drawPileUI.text=gc.drawPile.drawPile.Count.ToString();
        discardPileUI.text=gc.discardPile.discardPile.Count.ToString();
        HPUI.text="HP: "+gc.player.hp+"/"+gc.player.hp_limit;
        SPUI.text=gc.player.sp+"/"+gc.player.sp_init;
    }
    public IEnumerator AnimatePanelAndText(List<string> texts)
    {
        isAnimating=true;

        panel=CreatePanel(canvasTransform,canvasTransform.GetComponent<RectTransform>().sizeDelta.x,300);
        panel.transform.DOScaleX(0,0.5f).From();

        float textHorizontalOffset=100f;
        int textCount=texts.Count;
        float totalWidth=textWidth*textCount+textHorizontalOffset*(textCount-1);
        float startX=-totalWidth/2+textWidth/2;
        for(int i=0;i<textCount;i++)
        {
            CreateText(panel.transform,texts[i],startX,i,textHorizontalOffset);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);

        foreach (Transform child in panel.transform)
        {
            child.gameObject.SetActive(false);
            //yield return new WaitForSeconds(0.1f);
        }

        panel.transform.DOScaleX(0,0.25f);
        yield return new WaitForSeconds(0.25f);
        Destroy(panel);
        isAnimating=false;
    }
    public GameObject CreatePanel(Transform parent,float width,float height)
    {
        GameObject panel=new("Panel");
        panel.transform.SetParent(parent, false);
        Image panelImage=panel.AddComponent<Image>();
        panelImage.color=new Color(0,0,0,0.5f);
        
        RectTransform rectTransform=panel.GetComponent<RectTransform>();
        rectTransform.anchorMin=new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax=new Vector2(0.5f, 0.5f);
        rectTransform.pivot=new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta=new Vector2(width, height);
        
        return panel;
    }
    GameObject CreateText(Transform parent, string textContent, float startX, int index, float offset)
    {
        GameObject textObject=new("Text");
        textObject.transform.SetParent(parent, false);
        TextMeshProUGUI text=textObject.AddComponent<TextMeshProUGUI>();
        text.text=textContent;
        text.font=usingFont;
        text.fontSize=120;
        text.alignment=TextAlignmentOptions.Center;
        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(textWidth,30);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        float positionX=startX+index*(textWidth+offset);
        rectTransform.anchoredPosition=new Vector2(positionX,0);

        return textObject;
    }
}
