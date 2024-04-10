using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class RoundButton : MonoBehaviour
{
    public GameController gc;
    private Vector3 originalScale;
    private bool waitClick;
    private void Start()
    {
        originalScale = transform.localScale;
    }
    private void OnMouseEnter()
    {
        transform.DOScale(originalScale * 1.1f, 0.1f);
    }
    private void OnMouseExit()
    {
        if (waitClick) waitClick = false;
        transform.DOScale(originalScale, 0.1f);
    }
    private void OnMouseDown()
    {
        waitClick = true;
    }
    private void OnMouseUp()
    {
        if (waitClick && !gc.dc.isOpeningPage && gc.gameStage == GameStage.Play)
        {
            gc.gameStage = GameStage.Discard;
            waitClick = false;
        }
    }
}
