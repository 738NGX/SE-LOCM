using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int hp;              // 体力值(HealthPoints)
    public int sp;              // 法术值(SpellPoints)
    public int hp_limit;        // 体力值上限
    public int sp_init;         // 每回合初始法术值
    public int attack_addon;    // 攻击加成
    public int defence_addon;   // 防御加成
    public int shield;          // 护盾

    void Start()
    {
        hp_limit=50;
        hp=hp_limit;
        sp_init=3;
        RecoverSP();
    }

    public void AddHP(int val)
    {
        if(val<1) return;
        if(hp+val>=hp_limit) hp=hp_limit;
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
    public void RecoverSP(){sp=sp_init;}
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
