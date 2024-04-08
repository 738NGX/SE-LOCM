using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public static class BuffDatabase
{
    private static readonly TextAsset csvData=Resources.Load<TextAsset>("Database/buffdb");
    public static Dictionary<int,Buff> data=new(){};
    static BuffDatabase()
    {
        if (csvData!=null)
        {
            string[] dataLines=csvData.text.Split('\n');
            for(int i=1;i<dataLines.Length;i++)
            {
                if(string.IsNullOrWhiteSpace(dataLines[i])) continue;
                Buff entry=new(dataLines[i]);
                data.Add(entry.id,entry);
            }
        }
        else
        {
            Debug.LogError("CSV file not found");
        }
    }
}
