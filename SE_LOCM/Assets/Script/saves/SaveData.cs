using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class LocalSaveDataManager
{
    public static void SaveLocalData(LocalSaveData localSaveData)
    {
        if(!File.Exists(Application.persistentDataPath+"/users"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/users");
        }
        string jsonData=JsonConvert.SerializeObject(localSaveData);
        File.WriteAllText(Application.persistentDataPath+"/users/localsave.json",jsonData);
        Debug.Log("Saveed To"+Application.persistentDataPath+"/users/localsave.json");
    }
    public static LocalSaveData LoadLocalData()
    {
        string path=Application.persistentDataPath+"/users/localsave.json";
        if(!File.Exists(path)) SavesController.SaveCustomLocalData();
        string jsonData=File.ReadAllText(path);
        return JsonConvert.DeserializeObject<LocalSaveData>(jsonData);
    }
}

public class LocalSaveData
{
    // 战斗数据
    public int hp;          // 体力值
    public int hpLimit;     // 体力值上限
    public int initAp;      // 初始攻击力
    public int initDp;      // 初始防御力
    public int initSp;      // 初始算术值

    // 资源数据
    public int coins;                           // 金币
    public List<(int,bool)> cardsData=new(){};  // 牌组
    public List<int> booksData;                 // 典籍

    // 地图数据
    public int currentLevel;                    // 当前层数
    public int currentPos;                      // 当前位置
}
