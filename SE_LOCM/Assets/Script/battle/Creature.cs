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
    public BuffContainer buffContainer;

    public virtual void AddHP(int val)
    {
        return;
    }
    public virtual void ReduceHP(int val)
    {
        return;
    }
}
