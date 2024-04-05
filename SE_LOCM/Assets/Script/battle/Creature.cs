using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public GameController gc;
    public int hp;              // 体力值(HealthPoints)
    public int hpLimit;         // 体力值上限
    public int ap;              // 攻击力
    public int dp;              // 防御力
    public int shield;          // 护盾
    public BuffContainer buffContainer = new();

    private int roundCount;

    private void Start()
    {
        buffContainer.EffectUpdate();
    }
    private void Update()
    {
        if (roundCount != gc.roundCount)
        {
            roundCount = gc.roundCount;
            buffContainer.RoundUpdate();
        }
    }
    public virtual void AddHP(int val)
    {
        gc.PlayAudio(gc.sfxRecover);
        if (val < 1) return;
        if (hp + val >= hpLimit) hp = hpLimit;
        else hp += val;
    }
    public virtual void ReduceHP(int val)
    {
        gc.PlayAudio(gc.sfxHurt);
        int _val = buffContainer.CallDefence(val);
        if (_val < 1) return;
        if (hp + shield - _val <= 0) hp = 0;
        else if (_val > shield)
        {
            hp -= _val - shield;
            shield = 0;
        }
        else shield -= _val;
    }
    public virtual void AddShield(int val)
    {
        return;
    }
    public virtual void AddBuff(Buff buff)
    {
        switch (buff.Style)
        {
            case BuffStyle.Positive: gc.PlayAudio(gc.sfxBuff); break;
            case BuffStyle.Negative: gc.PlayAudio(gc.sfxDeuff); break;
            default: break;
        }
        buffContainer.AddBuff(buff);
    }
}
