using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int hp;              // 体力值(HealthPoints)
    public int sp;              // 法术值(SpellPoints)
    public int hpLimit;         // 体力值上限
    public int spInit;          // 每回合初始算术值
    public int ap;              // 攻击力
    public int dp;              // 防御力
    public int shield;          // 护盾

    public void Init(LocalSaveData localSaveData)
    {
        hpLimit=localSaveData.hpLimit;
        hp=localSaveData.hp;
        ap=localSaveData.initAp;
        dp=localSaveData.initDp;
        spInit=localSaveData.initSp;
        RecoverSP();
    }

    public void AddHP(int val)
    {
        if(val<1) return;
        if(hp+val>=hpLimit) hp=hpLimit;
        else hp+=val;
    }
    public void ReduceHP(int val)
    {
        if(val<1) return;
        if(hp+shield-val<=0) hp=0;
        else if(val>shield)
        {
            hp-=val-shield;
            shield=0;
        }
        else shield-=val;
    }
    public void RecoverSP(){sp=spInit;}
    public void AddSP(int val)
    {
        if(val<1) return;
        sp+=val;
    }
    public void ReduceSP(int val)
    {
        if(val<1) return;
        if(sp-val<=0) sp=0;
        else sp-=val;
    }
}
