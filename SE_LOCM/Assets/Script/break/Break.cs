using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Break : MonoBehaviour
{
    public SceneFader sf;
    public RectTransform page;
    public GameObject resultStringUI;
    private LocalSaveData localSaveData;
    private string resultString;
    private void Start()
    {
        page.DOMoveY(0,1f).From(-10);

        localSaveData=LocalSaveDataManager.LoadLocalData();
    }
    public void ExecuteBreak(int breakType)
    {
        // 0 客房;1 论战堂;2 棋室;3 书室;4 酒室;5 茶室
        switch(breakType)
        {
            case 0: Recover(); break;
            case 2: AdjustCoins(); break;
            case 4: AddAttack(); break;
            case 5: AddDefence(); break;
            default: break; 
        }
        EndBreak();
    }
    private void Recover()
    {
        int val=localSaveData.hpLimit/5;
        localSaveData.AdjustHP(val);
        resultString="在客房睡到了天亮,回复了"+val+"点体力值";
    }
    private void AdjustCoins()
    {
        int val=Random.Range(-100,101);
        localSaveData.AdjustCoins(val);
        if(val>0) resultString="对弈胜利,赢下了"+val+"银币";
        else if(val<0) resultString="对弈失败,输掉了"+(-val)+"银币";
        else resultString="对弈平局,没有赢得也没有输掉银币";
    }
    private void AddAttack()
    {
        localSaveData.initAp++;
        resultString="";
    }
    private void AddDefence()
    {
        localSaveData.initDp++;
    }
    private void EndBreak()
    {
        page.DOMoveY(-10,1f);
        LocalSaveDataManager.SaveLocalData(localSaveData);
        sf.FadeOut(MainTheme.NextScene(localSaveData));
    }
}
