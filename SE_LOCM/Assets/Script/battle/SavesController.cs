using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SavesController : MonoBehaviour
{
    public GameController gc;
    public static LocalSaveData localSaveData=new();
    
    [MenuItem("存档测试/存档")]
    public static void SaveCustomLocalData()
    {
        localSaveData.hp=70;
        localSaveData.hpLimit=70;
        localSaveData.initAp=0;
        localSaveData.initDp=0;
        localSaveData.initSp=3;
        localSaveData.coins=114;
        localSaveData.cardsData=new(){
            (101,false),(101,false),(101,false),(102,false),(102,false),(102,false),
            (100,false),(101,false),(102,false),(103,false),(104,false),
            (105,false),(106,false),(107,false),(108,false),(109,false),
            (110,false),(111,false),(112,false),(113,false),(114,false),
        };
        localSaveData.booksData=new(){};
        localSaveData.currentLevel=0;
        localSaveData.currentPos=0;
        LocalSaveDataManager.SaveLocalData(localSaveData);
    }

    public void LoadLocalData()
    {
        localSaveData=LocalSaveDataManager.LoadLocalData();
        gc.player.Init(localSaveData);
        gc.drawPile.Init(localSaveData.cardsData);
    }
}
