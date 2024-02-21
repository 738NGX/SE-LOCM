using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public static class MapDatabase
{
    private static readonly TextAsset csvData=AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Database/mapdb.csv");
    public static Dictionary<int,MapNodeData> data=new(){};
    static MapDatabase()
    {
        if (csvData!=null)
        {
            string[] dataLines=csvData.text.Split('\n');
            for(int i=1;i<dataLines.Length;i++)
            {
                if(string.IsNullOrWhiteSpace(dataLines[i])) continue;
                MapNodeData entry=new(dataLines[i]);
                data.Add(entry.id,entry);
            }
        }
        else
        {
            Debug.LogError("CSV file not found");
        }
    }
}

public class MapNodeData
{
    public int id;
    public MapNodeType type;                                    // 节点类型
    public List<int> nextNodes;                                 // 下一节点
    public List<int> unlockCardsPool;                           // 解锁卡池
    private readonly int[] coinsRange=new int[2];               // 银币奖励范围
    public int rewardCardsNum{get;}                             // 卡牌奖励次数
    private readonly int[] rewardCardChance=new int[2];         // 卡牌奖励概率(%)
    private readonly int[] rewardBookChance=new int[2];         // 典籍奖励概率(%)

    public MapNodeData(string rawData)
    {
        string[] values=rawData.Split(',');
        id=int.Parse(values[0]);
        type=values[1] switch
        {
            "剧情"=>MapNodeType.Story,
            "休息"=>MapNodeType.Break,
            "商铺"=>MapNodeType.Shop,
            "宝箱"=>MapNodeType.Box,
            "未知"=>MapNodeType.Unknown,
            "敌人"=>MapNodeType.Enemy,
            "精英"=>MapNodeType.Senior,
            "Boss"=>MapNodeType.Boss,
            _=>MapNodeType.Unknown,
        };
        nextNodes=values[2].Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s=>int.Parse(s.Trim())).ToList();
        unlockCardsPool=values[3].Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s=>int.Parse(s.Trim())).ToList();
        coinsRange[0]=int.Parse(values[4]);
        coinsRange[1]=int.Parse(values[5]);
        rewardCardsNum=int.Parse(values[6]);
        rewardCardChance[0]=int.Parse(values[7]);
        rewardCardChance[1]=int.Parse(values[8]);
        rewardBookChance[0]=int.Parse(values[9]);
        rewardBookChance[1]=int.Parse(values[10]);
    }
    public int Level(){return id/100;}
    public int RewardCoins(){return Random.Range(coinsRange[0],coinsRange[1]+1);}
    public List<Card> RewardCards(List<Card> cardsPool,int num=3)
    {
        List<Card> ordinaryCards=new();
        List<Card> rareCards=new();
        List<Card> epicCards=new();
        List<Card> rewardCards=new();
        foreach(var card in cardsPool)
        {
            switch(card.rarity)
            {
                case CardRarity.Ordinary:ordinaryCards.Add(card); break;
                case CardRarity.Rare:rareCards.Add(card); break;
                case CardRarity.Epic:epicCards.Add(card); break;
            }
        } 
        for(int i=0;i<num;i++)
        {
            int seed=Random.Range(0,100);
            if(epicCards.Count>0 && seed<rewardCardChance[1])
            {
                rewardCards.Add(epicCards[Random.Range(0,epicCards.Count)]);
            }
            else if(rareCards.Count>0 && seed<rewardCardChance[0])
            {
                rewardCards.Add(rareCards[Random.Range(0,rareCards.Count)]);
            }
            else
            {
                rewardCards.Add(ordinaryCards[Random.Range(0,ordinaryCards.Count)]);
            }
        }

        return rewardCards;
    }
    public Book RewardBook(List<Book> achievedBooks)
    {
        if(Random.Range(0,100)<rewardBookChance[1])
        {
            int seed=Random.Range(10,35);
            return achievedBooks.Contains(new Book(seed)) ? null : new Book(seed);
        }
        else if(Random.Range(0,100)<rewardBookChance[0])
        {
            int seed=Random.Range(21,35);
            return achievedBooks.Contains(new Book(seed)) ? null : new Book(seed);
        }
        else return null;
    }
}
