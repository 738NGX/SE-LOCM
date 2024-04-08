using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class SavesController : MonoBehaviour
{
    public GameController gc;
    public LocalSaveData localSaveData;

    public void LoadLocalData()
    {
        localSaveData=LocalSaveDataManager.LoadLocalData();
        var node=new EnemyNodeData(localSaveData.route[^1]);
        gc.enemyIds=node.GetEnemyIds();
        gc.dc.bg.sprite=Resources.Load<Sprite>(node.Background);
        
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
