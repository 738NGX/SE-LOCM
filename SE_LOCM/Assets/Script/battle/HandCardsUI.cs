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
    readonly float cardWidth = 300f;      // 手牌宽度
    readonly float spacing = 120f;         // 手牌堆叠尺寸
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

    public void ReturnCards()
    {
        Vector3[] path = new Vector3[3];
        path[0] = new Vector3(8f, -4f);
        path[1] = new Vector3(0f, -2f);
        path[2] = new Vector3(-8f, -4f);

        GameObject CardObject = Instantiate(cardTemplateLite, transform);
        CardObject.transform.SetParent(transform, false);
        AudioSource cardTemplateAudio = CardObject.GetComponent<AudioSource>();

        CardObject.transform.position = path[0];
        CardObject.transform.localScale *= 0.5f;
        CardObject.SetActive(true);
        cardTemplateAudio.Play();
        CardObject.transform.DOPath(path, 0.5f, PathType.CatmullRom);
        CardObject.transform.DOScale(0, 0.5f);
    }
    public static void CardDisplayInfoUpdate(GameObject obj, Card card)
    {
        // 为卡牌信息赋值
        Image image = obj.transform.Find("牌面").GetComponent<Image>();
        if (card.type == CardType.Attack)
        {
            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/card_attack.png");
        }
        else if (card.type == CardType.Spell)
        {
            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/card_spell.png");
        }
        else
        {
            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/battle/card_equip.png");
        }
        TextMeshProUGUI cardName = obj.transform.Find("牌名").GetComponent<TextMeshProUGUI>();
        cardName.text = card.displayInfo.name;
        TextMeshProUGUI cardCost = obj.transform.Find("消耗").GetComponent<TextMeshProUGUI>();
        cardCost.text = card.displayInfo.cost;
        TextMeshProUGUI cardType = obj.transform.Find("属性").GetComponent<TextMeshProUGUI>();
        cardType.text = card.displayInfo.type;
        TextMeshProUGUI cardEffect = obj.transform.Find("效果").GetComponent<TextMeshProUGUI>();
        cardEffect.text = card.displayInfo.effect;
        TextMeshProUGUI cardQuote = obj.transform.Find("原文").GetComponent<TextMeshProUGUI>();
        cardQuote.text = card.displayInfo.quote;
    }
    private void CardDisplayIndexUpdate(Transform transform)
    {
        foreach (Transform child in transform)
        {
            if (!child.TryGetComponent<CardTemplate>(out var cardTemplate)) continue;
            if (cardTemplate.inHand) child.SetSiblingIndex(cardTemplate.index);
        }
    }
    public void DrawCards()
    {
        int currentDisplayCards = 0;
        foreach (Transform child in transform)
        {
            if (!child.TryGetComponent<CardTemplate>(out var cardTemplateCount)) continue;
            if (cardTemplateCount.inHand && !cardTemplateCount.isDragging) currentDisplayCards++;
        }
        int k = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.gameObject.TryGetComponent<CardTemplate>(out var cardTemplateComponent)) continue;
            if (!cardTemplateComponent.inHand) continue;

            cardTemplateComponent.index = k;
            CardDisplayInfoUpdate(child.gameObject, hc.Cards[k]);

            float cardPositionX = cardTemplateComponent.index * (cardWidth - spacing) - (hc.Cards.Count - 1) * (cardWidth - spacing) / 2;
            child.gameObject.transform.localPosition = new Vector3(cardPositionX, 0, 0);
            (child.gameObject.transform.position, cardTemplateComponent.originalPosition) = (cardTemplateComponent.originalPosition, child.gameObject.transform.position);

            k++;
        }
        for (int i = currentDisplayCards; i < hc.Cards.Count; i++)
        {
            GameObject CardObject = Instantiate(cardTemplate, transform);
            CardTemplate cardTemplateComponent = CardObject.GetComponent<CardTemplate>();

            cardTemplateComponent.index = i;
            CardDisplayInfoUpdate(CardObject, hc.Cards[i]);
            CardObject.transform.SetParent(transform, false);
            CardObject.SetActive(true);

            cardTemplateComponent.originalScale = new Vector3(1, 1);

            float cardPositionX = i * (cardWidth - spacing) - (hc.Cards.Count - 1) * (cardWidth - spacing) / 2;
            CardObject.transform.localPosition = new Vector3(cardPositionX, 0, 0);
            cardTemplateComponent.originalPosition = CardObject.transform.position;
            CardObject.transform.position = new Vector3(-8f, -4f);
            CardObject.transform.localScale = Vector3.zero;
        }
    }
    public void DisposeNonAttackCards()
    {
        int k = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.TryGetComponent<CardTemplate>(out var cardTemplateComponent)) continue;
            if (!cardTemplateComponent.inHand) continue;

            if (cardTemplateComponent.BindCard.type != CardType.Attack)
            {
                cardTemplateComponent.inHand = false;
                cardTemplateComponent.gameObject.SetActive(false);
            }

            k++;
        }

        RemoveCard();
    }
    public void RemoveCard()
    {
        int k = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.TryGetComponent<CardTemplate>(out var cardTemplateComponent)) continue;
            if (!cardTemplateComponent.inHand) continue;

            cardTemplateComponent.index = k;
            CardDisplayInfoUpdate(child.gameObject, hc.Cards[k]);

            float cardPositionX = cardTemplateComponent.index * (cardWidth - spacing) - (hc.Cards.Count - 1) * (cardWidth - spacing) / 2;
            child.gameObject.transform.localPosition = new Vector3(cardPositionX, 0, 0);
            (child.gameObject.transform.position, cardTemplateComponent.originalPosition) = (cardTemplateComponent.originalPosition, child.gameObject.transform.position);
            child.gameObject.transform.DOMove(cardTemplateComponent.originalPosition, 0.1f);

            k++;
        }
    }
    public void CardsDisplayInfoUpdate()
    {
        int k = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.TryGetComponent<CardTemplate>(out var cardTemplateComponent)) continue;
            if (!cardTemplateComponent.inHand) continue;
            CardDisplayInfoUpdate(child.gameObject, hc.Cards[k]);
            k++;
        }
    }
    public IEnumerator CardDisplayUpdate()
    {
        foreach (Transform child in transform)
        {
            if (!child.TryGetComponent<CardTemplate>(out var cardTemplateComponent)) continue;
            if (cardTemplateComponent.inHand && !cardTemplateComponent.isDragging)
            {
                if (child.position == new Vector3(-8f, -4f))
                {
                    child.GetComponent<AudioSource>().Play();
                    child.DOScale(cardTemplateComponent.originalScale, 0.25f);
                    child.DOMove(cardTemplateComponent.originalPosition, 0.25f);
                    yield return new WaitForSeconds(0.15f);
                }
                else
                {
                    child.DOMove(cardTemplateComponent.originalPosition, 0.25f);
                }
            }
        }
        CardDisplayIndexUpdate(transform);
    }
    public IEnumerator DisCards()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.transform.DOMove(new Vector3(8f, -4f), 0.25f);
            child.gameObject.transform.DOScale(0, 0.25f);
        }
        yield return new WaitForSeconds(0.25f);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}