using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;                  // 体力值
    public int ap;                  // 基础攻击力
    public int dp;                  // 基础防御力
    public float attack_factor;     // 攻击倍率
    public float defence_factor;    // 防御倍率
    public int hp_limit;           // 体力上限
    void Start()
    {
        hp_limit=hp;
        attack_factor=1;
        defence_factor=1;
    }
}
