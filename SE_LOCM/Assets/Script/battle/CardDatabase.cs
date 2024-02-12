using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public enum CardType{Attack,Spell,Equip};           // 攻击 锦囊 装备
public enum CardRarity{Base,Ordinary,Rare};         // 基础 普通 稀有
public static class CardDatabase
{
    private static readonly TextAsset csvData=AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Script/battle/carddb.csv");
    public static Dictionary<int,CardDisplayInfo> data=new(){};
    static CardDatabase()
    {
        if (csvData!=null)
        {
            string[] dataLines=csvData.text.Split('\n');
            for(int i=1;i<dataLines.Length;i++)
            {
                CardDisplayInfo entry=new(dataLines[i]);
                data.Add(entry.id,entry);
            }
        }
        else
        {
            Debug.LogError("CSV file not found");
        }
    }
}
