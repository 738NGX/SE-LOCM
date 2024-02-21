using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

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
        if(MapDatabase.data[id].type==MapNodeType.Enemy)
        {
            sf.FadeOut("Scenes/battle/b1-01");
        }
        else if(MapDatabase.data[id].type==MapNodeType.Senior)
        {
            sf.FadeOut("Scenes/battle/b1-01");
        }
    }
}
