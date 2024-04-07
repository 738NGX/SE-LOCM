using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public static class MapDatabase
{
    private static readonly TextAsset csvData=Resources.Load<TextAsset>("Database/mapdb");
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

public static class EnemyNodeDatabase
{
    private static readonly TextAsset csvData=Resources.Load<TextAsset>("Database/enemynodedb");
    public static Dictionary<int,EnemyNodeData> data=new(){};
    static EnemyNodeDatabase()
    {
        if (csvData!=null)
        {
            string[] dataLines=csvData.text.Split('\n');
            for(int i=1;i<dataLines.Length;i++)
            {
                if(string.IsNullOrWhiteSpace(dataLines[i])) continue;
                EnemyNodeData entry=new(dataLines[i]);
                data.Add(entry.id,entry);
            }
        }
        else
        {
            Debug.LogError("CSV file not found");
        }
    }
}

public static class MapNodeIdToStorySceneDatabase
{
    public static Dictionary<int,string> data=new()
    {
        {100,"Scenes/story/s1/s1-01"},{101,"Scenes/story/s1/s1-02"},
        {120,"Scenes/story/s1/s1-04"},{132,"Scenes/story/s1/s1-06"},{200,"Scenes/story/s2/s2-01"},
        {201,"Scenes/story/s2/s2-02"},{209,"Scenes/story/s2/s2-03"},{220,"Scenes/story/s2/s2-04"},
        {228,"Scenes/story/s2/s2-05"},{300,"Scenes/story/s3/s3-01"},{301,"Scenes/story/s3/s3-02"},
        {320,"Scenes/story/s3/s3-03"},{332,"Scenes/story/s3/s3-04"},{400,"Scenes/story/s4/s4-01"},
        {404,"Scenes/story/s4/s4-02"},{415,"Scenes/story/s4/s4-03"},{423,"Scenes/story/s4/s4-04"},
        {430,"Scenes/story/s4/s4-05"},{500,"Scenes/story/s5/s5-01"},{501,"Scenes/story/s5/s5-02"},
        {509,"Scenes/story/s5/s5-03"},{524,"Scenes/story/s5/s5-04"},{529,"Scenes/story/s5/s5-05"},
        {600,"Scenes/story/s6/s6-01"},{606,"Scenes/story/s6/s6-02"},{623,"Scenes/story/s6/s6-03"},
        {632,"Scenes/story/s6/s6-04"},{700,"Scenes/story/s7/s7-01"},{702,"Scenes/story/s7/s7-02"},
        {714,"Scenes/story/s7/s7-03"},{730,"Scenes/story/s7/s7-04"},{800,"Scenes/story/s8/s8-01"},
        {805,"Scenes/story/s8/s8-02"},{820,"Scenes/story/s8/s8-03"},{830,"Scenes/story/s8/s8-04"},
        {900,"Scenes/story/s9/s9-01"},{902,"Scenes/story/s9/s9-02"},{916,"Scenes/story/s9/s9-03"},
        {920,"Scenes/story/s9/s9-04"},{932,"Scenes/story/s9/s9-05"},
    };
}

public class MapNodeData
{
    public int id;
    public MapNodeType type;                                    // 节点类型
    public List<int> nextNodes;                                 // 下一节点
    public List<int> unlockCardsPool;                           // 解锁卡池
    private readonly int[] coinsRange=new int[2];               // 银币奖励范围
    public int RewardCardsNum{get;}                             // 卡牌奖励次数
    public int Level{get{return id/100;}}
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
        RewardCardsNum=int.Parse(values[6]);
        rewardCardChance[0]=int.Parse(values[7]);
        rewardCardChance[1]=int.Parse(values[8]);
        rewardBookChance[0]=int.Parse(values[9]);
        rewardBookChance[1]=int.Parse(values[10]);
    }
    public int RewardCoins(){return Random.Range(coinsRange[0],coinsRange[1]+1);}
    public List<Card> RewardCards(List<Card> cardsPool,int num=3)
    {
        List<Card> ordinaryCards=new();
        List<Card> rareCards=new();
        List<Card> epicCards=new();
        List<Card> rewardCards=new();

        foreach (var card in cardsPool)
        {
            switch(card.rarity)
            {
                case CardRarity.Ordinary: ordinaryCards.Add(card); break;
                case CardRarity.Rare: rareCards.Add(card); break;
                case CardRarity.Epic: epicCards.Add(card); break;
            }
        }

        int uniqueCardsCount=ordinaryCards.Distinct().Count()+rareCards.Distinct().Count()+epicCards.Distinct().Count();
        num=System.Math.Min(num,uniqueCardsCount);

        for(int i=0;i<num;i++)
        {
            Card selectedCard=null;
            while(selectedCard==null)
            {
                int seed=Random.Range(0,100);
                if(epicCards.Count>0 && seed<rewardCardChance[1])
                {
                    selectedCard=epicCards[Random.Range(0,epicCards.Count)];
                }
                else if(rareCards.Count>0 && seed<rewardCardChance[0])
                {
                    selectedCard=rareCards[Random.Range(0,rareCards.Count)];
                }
                else if(ordinaryCards.Count>0)
                {
                    selectedCard=ordinaryCards[Random.Range(0,ordinaryCards.Count)];
                }
                if(rewardCards.Contains(selectedCard))
                {
                    selectedCard=null;
                }
            }

            rewardCards.Add(selectedCard);

            switch(selectedCard.rarity)
            {
                case CardRarity.Ordinary: ordinaryCards.Remove(selectedCard); break;
                case CardRarity.Rare: rareCards.Remove(selectedCard); break;
                case CardRarity.Epic: epicCards.Remove(selectedCard); break;
            }
        }

        return rewardCards;
    }
    public Book RewardBook(List<Book> achievedBooks)
    {
        if(Random.Range(0,100)<rewardBookChance[1])
        {
            int seed=Random.Range(10,21);
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

public class EnemyNodeData
{
    public int id;
    public List<List<int>> EnemyPool{get; private set;}

    public EnemyNodeData(string rawData)
    {
        EnemyPool=new();
        string[] values=rawData.Split(',');
        id=int.Parse(values[0]);

        for(int i=0;i<int.Parse(values[1]);i++)
        {
            List<int> enemyPoolItem=System.Array.ConvertAll(values[2+i].Split(';'),s=>int.Parse(s)).ToList();
            EnemyPool.Add(enemyPoolItem);
        }
    }
    public EnemyNodeData(int id)
    {
        int usingId=EnemyNodeDatabase.data.Keys.Contains(id) ? id : 103;
        this.id=id;
        EnemyPool=EnemyNodeDatabase.data[usingId].EnemyPool;
    }
    public List<int> GetEnemyIds()
    {
        int seed=Random.Range(0,EnemyPool.Count);
        return EnemyPool[seed];
    }
}