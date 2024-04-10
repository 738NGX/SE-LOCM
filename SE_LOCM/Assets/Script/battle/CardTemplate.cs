using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CardTemplate : MonoBehaviour
{
    public Vector3 originalScale;
    public Vector3 originalPosition;
    public Camera mainCamera;
    public AudioSource audioSource;
    public GameController gc;
    public int index;
    public bool inHand = true;
    public bool isDragging = false;
    public Card BindCard { get { return gc.handCards.Cards[index]; } }
    private bool inPlayArea = false;
    private void OnMouseEnter()
    {
        if (gc.gameStage == GameStage.Play && !gc.dc.isOpeningPage)
        {
            transform.SetAsLastSibling();
            //Debug.Log(index);
            audioSource.Play();
            transform.DOMove(originalPosition + Vector3.up * 0.75f, 0.1f);
            transform.DOScale(originalScale * 1.1f, 0.1f);
        }
    }
    private void OnMouseExit()
    {
        transform.SetSiblingIndex(index);
        if (inHand)
        {
            transform.DOScale(originalScale, 0.1f);
            transform.DOMove(originalPosition, 0.1f);
        }
    }
    void OnMouseDown()
    {
        // 开始拖动
        if (gc.gameStage == GameStage.Play && !gc.dc.isOpeningPage) isDragging = true;
    }
    void Update()
    {
        if (isDragging)
        {
            // 更新GameObject的位置到鼠标位置
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(transform.position).z));
            transform.position = mousePosition;
            Vector2 screenPosition = mainCamera.WorldToScreenPoint(mousePosition);
            if (!inPlayArea && gc.hcui.PlayArea.Contains(screenPosition))
            {
                inPlayArea = true;
                transform.DOScale(originalScale * 0.5f, 0.1f);
            }
            if (inPlayArea && !gc.hcui.PlayArea.Contains(screenPosition))
            {
                inPlayArea = false;
                transform.DOScale(originalScale * 1.1f, 0.1f);
            }
        }
    }
    void OnMouseUp()
    {
        if (!isDragging) return;
        if (inPlayArea && gc.waitingDiscardCount > 0)
        {
            inHand = false;
            StartCoroutine(DisCard());
            gc.waitingDiscardCount--;
        }
        else if (inPlayArea && gc.handCards.Cards[index].cost <= gc.player.sp)
        {
            var card = gc.handCards.Cards[index];
            int executeTimes = gc.player.buffContainer.CallPlayCard(gc.handCards.Cards[index]);

            if (executeTimes == -1) return;

            inHand = false;
            gc.handCards.Cards[index].Play();
            gc.roundPlayedCards.AddCard(gc.handCards.Cards[index]);

            // 夏侯阳算经效果:每打出一张手牌，获得2点护盾值。
            if (gc.player.ContainsBook(18)) gc.player.shield += 2;

            StartCoroutine(RemoveCard());

            for (int i = 0; i < executeTimes; i++)
            {
                gc.CardExecuteAction(card.id, card.isPlused);
            }
        }
        else transform.DOMove(originalPosition, 0.1f);
        if (gc.player.ContainsBook(20) && gc.handCards.Cards.Count == 0)
        {
            // 缀术效果:在你的回合，当你没有手牌时，抽一张牌。
            gc.DrawCards(1);
        }
        isDragging = false;
    }
    private IEnumerator RemoveCard()
    {
        //Debug.Log(gc.handCards.handCards[index].displayInfo.name);
        gc.player.ReduceSp(gc.handCards.Cards[index].cost);
        if (gc.handCards.Cards[index].disposable)
        {
            gc.discardPile.AddCardToDisposable(gc.handCards.Cards[index]);
        }
        else gc.discardPile.AddCardToDiscard(gc.handCards.Cards[index]);

        gc.handCards.RemoveCard(gc.handCards.Cards[index]);

        audioSource.Play();
        transform.DOMove(new Vector3(8f, -4f), 0.25f);
        transform.DOScale(0, 0.25f);
        gc.hcui.RemoveCard();
        StartCoroutine(gc.hcui.CardDisplayUpdate());
        yield return new WaitForSeconds(0.25f);
    }
    private IEnumerator DisCard()
    {
        //Debug.Log(gc.handCards.handCards[index].displayInfo.name);

        gc.discardPile.AddCardToDiscard(gc.handCards.Cards[index]);

        gc.handCards.RemoveCard(gc.handCards.Cards[index]);

        audioSource.Play();
        transform.DOMove(new Vector3(8f, -4f), 0.25f);
        transform.DOScale(0, 0.25f);
        gc.hcui.RemoveCard();
        StartCoroutine(gc.hcui.CardDisplayUpdate());
        yield return new WaitForSeconds(0.25f);
    }
}
