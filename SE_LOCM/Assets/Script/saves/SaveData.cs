using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;

public enum LocalSaveStatus{Break,Gaming,Victory,Defeat};
public class LocalSaveData
{
    // 游戏状态
    public LocalSaveStatus status;
    
    // 战斗数据
    public int hp;          // 体力值
    public int hpLimit;     // 体力值上限
    public int initAp;      // 初始攻击力
    public int initDp;      // 初始防御力
    public int initSp;      // 初始算术值

    // 资源数据
    public int coins;                           // 金币
    public List<(int,bool)> cardsData;          // 牌组
    public List<int> booksData;                 // 典籍
    public List<int> cardsPool;                 // 卡池

    // 地图数据
    public List<int> route;                     // 当层路径

    // 数据写入
    public void AdjustHP(int val)
    {
        if(hp+val>=hpLimit) hp=hpLimit;
        else if(hp+val<=0) hp=0;
        else hp+=val;
    }
    public void AdjustCoins(int val)
    {
        if(coins+val<=0) coins=0;
        else coins+=val;
    }
    public void AddCardsData(List<Card> cards)
    {
        foreach(var card in cards)
        {
            cardsData.Add((card.id,card.isPlused));
        }
    }
    public void AddCardsPool(List<Card> cards)
    {
        foreach(var card in cards)
        {
            if(cardsPool.Contains(card.id)) continue;
            cardsPool.Add(card.id);
        }
    }
    public void AddCardsPool(List<int> cards)
    {
        foreach(var card in cards)
        {
            if(cardsPool.Contains(card)) continue;
            cardsPool.Add(card);
        }
    }
    public void AddBooksData(List<Book> books)
    {
        foreach(var book in books)
        {
            if(booksData.Contains(book.id)) continue;
            booksData.Add(book.id);
        }
    }
    // 数据导出
    public List<Card> ReadCardsData()
    {
        List<Card> result=new();
        foreach(var data in cardsData) result.Add(new Card(data.Item1,data.Item2));
        return result;
    }
    public List<Card> ReadNotUpdatedCardsData()
    {
        List<Card> result=new();
        foreach(var data in cardsData)
        {
            if(!data.Item2) result.Add(new Card(data.Item1,data.Item2));
        } 
        return result;
    }
    public List<Card> ReadCardsPool()
    {
        List<Card> result=new();
        foreach(var data in cardsPool) result.Add(new Card(data));
        return result;
    }
    public List<Book> ReadBooksData()
    {
        List<Book> result=new();
        foreach(var data in booksData) result.Add(new Book(data));
        return result;
    }
}

public static class LocalSaveDataManager
{
    [MenuItem("存档测试/初始存档")]
    public static void SaveInitLocalData()
    {
        LocalSaveData localSaveData=new()
        {
            status=LocalSaveStatus.Break,
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
            cardsPool=new(){},
            route=new(){100}
        };
        SaveLocalData(localSaveData);
    }
    [MenuItem("存档测试/自定义存档")]
    public static void SaveCustomLocalData()
    {
        LocalSaveData localSaveData=new()
        {
            status=LocalSaveStatus.Break,
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
            cardsPool=new(){100,103,104,105,106,107,108,109,110,111,112,113,114},
            route=new(){100,101,102}
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
        //Debug.Log("Saveed To: "+Application.persistentDataPath+"/users/localsave.json");
    }
    public static LocalSaveData LoadLocalData()
    {
        string path=Application.persistentDataPath+"/users/localsave.json";
        if(!File.Exists(path)) SaveCustomLocalData();
        string jsonData=File.ReadAllText(path);
        return JsonConvert.DeserializeObject<LocalSaveData>(jsonData);
    }
}
