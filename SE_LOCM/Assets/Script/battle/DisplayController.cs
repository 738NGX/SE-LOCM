using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;

public class DisplayController : MonoBehaviour
{
    public GameController gc;
    public bool isAnimating=false;
    public bool isOpeningPage=false;
    public Transform hud;
    public TMP_FontAsset usingFont;
    public Transform higherCanvas;
    public TextMeshProUGUI drawPileUI;
    public TextMeshProUGUI discardPileUI;
    public TextMeshProUGUI disposablePileUI;
    public GameObject playerObject;
    public List<GameObject> enemyObjects;
    public TextMeshProUGUI HPUI;
    public TextMeshProUGUI SPUI;
    public TextMeshProUGUI playerAttackAddon;
    public TextMeshProUGUI playerDefenceAddon;
    public Image playerHPSlider;
    public TextMeshProUGUI playerShield;
    public List<TextMeshProUGUI> EnemyHPUI;
    public List<Image> EnemyHPSlider;
    public List<TextMeshProUGUI> EnemyShield;
    public List<TextMeshProUGUI> EnemyAttackAddon;
    public List<TextMeshProUGUI> EnemyDefenceAddon;
    public List<Image> EnemyIntend;
    public List<TextMeshProUGUI> EnemyIntendVal;
    public GameObject bezierArrow;
    public GameObject shield;
    public GameObject upArrow;
    public GameObject downArrow;
    
    private GameObject panel;
    private readonly float textWidth=100f;

    private void Update()
    {
        drawPileUI.text=gc.drawPile.Cards.Count.ToString();
        discardPileUI.text=gc.discardPile.discards.Count.ToString();
        disposablePileUI.text=gc.discardPile.disposedCards.Count.ToString();
        HPUI.text=gc.player.hp+"/"+gc.player.hpLimit;
        hud.Find("heart").GetComponentInChildren<TextMeshProUGUI>().text=gc.player.hp+"/"+gc.player.hpLimit;
        hud.Find("coin").GetComponentInChildren<TextMeshProUGUI>().text=gc.player.coins.ToString();
        hud.Find("round").GetComponentInChildren<TextMeshProUGUI>().text=gc.roundCount.ToString();
        SPUI.text=gc.player.sp+"/"+gc.player.spInit;
        playerAttackAddon.text=(gc.player.ap<0 ? "" : "+")+gc.player.ap+"\nx"+gc.player.buffContainer.AttackRate;
        playerDefenceAddon.text=(gc.player.dp<0 ? "" : "+")+gc.player.dp+"\nx"+gc.player.buffContainer.DefenceRate;
        playerShield.text=gc.player.shield.ToString();
        for(int i=0;i<gc.enemies.Count;i++)
        {
            EnemyHPUI[i].text=gc.enemies[i].hp+"/"+gc.enemies[i].hpLimit;
            EnemyShield[i].text=gc.enemies[i].shield.ToString();
            EnemyAttackAddon[i].text=(gc.enemies[i].ap<0 ? "" : "+")+gc.enemies[i].ap+"\nx"+gc.enemies[i].buffContainer.AttackRate;
            EnemyDefenceAddon[i].text=(gc.enemies[i].dp<0 ? "" : "+")+gc.enemies[i].dp+"\nx"+gc.enemies[i].buffContainer.DefenceRate;
            
            EnemyIntend[i].sprite=gc.enemies[i].intendType switch
            {
                IntendType.Attack => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/intend_attack.png"),
                IntendType.MAttack => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/intend_attack.png"),
                IntendType.HAttack => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/intend_attack.png"),
                IntendType.Defence => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/intend_defence.png"),
                IntendType.Buff => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/intend_buff.png"),
                IntendType.Debuff => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/intend_debuff.png"),
                IntendType.Recover => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/intend_recover.png"),
                IntendType.ADefence => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/intend_adefence.png"),
                IntendType.ABuff => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/intend_abuff.png"),
                IntendType.ADebuff => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/intend_adebuff.png"),
                IntendType.ARecover => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/intend_arecover.png"),
                IntendType.Sleep => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/intend_sleep.png"),
                _ => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/intend_unknown.png"),
            };
            EnemyIntendVal[i].text=gc.enemies[i].intendValue<0?"":gc.enemies[i].intendValue.ToString()+(gc.enemies[i].intendTimes<2?"":"x"+gc.enemies[i].intendTimes);
        }
    }
    public void UpdateHPSlider(int i)
    {
        if(i==-1)
        {
            DOTween.To(()=>playerHPSlider.fillAmount,x=>playerHPSlider.fillAmount=x,(float)gc.player.hp/gc.player.hpLimit,0.25f);
        }
        else DOTween.To(()=>EnemyHPSlider[i].fillAmount,x=>EnemyHPSlider[i].fillAmount=x,(float)gc.enemies[i].hp/gc.enemies[i].hpLimit,0.25f);
    }
    public IEnumerator AnimatePanelAndText(List<string> texts,float time=0.5f)
    {
        isAnimating=true;

        panel=CreatePanel(higherCanvas,higherCanvas.GetComponent<RectTransform>().sizeDelta.x,300);
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

        yield return new WaitForSeconds(time);
        yield return new WaitUntil(()=>gc.waitingDiscardCount==0);

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
    private GameObject CreateText(Transform parent, string textContent, float startX, int index, float offset)
    {
        GameObject textObject=new("Text");
        textObject.transform.SetParent(parent, false);
        TextMeshProUGUI text=textObject.AddComponent<TextMeshProUGUI>();
        text.text=textContent;
        text.font=usingFont;
        text.fontSize=120;
        text.alignment=TextAlignmentOptions.Center;
        RectTransform rectTransform=textObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta=new Vector2(textWidth,30);
        rectTransform.anchorMin=new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax=new Vector2(0.5f, 0.5f);
        rectTransform.pivot=new Vector2(0.5f, 0.5f);

        float positionX=startX+index*(textWidth+offset);
        rectTransform.anchoredPosition=new Vector2(positionX,0);

        return textObject;
    }
}
