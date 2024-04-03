using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public enum BuffContainerStatus { None, Profit, Inadequate }
public class BuffContainer
{
    public Creature creature;
    public List<Buff> buffs = new();
    public float AttackRate { get; private set; }
    public float DefenceRate { get; private set; }
    public int StealedCoins { get; private set; }
    public int ExtraCard { get; private set; }
    public int ExtraSP { get; private set; }
    public BuffContainerStatus Status { get; private set; } = BuffContainerStatus.None;

    public void AddBuff(Buff buff)
    {
        if (buff.id == 401)
        {
            if (Status == BuffContainerStatus.Profit)
            {
                Status = BuffContainerStatus.Inadequate;
                creature.gc.player.sp+=2;
            }
            Status = BuffContainerStatus.Profit;
        }
        else if (buff.id == 402)
        {
            if (Status == BuffContainerStatus.Inadequate)
            {
                Status = BuffContainerStatus.Profit;
            }
            Status = BuffContainerStatus.Inadequate;
        }
        else if (buff.Style == BuffStyle.Negative && ExistBuff(205, out var buff205))
        {
            buff205.DecreaseLevel();
            if (buff205.Level == 0) RemoveBuff(205);
        }
        else if (ExistBuff(buff, out var foundBuff))
        {
            foundBuff.IncreaseLevel(buff.Level);
        }
        else buffs.Add(buff);
        EffectUpdate();
    }
    public int CallAttack(int val)
    {
        float rate = AttackRate;

        if (ExistBuff(202, out var buff202))
        {
            buff202.DecreaseLevel();
            if (buff202.Level == 0) RemoveBuff(202);
        }
        if (ExistBuff(305, out var buff305))
        {
            StealedCoins += creature.gc.player.ReduceCoins(10 * buff305.Level);
        }

        EffectUpdate();
        //Debug.Log("attack rate"+rate+"damage"+(int)(rate*val));
        return (int)(rate * val);
    }
    public int CallDefence(int val)
    {
        float rate = DefenceRate;

        if (ExistBuff(201, out var buff201) && (int)(rate * val) > creature.shield)
        {
            buff201.DecreaseLevel();
            if (buff201.Level == 0) RemoveBuff(201);
            return 0;
        }
        if (ExistBuff(203, out var buff203))
        {
            buff203.DecreaseLevel();
            if (buff203.Level == 0) RemoveBuff(203);
        }
        if (ExistBuff(107)) creature.gc.player.ReduceHP((int)(rate * val) / 5);

        EffectUpdate();
        //Debug.Log("defence rate"+rate+"damage"+(int)(rate*val));
        return (int)(rate * val);
    }
    public int CallPlayCard(Card card)
    {
        if (ExistBuff(110) && card.type == CardType.Attack) return -1;
        if (ExistBuff(111) && card.type != CardType.Attack) return -1;

        int executeTimes = 1;

        if (ExistBuff(204, out var buff204))
        {
            buff204.DecreaseLevel();
            if (buff204.Level == 0) RemoveBuff(204);
            executeTimes = 2;
        }

        if (ExistBuff(303, out var buff303))
        {
            creature.gc.AllAttack(buff303.Level);
        }
        if (ExistBuff(304, out var buff304))
        {
            creature.gc.player.ReduceHP(buff304.Level);
        }

        EffectUpdate();
        return executeTimes;
    }
    public void EffectUpdate()
    {
        AttackRate = 1f;
        DefenceRate = 1f;
        ExtraCard = 0;
        ExtraSP = 0;

        foreach (var buff in buffs)
        {
            switch (buff.id)
            {
                case 101: AttackRate += 0.25f; break;
                case 102: DefenceRate -= 0.25f; break;
                case 103: AttackRate -= 0.25f; break;
                case 104: DefenceRate += 0.25f; break;
                case 202: AttackRate += 0.5f; break;
                case 203: DefenceRate -= 0.5f; break;
                case 306: ExtraCard += buff.Level; break;
                case 307: ExtraCard -= buff.Level; break;
                case 308: ExtraSP += buff.Level; break;
                case 309: ExtraSP -= buff.Level; break;
                case 310: ExtraCard += buff.Level; break;
                case 311: ExtraCard -= buff.Level; break;
                case 312: ExtraSP += buff.Level; break;
                case 313: ExtraSP -= buff.Level; break;
                default: break;
            }
        }
        if (Status==BuffContainerStatus.Inadequate)
        {
            AttackRate *= 2;
            DefenceRate *= 2;
        }
        if(ExistBuff(118))
        {
            AttackRate *= 3;
            DefenceRate *= 0;
        }
    }
    public void RoundUpdate()
    {
        if (ExistBuff(105)) creature.AddHP(creature.hpLimit / 20);
        if (ExistBuff(106)) creature.ReduceHP(creature.hpLimit / 20);
        if (ExistBuff(117)) creature.ap *= 2;
        if (ExistBuff(301, out var buff301)) creature.AddShield(buff301.Level);
        if (ExistBuff(302, out var buff302)) creature.AddShield(buff302.Level);

        List<int> buffToRemove = new();

        foreach (var buff in buffs)
        {
            if (buff.Type != BuffType.Round) continue;

            buff.ChangeLevel(buff.Level - 1);
            if (buff.Level <= 0) buffToRemove.Add(buff.id);
        }
        foreach (var id in buffToRemove)
        {
            RemoveBuff(id);
        }

        EffectUpdate();
    }
    public void Clarify()
    {
        creature.gc.PlayAudio(creature.gc.sfxRecover);
        List<int> buffToRemove = new();

        foreach (var buff in buffs)
        {
            if (buff.Style != BuffStyle.Negative) continue;
            buffToRemove.Add(buff.id);
        }
        foreach (var id in buffToRemove)
        {
            RemoveBuff(id);
        }

        EffectUpdate();
    }
    private void RemoveBuff(int id, bool execute = true)
    {
        foreach (var buff in buffs)
        {
            if (buff.id != id) continue;
            buffs.Remove(buff);
            break;
        }
        if (!execute) return;
        switch (id)
        {
            case 108: RemoveBuff(301); break;
            case 112: RemoveBuff(306); break;
            case 113: RemoveBuff(307); break;
            case 114: RemoveBuff(308); break;
            case 115: RemoveBuff(309); break;
            case 116:
                creature.gc.player.ReduceHP(creature.hp);
                creature.ReduceHP(creature.hp);
                break;
            case 118:
                creature.hp=0; break;
            default: break;
        }
    }
    public bool ExistBuff(int id)
    {
        foreach (var buff in buffs)
        {
            if (buff.id != id) continue;
            return true;
        }
        return false;
    }
    private bool ExistBuff(Buff buff)
    {
        return ExistBuff(buff.id);
    }
    private bool ExistBuff(int id, out Buff foundBuff)
    {
        foundBuff = null;

        foreach (var buff in buffs)
        {
            if (buff.id != id) continue;

            foundBuff = buff;
            return true;
        }

        return false;
    }
    private bool ExistBuff(Buff buff, out Buff foundBuff)
    {
        var result = ExistBuff(buff.id, out var foundBuff1);
        foundBuff = foundBuff1;
        return result;
    }
    public int GetLevel(int id)
    {
        if (ExistBuff(id, out var buff)) return buff.Level;
        return 0;
    }
}
