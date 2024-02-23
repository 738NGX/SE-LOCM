using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public List<MapNode> mapNodes=new();
    public static LocalSaveData localSaveData;
    public SceneFader sf;
    private void Start()
    {
        localSaveData=LocalSaveDataManager.LoadLocalData();
        foreach(Transform child in transform)
        {
            mapNodes.Add(child.GetComponent<MapNode>());
        }
        foreach(var mapNode in mapNodes)
        {
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
    public void BackTheme()
    {
        localSaveData=LocalSaveDataManager.LoadLocalData();
        localSaveData.status=LocalSaveStatus.Break;
        LocalSaveDataManager.SaveLocalData(localSaveData);
        sf.FadeOut("Scenes/theme");
    }
}