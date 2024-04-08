using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavesController : MonoBehaviour
{
    public GameController gc;
    public LocalSaveData localSaveData;

    public void LoadLocalData()
    {
        localSaveData=LocalSaveDataManager.LoadLocalData();
        gc.enemyIds=new EnemyNodeData(localSaveData.route[^1]).GetEnemyIds();
        
        gc.player.Init(localSaveData);
        gc.drawPile.Init(localSaveData.cardsData);
    }
    public void SaveLocalData()
    {
        localSaveData.hp=gc.player.hp;
        localSaveData.hpLimit=gc.player.hpLimit;
        foreach(var friend in gc.friends)
        {
            localSaveData.friends[friend.index]=friend.WaitRound;
        }
        localSaveData.FriendsCoolDown();
        LocalSaveDataManager.SaveLocalData(localSaveData);
    }
}
