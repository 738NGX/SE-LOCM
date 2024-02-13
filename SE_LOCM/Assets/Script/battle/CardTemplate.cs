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
    private int originalIndex;
    private bool isDragging=false;
    private bool inPlayArea=false;
    private void Start()
    {
        originalIndex=transform.GetSiblingIndex();
    }
    private void OnMouseEnter()
    {
        transform.SetAsLastSibling();
        audioSource.Play();
        transform.DOMove(originalPosition+Vector3.up*0.75f,0.1f);
        transform.DOScale(originalScale*1.1f,0.1f);
    }
    private void OnMouseExit()
    {
        transform.SetSiblingIndex(originalIndex);
        transform.DOScale(originalScale,0.1f);
        transform.DOMove(originalPosition,0.1f);
    }
    void OnMouseDown()
    {
        // 开始拖动
        isDragging = true;
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
        // 停止拖动并返回起始位置
        if(inPlayArea&&gc.handCards.handCards[index].cost<=gc.player.sp)
        {
            gc.CardExecuteAction(gc.handCards.handCards[index].id);
            StartCoroutine(RemoveCard());
            Destroy(this);
        }
        isDragging=false;
        // 使用DOTween来移动GameObject回到起始位置
        transform.DOMove(originalPosition,0.1f);
    }
    private IEnumerator RemoveCard()
    {
        Debug.Log(gc.handCards.handCards[index].displayInfo.name);
        gc.player.ReduceSP(gc.handCards.handCards[index].cost);
        gc.discardPile.AddCardToDiscard(gc.handCards.handCards[index]);
        gc.handCards.RemoveCard(gc.handCards.handCards[index]);
        
        gc.hcui.RemoveCard();

        Sequence s=DOTween.Sequence();
        s.Append(transform.DOMove(new Vector3(8f,-4f),0.5f));
        s.Join(transform.DOScale(0,0.5f));
        audioSource.Play();
        s.Play();

        yield return new WaitForSeconds(0.5f);
    }
}
