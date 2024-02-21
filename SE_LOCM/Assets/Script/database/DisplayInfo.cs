using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
};

public class BookDisplayInfo : DisplayInfo
{
    public string effect;
    public string introduction;
    public BookDisplayInfo(string rawData)
    {
        string[] values=rawData.Split(',');
        id=int.Parse(values[0]);
        name=values[1];
        effect=values[2];
        introduction=values[3];
    }
}