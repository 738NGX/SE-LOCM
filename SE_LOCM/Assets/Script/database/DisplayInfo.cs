using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Fungus;

public class DisplayInfo
{
    public int id;
    public string name;
}

public class CardDisplayInfo : DisplayInfo
{
    public string cost;         // 消耗
    public string type;         // 属性
    public string rarity;       // 稀有度
    public string disposable;   // 一次性
    public string effect;       // 效果
    public string plusedEffect; // 强化效果
    public string quote;        // 原文
    public CardDisplayInfo(string rawData)
    {
        string[] values=rawData.Split(',');
        id=int.Parse(values[0]);
        name=values[1];
        cost=values[2];
        type=values[3];
        rarity=values[4];
        disposable=values[5];
        effect=values[6];
        plusedEffect=values[7];
        quote=values[8].Replace("\\n", "\n");;
    }
    public CardDisplayInfo Copy()
    {
        return new CardDisplayInfo($"{id},{name},{cost},{type},{rarity},{disposable},{effect},{plusedEffect},{quote.Replace("\n", "\\n")}");
    }
};

public class BookDisplayInfo : DisplayInfo
{
    public string rarity;
    public string effect;
    public string introduction;
    public BookDisplayInfo(string rawData)
    {
        string[] values=rawData.Split(',');
        id=int.Parse(values[0]);
        name=values[1];
        rarity=values[2];
        effect=values[3];
        introduction=values[4];
    }
    public BookDisplayInfo Copy()
    {
        return new BookDisplayInfo($"{id},{name},{rarity},{effect},{introduction}");
    }
}

public class EnemyInfo : DisplayInfo
{
    public int hpLimit;
    public int ap;
    public int dp;
    public int apAddSpeed;
    public int dpAddSpeed;
    public int apAddChance;
    public int dpAddChance;
    public int mAttackTimes;
    public List<Buff> initBuffs=new();
    public List<Buff> giveBuffs=Enumerable.Repeat<Buff>(null,10).ToList();
    public List<Buff> extraGiveBuffs=Enumerable.Repeat<Buff>(null,10).ToList();
    public List<IntendType> intends=new();
    public List<int> intendChances=new();

    public EnemyInfo()
    {
        
    }
    public EnemyInfo(string rawData)
    {
        string[] values=rawData.Split(',');
        
        id=int.Parse(values[0]);
        name=values[1];
        hpLimit=int.Parse(values[2]);
        ap=int.Parse(values[3]);
        dp=int.Parse(values[4]);
        apAddSpeed=int.Parse(values[5]);
        dpAddSpeed=int.Parse(values[6]);
        apAddChance=int.Parse(values[7]);
        dpAddChance=int.Parse(values[8]);
        mAttackTimes=int.Parse(values[9]);

        int[] initBuffIds=Array.ConvertAll(values[10].Split(';'),s=>int.Parse(s));
        int[] initBuffLevels=Array.ConvertAll(values[11].Split(';'),s=>int.Parse(s));
        initBuffs=initBuffIds.Zip(initBuffLevels,(id,level)=>new Buff(id,level)).ToList();

        int[] intendBuffIds=Array.ConvertAll(values[12].Split(';'),s=>int.Parse(s));
        int[] intendBuffLevels=Array.ConvertAll(values[13].Split(';'),s=>int.Parse(s));
        List<Buff> intendBuffs=intendBuffIds.Zip(intendBuffLevels,(id,level)=>new Buff(id,level)).ToList();
        intendBuffs.Reverse();
        Stack<Buff> intendBuffsStack=new(intendBuffs);

        for(int i=14;i<34;i++)
        {
            if(i%2==0)
            {
                IntendType type=values[i] switch
                {
                    "攻击"=>IntendType.Attack,
                    "连击"=>IntendType.MAttack,
                    "重击"=>IntendType.HAttack,
                    "防御"=>IntendType.Defence,
                    "正面状态"=>IntendType.Buff,
                    "负面状态"=>IntendType.Debuff,
                    "回血"=>IntendType.Recover,
                    "休息"=>IntendType.Sleep,
                    "攻击防御"=>IntendType.ADefence,
                    "攻击正面"=>IntendType.ABuff,
                    "攻击负面"=>IntendType.ADebuff,
                    "攻击回血"=>IntendType.ARecover,
                    _=>IntendType.Unknown
                };
                
                if(new[]{IntendType.Buff,IntendType.Debuff,IntendType.ABuff,IntendType.ADebuff}.Contains(type))
                {
                    var buff=intendBuffsStack.Pop();
                    giveBuffs[(i-14)/2]=buff;
                    
                    if(new[]{301,306,307,308,309}.Contains(buff.id))
                    {
                        var extraBuff=intendBuffsStack.Pop();
                        extraGiveBuffs[(i-14)/2]=extraBuff;
                    }
                }
                
                intends.Add(type);
            }
            else intendChances.Add(int.Parse(values[i]));
        }
    }
    public EnemyInfo Copy()
    {
        EnemyInfo copy=new()
        {
            id=id,
            name=name,
            hpLimit=hpLimit,
            ap=ap,
            dp=dp,
            apAddSpeed=apAddSpeed,
            dpAddSpeed=dpAddSpeed,
            apAddChance=apAddChance,
            dpAddChance=dpAddChance,
            mAttackTimes=mAttackTimes,

            initBuffs=new(initBuffs.Select(buff => new Buff(buff.id, buff.Level))),
            giveBuffs=new(giveBuffs.Select(buff => buff != null ? new Buff(buff.id, buff.Level) : null)),
            extraGiveBuffs=new List<Buff>(extraGiveBuffs.Select(buff => buff != null ? new Buff(buff.id, buff.Level) : null)),
            intends=new(intends),
            intendChances=new(intendChances)
        };
        return copy;
    }
}

public class ProblemInfo : DisplayInfo
{
    public string TrueAnswer{get; private set;}
    public string FalseAnswer{get; private set;}

    public ProblemInfo(string rawData)
    {
        string[] values=rawData.Split(',');
        
        id=int.Parse(values[0]);
        name=values[1];
        TrueAnswer=values[2];
        FalseAnswer=values[3];
    }
}