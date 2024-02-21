using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ThemeButton : MonoBehaviour
{
    public int id;
    public MainTheme mt;
    private Image image;
    private Transform text;
    private bool waitClick=false;

    private void Start()
    {
        image=GetComponent<Image>();
        text=transform.GetChild(0);
    }
    private void OnMouseEnter()
    {
        image.DOFade(0.75f,0.2f).From(0);
        text.DOScale(1.1f,0.2f);
    }
    private void OnMouseExit()
    {
        image.DOFade(0,0.2f).From(0.75f);
        text.DOScale(1f,0.2f);
        waitClick=false;
    }
    private void OnMouseDown()
    {
        if(id<5&&mt.popWindow) return;
        waitClick=true;
    }
    private void OnMouseUp()
    {
        if(id<5&&mt.popWindow) return;
        if(waitClick)
        {
            switch(id)
            {
                case 0: mt.StartGameCheck(); break;
                case 1: mt.sf.FadeOut("Scenes/wiki"); break;
                case 4: mt.PopWinodw(1); break;
                case 7: mt.DisPopWindow(0); break;
                case 8: mt.DisPopWindow(1); break;
                case 9: mt.ExitGame(); break;
                case 10: mt.DisPopWindow(1); break;
                case 11: mt.wiki.ChangeWikiStatus(WikiStatus.Card); break;
                case 12: mt.wiki.ChangeWikiStatus(WikiStatus.Book); break;
                case 15: mt.sf.FadeOut("Scenes/theme"); break;
                default: break;
            }
        }
    }
}
