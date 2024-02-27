using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public enum CardType{Attack,Spell,Equip,Quiz};           // 攻击 锦囊 装备
public enum CardRarity{Base,Ordinary,Rare,Epic};         // 基础 普通 稀有 史诗
public static class CardDatabase
{
    private static readonly TextAsset csvData=AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Database/carddb.csv");
    public static Dictionary<int,CardDisplayInfo> data=new(){};
    static CardDatabase()
    {
        if (csvData!=null)
        {
            string[] dataLines=csvData.text.Split('\n');
            for(int i=1;i<dataLines.Length;i++)
            {
                if(string.IsNullOrWhiteSpace(dataLines[i])) continue;
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