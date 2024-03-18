using System.Collections.Generic;
using Fungus;
using UnityEngine;

public enum BuffType{Round,Consume,Ability};
public enum BuffStyle{Positive,Negative,Neutral}
public class Buff
{
    public int id;
    public string name;
    public BuffType Type{get;}
    public BuffStyle Style{get;}
    public string Effect{get; private set;}

    public int Level{get; private set;}

    public Buff(int id,int level)
    {
        this.id=id;
        name=BuffDatabase.data[id].name;
        Effect=BuffDatabase.data[id].Effect;
        Type=BuffDatabase.data[id].Type;
        Style=BuffDatabase.data[id].Style;
        ChangeLevel(level);
    }
    public Buff(string rawData)
    {
        string[] values=rawData.Split(',');
        id=int.Parse(values[0]);
        name=values[1];
        Effect=values[3];
        Level=0;
        Type=(id/100) switch
        {
            1=>BuffType.Round,
            2=>BuffType.Consume,
            3=>BuffType.Ability,
            _=>BuffType.Round
        };
        Style=values[2] switch
        {
            "正面"=>BuffStyle.Positive,
            "负面"=>BuffStyle.Negative,
            "中性"=>BuffStyle.Neutral,
            _=>BuffStyle.Neutral
        };
    }

    public void ChangeLevel(int newLevel)
    {
        Level=newLevel;
        Effect=BuffDatabase.data[id].Effect;
        Effect=Effect.Replace("X",Level.ToString());
    }
    public void DecreaseLevel(int val=1)
    {
        int newLevel=val>Level ? 0 : Level-val;
        ChangeLevel(newLevel);
    }
    public void IncreaseLevel(int val=1)
    {
        int newLevel=Level+val;
        ChangeLevel(newLevel);
    }
}