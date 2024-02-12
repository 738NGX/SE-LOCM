using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEditor;

public class HandCardsUI : MonoBehaviour
{
    public GameObject cardTemplate;     // 手牌模板
    public HandCards hc;                // 手牌堆
    readonly float cardWidth=300f;      // 手牌宽度
    readonly float spacing=75f;         // 手牌堆叠尺寸

    public void DrawCards()
    {
        StartCoroutine(DrawCardsCoroutine());
    }
    IEnumerator DrawCardsCoroutine()
    {
        // 清除旧的手牌显示
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        yield return null;

        // 为每张手牌创建一个新的实例
        for(int i=0;i<hc.handCards.Count;i++)
        {
            // 从模板创建新卡牌
            GameObject CardObject=Instantiate(cardTemplate,transform);

            float cardPositionX=i*(cardWidth-spacing)-(hc.handCards.Count-1)*(cardWidth-spacing)/2;
            CardObject.transform.localPosition=new Vector3(cardPositionX,0,0);
            CardTemplate cardTemplateComponent=CardObject.GetComponent<CardTemplate>();
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

            Sequence s=DOTween.Sequence();
            s.Append(CardObject.transform.DOMove(new Vector3(-8f,-4f),0.5f).From());
            s.Join(CardObject.transform.DOScale(0,0.5f).From());
            s.Play();

            yield return new WaitForSeconds(0.2f);
        }
    }
}