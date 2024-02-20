using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;

public class LocalSaveDataManager
{
    [MenuItem("存档测试/初始存档")]
    public static void SaveInitLocalData()
    {
        LocalSaveData localSaveData=new()
        {
            hp=70,
            hpLimit=70,
            initAp=0,
            initDp=0,
            initSp=3,
            coins=114,
            cardsData=new(){
                (101,false),(101,false),(101,false),(101,false),(103,false),
                (102,false),(102,false),(102,false),(102,false),(104,false),
                (100,false)
            },
            booksData=new(){},
            currentLevel=0,
            route=new(){0}
        };
        SaveLocalData(localSaveData);
    }
    [MenuItem("存档测试/自定义存档")]
    public static void SaveCustomLocalData()
    {
        LocalSaveData localSaveData=new()
        {
            hp=70,
            hpLimit=70,
            initAp=0,
            initDp=0,
            initSp=3,
            coins=114,
            cardsData=new(){
                (101,false),(101,false),(101,false),(101,false),(103,false),
                (102,false),(102,false),(102,false),(102,false),(104,false),
                (100,false),(105,false),(106,false),(107,false),(108,false),
                (109,false),(110,false),(111,false),(112,false),(113,false),
                (114,false),
            },
            booksData=new(){},
            currentLevel=0,
            route=new(){0}
        };
        SaveLocalData(localSaveData);
    }
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
        if(!File.Exists(path)) SaveCustomLocalData();
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
    public List<int> route;                     // 当层路径
}
