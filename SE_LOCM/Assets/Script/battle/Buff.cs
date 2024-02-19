using System.Collections.Generic;
using UnityEngine;

// 强化,硬化,虚弱,脆弱,回复,中毒
public enum BuffType{Strengthen,Harden,Infirmity,Frail,Recover,Poisoned};
// 护甲,免疫
public enum LatentBuffType{Armor,Immunity};

// 普通buff
public class Buff
{
    public BuffContainer container;
    public BuffType type;
    public int remainRounds;
    private int endRound;

    public Buff(BuffContainer buffContainer,BuffType buffType,int effectRounds)
    {
        container=buffContainer;
        type=buffType;
        remainRounds=effectRounds;
        endRound=container.gc.roundCount+effectRounds;
    }
    public bool IsDebuff()
    {
        return type==BuffType.Infirmity||type==BuffType.Frail||type==BuffType.Poisoned;
    }
    public void RoundCountUpdate()
    {
        remainRounds=endRound-container.gc.roundCount;
    }
    public void AdjustEndRound(int val)
    {
        endRound+=val;
    }
}

// 延时Buff
public class LatentBuff
{
    public BuffContainer container;
    public LatentBuffType type;
}