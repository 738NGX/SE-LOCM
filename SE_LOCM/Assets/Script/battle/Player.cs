using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    public int sp;              // 法术值(SpellPoints)
    public int spInit;          // 每回合初始算术值
    public int coins;
    public List<int> books;

    private void Start()
    {
        buffContainer.creature=this;
    }
    public void Init(LocalSaveData localSaveData)
    {
        hpLimit=localSaveData.hpLimit;
        hp=localSaveData.hp;
        ap=localSaveData.initAp;
        dp=localSaveData.initDp;
        spInit=localSaveData.initSp;
        coins=localSaveData.coins;
        books=localSaveData.booksData;

        RecoverSp();
    }
    public override void AddHP(int val)
    {
        base.AddHP(val);
        gc.dc.UpdateHPSlider(-1);
    }
    public override void ReduceHP(int val)
    {
        base.ReduceHP(val);
        gc.dc.UpdateHPSlider(-1);
    }
    public override void AddShield(int val)
    {
        gc.AddShield(val);
    }
    public override void AddBuff(Buff buff)
    {
        base.AddBuff(buff);
        switch(buff.Style)
        {
            case BuffStyle.Positive: gc.UpArrow(); break;
            case BuffStyle.Negative: gc.DownArrow(); break;
            default: break;
        }
    }
    public void RecoverSp()
    {
        sp=spInit;
    }
    public void AddSp(int val)
    {
        if(val<1) return;
        sp+=val;
    }
    public void ReduceSp(int val)
    {
        if(val<1) return;
        if(sp-val<=0) sp=0;
        else sp-=val;
    }
    public void AddCoins(int val)
    {
        if(val<1) return;
        coins+=val;
    }
    public int ReduceCoins(int val)
    {
        if(val<1) return 0;
        if(coins-val<=0)
        { 
            int res=coins;
            coins=0;
            return res;
        }
        else coins-=val; 
        return val;
    }
    public bool ContainsBook(int id)
    {
        return books.Contains(id);
    }
}
