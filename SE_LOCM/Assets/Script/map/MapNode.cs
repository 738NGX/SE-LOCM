using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Fungus;

// 可到达,不可到达,已通过
public enum MapNodeStauts{Available,Unavailable,Passed};
// 剧情,休息,商铺,宝箱,未知,敌人,精英,Boss
public enum MapNodeType{Story,Break,Shop,Box,Unknown,Enemy,Senior,Boss};

public class MapNode : MonoBehaviour
{
    public int id;
    public MapNodeStauts status;
    public SceneFader sf;
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => LoadScene(id));
    }
    private void Update()
    {
        string statusStr=status switch
        {
            MapNodeStauts.Unavailable=>"unavailable",
            MapNodeStauts.Available=>"available",
            MapNodeStauts.Passed=>"passed",
            _=>"unavailable"
        };
        gameObject.GetComponent<Image>().sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/map/"+statusStr+".png");
    }
    private void LoadScene(int id)
    {
        if(status!=MapNodeStauts.Available) return;
        
        LocalSaveData localSaveData=LocalSaveDataManager.LoadLocalData();
        
        if(!localSaveData.route.Contains(id)) localSaveData.route.Add(id);
        LocalSaveDataManager.SaveLocalData(localSaveData);

        var type=MapDatabase.data[id].type;

        if(type==MapNodeType.Story)
        {
            if(!MapNodeIdToStorySceneDatabase.data.TryGetValue(id,out var target)) return;
            sf.FadeOut(target);
        }
        else if(type==MapNodeType.Unknown)
        {
            sf.FadeOut("Scenes/unknown");
        }
        else if(type==MapNodeType.Break)
        {
            sf.FadeOut("Scenes/break");
        }
        else if(type==MapNodeType.Box)
        {
            sf.FadeOut("Scenes/reward");
        }
        else if(type==MapNodeType.Enemy||type==MapNodeType.Senior)
        {
            sf.FadeOut("Scenes/battle");
        }
        else if(type==MapNodeType.Boss)
        {
            if(localSaveData.ContainsBook(19))
            {
                // 算术书效果:Boss战开始前，回复25点体力值。
                localSaveData.AdjustHP(25);
                LocalSaveDataManager.SaveLocalData(localSaveData);
            }
            sf.FadeOut("Scenes/battle");
        }
    }
}
