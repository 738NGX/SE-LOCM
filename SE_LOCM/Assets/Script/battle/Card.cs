using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System;
[System.Serializable]

public class Card
{
    public int id;
    public int cost;                    // 消耗
    public CardType type;               // 属性
    public CardRarity rarity;           // 稀有度
    public bool isPlused;               // 是否强化
    public bool disposable;             // 是否一次性
    public int playTimes;               // 打出次数
    public CardDisplayInfo displayInfo; // UI显示信息

    public Card(int id, bool isPlused = false)
    {
        this.id = id;
        this.isPlused = isPlused;
        playTimes = 0;
        ReadCardData(isPlused);
    }
    public void Play()
    {
        playTimes++;
        if (id == 114)
        {
            string oldContent = !isPlused ? (9 + (playTimes - 1) * 3).ToString() : (12 + (playTimes - 1) * 4).ToString();
            string newContent = !isPlused ? (9 + playTimes * 3).ToString() : (12 + playTimes * 4).ToString();
            displayInfo.effect = displayInfo.effect.Replace(oldContent, newContent);
        }
    }
    public void Upgrade()
    {
        if (isPlused) return;
        isPlused = true;
        ReadCardData(true);
    }
    public (int, bool) Export() { return (id, isPlused); }
    private void ReadCardData(bool _isPlused)
    {
        if (CardDatabase.data.TryGetValue(id, out var info))
        {
            displayInfo = info.Copy();
            cost = info.cost switch
            {
                "无" => -1,
                "零" => 0,
                "壹" => 1,
                "贰" => 2,
                "叁" => 3,
                "壹-" => !_isPlused ? 1 : 0,
                "贰-" => !_isPlused ? 2 : 1,
                "叁-" => !_isPlused ? 3 : 2,
                _ => 0
            };
            type = info.type switch
            {
                "攻击" => CardType.Attack,
                "锦囊" => CardType.Spell,
                "装备" => CardType.Equip,
                "谜题" => CardType.Quiz,
                _ => CardType.Attack,
            };
            rarity = info.rarity switch
            {
                "基础" => CardRarity.Base,
                "普通" => CardRarity.Ordinary,
                "稀有" => CardRarity.Rare,
                "史诗" => CardRarity.Epic,
                _ => CardRarity.Ordinary,
            };
            disposable = info.disposable switch
            {
                "是" => true,
                "否" => false,
                "可变" => !_isPlused,
                _ => false,
            };
        }
        else Debug.LogError($"Card ID not found in database:{id}");
        displayInfo.cost = cost switch
        {
            -1 => "无",
            0 => "零",
            1 => "壹",
            2 => "贰",
            3 => "叁",
            _ => "零",
        };
        if (_isPlused)
        {
            displayInfo.name += "+";
            displayInfo.effect = displayInfo.plusedEffect;
            if (displayInfo.cost[^1] == '-') displayInfo.cost = displayInfo.cost[..^1];
        }
    }
}