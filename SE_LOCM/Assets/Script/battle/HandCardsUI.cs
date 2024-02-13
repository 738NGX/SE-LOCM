using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEditor;

public class HandCardsUI : MonoBehaviour
{
    public GameObject cardTemplate;     // 手牌模板
    public GameObject cardTemplateLite; // 手牌模板(没有附加类型和碰撞箱)
    public HandCards hc;                // 手牌堆
    readonly float cardWidth=300f;      // 手牌宽度
    readonly float spacing=75f;         // 手牌堆叠尺寸
    public Rect playArea;

    /*
    void OnGUI() 
    {
        // 测试playarea
        Rect guiRect = new Rect(playArea.x, Screen.height - playArea.y - playArea.height, playArea.width, playArea.height);
        GUI.color = new Color(0, 0, 0, 0.5f);
        GUI.DrawTexture(guiRect, Texture2D.whiteTexture);
        GUI.color = Color.white;
    }
    */
    
    public IEnumerator ReturnCards()
    {
        Vector3[] path=new Vector3[3];
        path[0]=new Vector3(8f,-4f);
        path[1]=new Vector3(0f,-2f);
        path[2]=new Vector3(-8f,-4f);

        GameObject CardObject=Instantiate(cardTemplateLite,transform);
        CardObject.transform.SetParent(transform,false);
        AudioSource cardTemplateAudio=CardObject.GetComponent<AudioSource>();
        
        CardObject.transform.position=path[0];
        CardObject.transform.localScale*=0.5f;
        CardObject.SetActive(true);
        cardTemplateAudio.Play();
        CardObject.transform.DOPath(path,0.5f,PathType.CatmullRom);
        CardObject.transform.DOScale(0,0.5f);
        yield return new WaitForSeconds(0.5f);
        Destroy(CardObject);
    }
    public void DrawCards()
    {
        StartCoroutine(DrawCardsCoroutine());
    }
    private IEnumerator DrawCardsCoroutine()
    {
        // 为每张手牌创建一个新的实例
        for(int i=0;i<hc.handCards.Count;i++)
        {
            // 从模板创建新卡牌
            GameObject CardObject=Instantiate(cardTemplate,transform);
            CardTemplate cardTemplateComponent=CardObject.GetComponent<CardTemplate>();
            AudioSource cardTemplateAudio=CardObject.GetComponent<AudioSource>();

            cardTemplateComponent.index=i;
            float cardPositionX=i*(cardWidth-spacing)-(hc.handCards.Count-1)*(cardWidth-spacing)/2;
            CardObject.transform.localPosition=new Vector3(cardPositionX,0,0);
            cardTemplateComponent.originalPosition=CardObject.transform.position;
            cardTemplateComponent.originalScale=new Vector3(1,1);

            // 为卡牌信息赋值
            Image image=CardObject.transform.Find("牌面").GetComponent<Image>();
            if(hc.handCards[i].type==CardType.Attack)
            {
                image.sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/card_attack.png");
            }
            else if(hc.handCards[i].type==CardType.Spell)
            {
                image.sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/card_spell.png");
            }
            else
            {
                image.sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/card_equip.png");
            }
            TextMeshProUGUI cardName=CardObject.transform.Find("牌名").GetComponent<TextMeshProUGUI>();
            cardName.text=hc.handCards[i].displayInfo.name;
            TextMeshProUGUI cardCost=CardObject.transform.Find("消耗").GetComponent<TextMeshProUGUI>();
            cardCost.text=hc.handCards[i].displayInfo.cost;
            TextMeshProUGUI cardType=CardObject.transform.Find("属性").GetComponent<TextMeshProUGUI>();
            cardType.text=hc.handCards[i].displayInfo.type;
            TextMeshProUGUI cardEffect=CardObject.transform.Find("效果").GetComponent<TextMeshProUGUI>();
            cardEffect.text=hc.handCards[i].displayInfo.effect;
            TextMeshProUGUI cardQuote=CardObject.transform.Find("原文").GetComponent<TextMeshProUGUI>();
            cardQuote.text=hc.handCards[i].displayInfo.quote;

            // 设置新卡牌是手牌堆的子对象
            CardObject.transform.SetParent(transform, false);
            CardObject.SetActive(true);

            CardObject.transform.DOMove(new Vector3(-8f,-4f),0.5f).From();
            CardObject.transform.DOScale(0,0.5f).From();
            cardTemplateAudio.Play();


            yield return new WaitForSeconds(0.2f);
        }
    }
    public void RemoveCard()
    {
        int i=0;
        foreach(Transform child in transform)
        {
            if(!child.gameObject.TryGetComponent<CardTemplate>(out var cardTemplateComponent)) continue;
            cardTemplateComponent.index=i;
            Vector3 lastPosition=cardTemplateComponent.originalPosition;

            // 为卡牌信息赋值
            Image image=child.gameObject.transform.Find("牌面").GetComponent<Image>();
            if(hc.handCards[i].type==CardType.Attack)
            {
                image.sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/card_attack.png");
            }
            else if(hc.handCards[i].type==CardType.Spell)
            {
                image.sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/card_spell.png");
            }
            else
            {
                image.sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/card_equip.png");
            }
            TextMeshProUGUI cardName=child.gameObject.transform.Find("牌名").GetComponent<TextMeshProUGUI>();
            cardName.text=hc.handCards[i].displayInfo.name;
            TextMeshProUGUI cardCost=child.gameObject.transform.Find("消耗").GetComponent<TextMeshProUGUI>();
            cardCost.text=hc.handCards[i].displayInfo.cost;
            TextMeshProUGUI cardType=child.gameObject.transform.Find("属性").GetComponent<TextMeshProUGUI>();
            cardType.text=hc.handCards[i].displayInfo.type;
            TextMeshProUGUI cardEffect=child.gameObject.transform.Find("效果").GetComponent<TextMeshProUGUI>();
            cardEffect.text=hc.handCards[i].displayInfo.effect;
            TextMeshProUGUI cardQuote=child.gameObject.transform.Find("原文").GetComponent<TextMeshProUGUI>();
            cardQuote.text=hc.handCards[i].displayInfo.quote;
            
            float cardPositionX=i*(cardWidth-spacing)-(hc.handCards.Count-1)*(cardWidth-spacing)/2;
            child.gameObject.transform.localPosition=new Vector3(cardPositionX,0,0);
            cardTemplateComponent.originalPosition=child.gameObject.transform.position;
            child.gameObject.transform.position=lastPosition;
            child.gameObject.transform.DOMove(cardTemplateComponent.originalPosition,0.1f);

            i++;
        }
    }
    public IEnumerator DisCards()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.transform.DOMove(new Vector3(8f,-4f),0.5f);
            child.gameObject.transform.DOScale(0,0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}