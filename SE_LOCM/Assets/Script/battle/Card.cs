using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class CardDisplayInfo
{
    public int id;
    public string name;         // 牌名
    public string cost;         // 消耗
    public string type;         // 属性
    public string rarity;       // 稀有度
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
        effect=values[5];
        plusedEffect=values[6];
        quote=values[7].Replace("\\n", "\n");;
    }
};

public class Card
{
    public int id;
    public int cost;                    // 消耗
    public CardType type;               // 属性
    public CardRarity rarity;           // 稀有度
    public bool isPlused;               // 是否强化
    public CardDisplayInfo displayInfo; // UI显示信息

    public Card(int id,bool isPlused=false)
    {
        this.id=id;
        this.isPlused=isPlused;
        if(CardDatabase.data.TryGetValue(id,out var info))
        {
            displayInfo=info;
            switch(info.cost)
            {
                case "零": cost=0; break;
                case "壹": cost=1; break;
                case "贰": cost=2; break;
            }
            switch(info.type)
            {
                case "攻击": type=CardType.Attack; break;
                case "锦囊": type=CardType.Spell;  break;
                case "装备": type=CardType.Equip;  break;
            }
            switch(info.rarity)
            {
                case "基础": rarity=CardRarity.Base;        break;
                case "普通": rarity=CardRarity.Ordinary;    break;
                case "稀有": rarity=CardRarity.Rare;        break;
                case "史诗": rarity=CardRarity.Epic;        break;
            }
        }
        else Debug.LogError($"Card ID not found in database:{id}");
    }

    // 手牌打出时调用
    public void ExecuteAction()
    {
        // 根据ID映射到具体的函数或逻辑
        switch (id)
        {
            default:
                Debug.LogWarning("No action defined for this card ID: " + id);
                break;
        }
    }
}