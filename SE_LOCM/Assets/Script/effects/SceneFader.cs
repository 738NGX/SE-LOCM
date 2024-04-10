using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Start()
    {
        FadeIn();
        //Camera.main.orthographicSize = Camera.main.orthographicSize * 1920 / 1080 * Screen.height / Screen.width;
    }

    public void FadeIn()
    {
        fadeImage.DOFade(0,fadeDuration).From(1).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            fadeImage.gameObject.SetActive(false);
        });
    }

    public void FadeOut(string sceneToLoad)
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1,fadeDuration).From(0).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneToLoad);
        });
    }
}
