using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public static class ProblemDatabase
{
    private static readonly TextAsset csvData=Resources.Load<TextAsset>("Database/problemdb");
    public static Dictionary<int,ProblemInfo> data=new(){};
    static ProblemDatabase()
    {
        if (csvData!=null)
        {
            string[] dataLines=csvData.text.Split('\n');
            for(int i=1;i<dataLines.Length;i++)
            {
                if(string.IsNullOrWhiteSpace(dataLines[i])) continue;
                ProblemInfo entry=new(dataLines[i]);
                data.Add(entry.id,entry);
            }
        }
        else
        {
            Debug.LogError("CSV file not found");
        }
    }
}
