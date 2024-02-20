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
};

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

    public Card(int id,bool isPlused=false)
    {
        this.id=id;
        this.isPlused=isPlused;
        playTimes=0;
        if(CardDatabase.data.TryGetValue(id,out var info))
        {
            displayInfo=info;
            cost=info.cost switch
            {
                "零" => 0,
                "壹" => 1,
                "贰" => 2,
                _ => 0
            };
            type=info.type switch
            {
                "攻击" => CardType.Attack,
                "锦囊" => CardType.Spell,
                "装备" => CardType.Equip,
                _ => CardType.Attack
            };
            rarity=info.rarity switch
            {
                "基础" => CardRarity.Base,
                "普通" => CardRarity.Ordinary,
                "稀有" => CardRarity.Rare,
                "史诗" => CardRarity.Epic,
                _ => CardRarity.Ordinary
            };
            disposable=info.disposable switch
            {
                "是" => true,
                "否" => false,
                "可变" => !isPlused,
                _ => false
            };
        }
        else Debug.LogError($"Card ID not found in database:{id}");
        if(isPlused)
        {
            displayInfo.name+="+";
            displayInfo.effect=displayInfo.plusedEffect;
        }
    }

    public void Play()
    {
        playTimes++;
        if(id==114)
        {
            string oldContent=!isPlused ? (9+(playTimes-1)*3).ToString() : (12+(playTimes-1)*4).ToString();
            string newContent=!isPlused ? (9+playTimes*3).ToString() : (12+playTimes*4).ToString();
            displayInfo.effect=displayInfo.effect.Replace(oldContent,newContent);
        }
    }
}