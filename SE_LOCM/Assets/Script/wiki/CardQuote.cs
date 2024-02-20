using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CardQuote : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public Image backgroundImage;
    private bool isClicked = false;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Color originalTextColor;

    void Start()
    {
        originalPosition=textMesh.transform.position;
        originalScale=textMesh.transform.localScale;
        originalTextColor=textMesh.color;

        textMesh.gameObject.AddComponent<Button>().onClick.AddListener(() => ToggleState());
        //backgroundImage.gameObject.AddComponent<Button>().onClick.AddListener(() => ToggleState());
    }

    void ToggleState()
    {
        if (!isClicked)
        {
            originalPosition=textMesh.transform.position;
            originalScale=textMesh.transform.localScale;
            backgroundImage.gameObject.SetActive(true);
            textMesh.DOColor(Color.white, 0.5f);
            textMesh.transform.DOMove(new Vector3(0,0),0.5f);
            textMesh.transform.DOScale(originalScale*3f,0.5f);
            backgroundImage.DOFade(1,0.5f).From(0);
            textMesh.alignment=TextAlignmentOptions.MidlineLeft;
        }
        else
        {
            textMesh.DOColor(originalTextColor,0.5f);
            textMesh.transform.DOMove(originalPosition, 0.5f);
            textMesh.transform.DOScale(originalScale,0.5f);
            backgroundImage.DOFade(0,0.75f).From(1).OnComplete(() =>
            {
                backgroundImage.gameObject.SetActive(false);
            });
            textMesh.alignment=TextAlignmentOptions.BottomLeft;
        }
        isClicked = !isClicked;
    }
}
