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
    public bool inHand=true;
    public bool isDragging=false;
    private bool inPlayArea=false;
    private void OnMouseEnter()
    {
        if(gc.gameStage==GameStage.Play)
        {
            transform.SetAsLastSibling();
            //Debug.Log(index);
            audioSource.Play();
            transform.DOMove(originalPosition+Vector3.up*0.75f,0.1f);
            transform.DOScale(originalScale*1.1f,0.1f);
        }
    }
    private void OnMouseExit()
    {
        transform.SetSiblingIndex(index);
        if(inHand)
        {
            transform.DOScale(originalScale,0.1f);
            transform.DOMove(originalPosition,0.1f);
        }
    }
    void OnMouseDown()
    {
        // 开始拖动
        if(gc.gameStage==GameStage.Play) isDragging=true;
    }
    void Update()
    {
        if(isDragging)
        {
            // 更新GameObject的位置到鼠标位置
            Vector3 mousePosition=mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(transform.position).z));
            transform.position=mousePosition;
            Vector2 screenPosition=mainCamera.WorldToScreenPoint(mousePosition);
            if(!inPlayArea&&gc.hcui.playArea.Contains(screenPosition))
            {
                inPlayArea=true;
                transform.DOScale(originalScale*0.5f,0.1f);
            }
            if(inPlayArea&&!gc.hcui.playArea.Contains(screenPosition))
            {
                inPlayArea=false;
                transform.DOScale(originalScale*1.1f,0.1f);
            }
        }
    }
    void OnMouseUp()
    {
        if(inPlayArea&&gc.waitingDiscardCount>0)
        {
            inHand=false;
            StartCoroutine(DisCard());
            gc.waitingDiscardCount--;
        }
        else if(inPlayArea&&gc.handCards.handCards[index].cost<=gc.player.sp)
        {
            var card=gc.handCards.handCards[index];
            int executeTimes=gc.player.buffContainer.CallPlayCard(card);
            
            if(executeTimes==-1) return;
            
            inHand=false;
            gc.handCards.handCards[index].Play();
            StartCoroutine(RemoveCard());
            for(int i=0;i<executeTimes;i++) gc.CardExecuteAction(card.id,card.isPlused);
        }
        else transform.DOMove(originalPosition,0.1f);
        isDragging=false;
    }
    private IEnumerator RemoveCard()
    {
        //Debug.Log(gc.handCards.handCards[index].displayInfo.name);
        gc.player.ReduceSP(gc.handCards.handCards[index].cost);
        if(gc.handCards.handCards[index].disposable)
        {
            gc.discardPile.AddCardToDisposable(gc.handCards.handCards[index]);
        }
        else gc.discardPile.AddCardToDiscard(gc.handCards.handCards[index]);
        
        gc.handCards.RemoveCard(gc.handCards.handCards[index]);

        audioSource.Play();
        transform.DOMove(new Vector3(8f,-4f),0.25f);
        transform.DOScale(0,0.25f);
        gc.hcui.RemoveCard();
        StartCoroutine(gc.hcui.CardDisplayUpdate());
        yield return new WaitForSeconds(0.25f);
    }
    private IEnumerator DisCard()
    {
        //Debug.Log(gc.handCards.handCards[index].displayInfo.name);

        gc.discardPile.AddCardToDiscard(gc.handCards.handCards[index]);
        
        gc.handCards.RemoveCard(gc.handCards.handCards[index]);

        audioSource.Play();
        transform.DOMove(new Vector3(8f,-4f),0.25f);
        transform.DOScale(0,0.25f);
        gc.hcui.RemoveCard();
        StartCoroutine(gc.hcui.CardDisplayUpdate());
        yield return new WaitForSeconds(0.25f);
    }
}
