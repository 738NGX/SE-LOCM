using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Problem : MonoBehaviour
{
    public SceneFader sf;
    public ProblemInfo UsingProblem{get; private set;}
    public TextMeshProUGUI problemUI;
    public Button endProblem;
    public Button option1;
    public Button option2;
    private LocalSaveData localSaveData;
    private List<int> problemIds;
    private List<int> seeds;
    private void Start()
    {
        localSaveData=LocalSaveDataManager.LoadLocalData();
        seeds=Enumerable.Range(0,3).Select(_ => Random.Range(0,101)).ToList();
        problemIds=ProblemDatabase.data.Keys.ToList();
        UsingProblem=ProblemDatabase.data[GetProblemDataId(localSaveData.Level)];

        problemUI.text=UsingProblem.name;

        if(seeds[0]<50)
        {
            option1.GetComponentInChildren<TextMeshProUGUI>().text=UsingProblem.TrueAnswer;
            option1.onClick.AddListener(() => CallTrueAnswer());
            option2.GetComponentInChildren<TextMeshProUGUI>().text=UsingProblem.FalseAnswer;
            option2.onClick.AddListener(() => CallFalseAnswer());
        }
        else
        {
            option1.GetComponentInChildren<TextMeshProUGUI>().text=UsingProblem.FalseAnswer;
            option1.onClick.AddListener(() => CallFalseAnswer());
            option2.GetComponentInChildren<TextMeshProUGUI>().text=UsingProblem.TrueAnswer;
            option2.onClick.AddListener(() => CallTrueAnswer());
        }
    }
    public void CallTrueAnswer()
    {
        problemUI.text="太棒了!回答正确.";
        option1.gameObject.SetActive(false);
        option2.gameObject.SetActive(false);
        endProblem.gameObject.SetActive(true);

        if(seeds[1]<30)
        {
            localSaveData.AdjustHP(seeds[2]/5+1);
            problemUI.text+=$"回复了{seeds[2]/5+1}点体力值.";
        }
        else if(seeds[1]<60)
        {
            localSaveData.AdjustCoins(seeds[2]/3+1);
            problemUI.text+=$"获得了{seeds[2]/3+1}枚银币.";
        }
        else if(seeds[1]<80)
        {
            localSaveData.initAp+=seeds[2]/20+1;
            problemUI.text+=$"在之后的战斗中,基础攻击力+{seeds[2]/20+1}.";
        }
        else
        {
            localSaveData.initDp+=seeds[2]/20+1;
            problemUI.text+=$"在之后的战斗中,基础防御力+{seeds[2]/20+1}.";
        }
    }
    public void CallFalseAnswer()
    {
        problemUI.text="呃啊!回答错误.";
        option1.gameObject.SetActive(false);
        option2.gameObject.SetActive(false);
        endProblem.gameObject.SetActive(true);

        if(seeds[1]<30)
        {
            localSaveData.AdjustHP(-seeds[2]/10-1);
            problemUI.text+=$"损失了{seeds[2]/10+1}点体力值.";
        }
        else if(seeds[1]<60)
        {
            localSaveData.AdjustCoins(-seeds[2]/3-1);
            problemUI.text+=$"损失了{seeds[2]/3+1}枚银币.";
        }
        else if(seeds[1]<80)
        {
            localSaveData.initAp-=seeds[2]/50+1;
            problemUI.text+=$"在之后的战斗中,基础攻击力-{seeds[2]/50+1}.";
        }
        else
        {
            localSaveData.initDp-=seeds[2]/50+1;
            problemUI.text+=$"在之后的战斗中,基础攻击力-{seeds[2]/50+1}.";
        }
    }
    public void CallEndProblem()
    {
        localSaveData.initAp=localSaveData.initAp<0 ? 0 : localSaveData.initAp;
        localSaveData.initDp=localSaveData.initDp<0 ? 0 : localSaveData.initDp;
        LocalSaveDataManager.SaveLocalData(localSaveData);
        sf.FadeOut(MainTheme.NextScene(localSaveData));
    }
    private int GetProblemDataId(int digit)
    {
        var filteredIds=problemIds.Where(n => n/100==digit).ToList();
        int index=Random.Range(0,filteredIds.Count);
        return filteredIds[index];
    }
}
