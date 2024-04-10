using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public static class EnemyDatabase
{
    private static readonly TextAsset csvData=Resources.Load<TextAsset>("Database/enemydb");
    public static Dictionary<int,EnemyInfo> data=new(){};
    static EnemyDatabase()
    {
        if (csvData!=null)
        {
            string[] dataLines=csvData.text.Split('\n');
            for(int i=1;i<dataLines.Length;i++)
            {
                if(string.IsNullOrWhiteSpace(dataLines[i])) continue;
                EnemyInfo entry=new(dataLines[i]);
                data.Add(entry.id,entry);
            }
        }
        else
        {
            Debug.LogError("CSV file not found");
        }
    }
}

public static class CharacterDatabase
{
    private static readonly TextAsset csvData=Resources.Load<TextAsset>("Database/chardb");
    public static Dictionary<int,CharacterInfo> data=new(){};
    static CharacterDatabase()
    {
        if (csvData!=null)
        {
            string[] dataLines=csvData.text.Split('\n');
            for(int i=1;i<dataLines.Length;i++)
            {
                if(string.IsNullOrWhiteSpace(dataLines[i])) continue;
                CharacterInfo entry=new(dataLines[i]);
                data.Add(entry.id,entry);
            }
        }
        else
        {
            Debug.LogError("CSV file not found");
        }
    }
}
