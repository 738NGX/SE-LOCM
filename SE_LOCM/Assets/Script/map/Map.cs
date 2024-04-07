using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public List<MapNode> mapNodes=new();
    public static LocalSaveData localSaveData;
    public TextMeshProUGUI hpui;
    public TextMeshProUGUI coinui;
    public SceneFader sf;
    private void Start()
    {
        localSaveData=LocalSaveDataManager.LoadLocalData();
        int level=localSaveData.Level;
        foreach(Transform child in transform)
        {
            mapNodes.Add(child.GetComponent<MapNode>());
        }
        foreach(var mapNode in mapNodes)
        {
            mapNode.id+=100*(level-1);
            //mapNode.transform.Find("标签").GetComponent<Image>().sprite=MapDatabase.data[mapNode.id].type switch
            //{
                
            //};
            mapNode.sf=sf;
            if(localSaveData.route.Contains(mapNode.id))
            {
                mapNode.status=MapNodeStauts.Passed;
            }
            else if(MapDatabase.data[localSaveData.route[^1]].nextNodes.Contains(mapNode.id))
            {
                mapNode.status=MapNodeStauts.Available;
            }
            else
            {
                mapNode.status=MapNodeStauts.Unavailable;
            }
        }
    }
    private void Update()
    {
        hpui.text=localSaveData.hp+"/"+localSaveData.hpLimit;
        coinui.text=localSaveData.coins.ToString();
    }
    public void BackTheme()
    {
        localSaveData=LocalSaveDataManager.LoadLocalData();
        localSaveData.status=LocalSaveStatus.Break;
        LocalSaveDataManager.SaveLocalData(localSaveData);
        sf.FadeOut("Scenes/theme");
    }
}