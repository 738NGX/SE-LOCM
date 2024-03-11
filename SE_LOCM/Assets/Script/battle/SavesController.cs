using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavesController : MonoBehaviour
{
    public GameController gc;
    public static LocalSaveData localSaveData;

    public void LoadLocalData()
    {
        localSaveData=LocalSaveDataManager.LoadLocalData();
        gc.player.Init(localSaveData);
        gc.drawPile.Init(localSaveData.cardsData);
    }
    public void SaveLocalData()
    {
        localSaveData.hp=gc.player.hp;
        localSaveData.hpLimit=gc.player.hpLimit;
        LocalSaveDataManager.SaveLocalData(localSaveData);
    }
}
