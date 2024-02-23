using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public enum BookRarity{Ordinary,Rare,Epic};
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
                if(string.IsNullOrWhiteSpace(dataLines[i])) continue;
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
