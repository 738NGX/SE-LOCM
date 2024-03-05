using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class BuffContainer : MonoBehaviour
{
    public Creature creature;
    public List<Buff> buffs=new();
    public float AttackRate {get; private set;}
    public float DefenceRate {get; private set;}
    private int roundCount;

    public void AddBuff(Buff buff)
    {
        if(ExistBuff(buff,out var foundBuff))
        {
            foundBuff.ChangeLevel(foundBuff.Level+buff.Level);
        }
        else
        {
            buffs.Add(buff);
        }
    }
    
    private void Start()
    {
        EffectUpdate();
    }
    private void Update()
    {
        if(roundCount!=creature.gc.roundCount) RoundUpdate();
    }
    private void EffectUpdate()
    {
        AttackRate=1f;
        DefenceRate=1f;
        foreach(var buff in buffs)
        {
            switch(buff.id)
            {
                case 101: AttackRate+=0.25f; break;
                case 102: DefenceRate+=0.25f; break;
                case 103: AttackRate-=0.25f; break;
                case 104: DefenceRate-=0.25f; break;
                case 202: AttackRate+=0.5f; break;
                case 203: DefenceRate+=0.5f; break;
                default: break;
            }
        }
    }
    private void RoundUpdate()
    {
        roundCount=creature.gc.roundCount;
        if(ExistBuff(105)) creature.AddHP(creature.hpLimit/20);
        if(ExistBuff(106)) creature.ReduceHP(creature.hpLimit/20);
        
        foreach(var buff in buffs)
        {
            if(buff.Type!=BuffType.Round) continue;
            
            buff.ChangeLevel(buff.Level-1);
            if(buff.Level==0)
            { 
                switch(buff.id)
                {
                    case 108: RemoveBuff(301); break;
                    case 112: RemoveBuff(306); break;
                    case 113: RemoveBuff(307); break;
                    case 114: RemoveBuff(308); break;
                    case 115: RemoveBuff(309); break;
                    case 116: 
                        creature.gc.player.ReduceHP(creature.hpLimit);
                        creature.ReduceHP(creature.hp);
                        break;
                    default: break;
                }
                buffs.Remove(buff);
            }
        }

        EffectUpdate();
    }
    private void RemoveBuff(int id)
    { 
        foreach(var buff in buffs)
        {
            if(buff.id!=id) continue;
            buffs.Remove(buff);
            break;
        }
    }
    public bool ExistBuff(int id)
    { 
        foreach(var buff in buffs)
        {
            if(buff.id!=id) continue;
            return true;
        }
        return false;
    }
    private bool ExistBuff(Buff buff)
    { 
        return ExistBuff(buff.id);
    }
    private bool ExistBuff(Buff _buff,out Buff foundBuff)
    {
        foundBuff=null;
        
        foreach(var buff in buffs)
        {
            if(buff.id!=_buff.id) continue;
            
            foundBuff=buff;
            return true;
        }
        
        return false;
    }
}
