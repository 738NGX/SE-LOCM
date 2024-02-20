using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public static class BookDatabase
{
    private static readonly TextAsset csvData=AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Database/bookdb.csv");
    public static Dictionary<int,BookDisplayInfo> data=new(){};
    static BookDatabase()
    {
        if (csvData!=null)
        {
            string[] dataLines=csvData.text.Split('\n');
            for(int i=1;i<dataLines.Length;i++)
            {
                if(dataLines.Length==0) continue;
                BookDisplayInfo entry=new(dataLines[i]);
                data.Add(entry.id,entry);
            }
        }
        else
        {
            Debug.LogError("CSV file not found");
        }
    }
}
