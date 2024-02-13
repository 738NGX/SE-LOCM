using UnityEngine;
using DG.Tweening;

public class CardTemplate : MonoBehaviour
{
    public Vector3 originalScale;
    public Vector3 originalPosition;
    public Camera mainCamera;
    public AudioSource audioSource;
    private int originalIndex;
    private bool isDragging=false;
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
        if (isDragging)
        {
            // 更新GameObject的位置到鼠标位置
            Vector3 mousePosition=mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(transform.position).z));
            transform.position=mousePosition;
        }
    }
    void OnMouseUp()
    {
        // 停止拖动并返回起始位置
        isDragging=false;
        // 使用DOTween来移动GameObject回到起始位置
        transform.DOMove(originalPosition,0.1f);
    }
}
