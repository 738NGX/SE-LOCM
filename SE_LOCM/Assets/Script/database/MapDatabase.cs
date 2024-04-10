using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public static class MapDatabase
{
    private static readonly TextAsset csvData = Resources.Load<TextAsset>("Database/mapdb");
    public static Dictionary<int, MapNodeData> data = new() { };
    static MapDatabase()
    {
        if (csvData != null)
        {
            string[] dataLines = csvData.text.Split('\n');
            for (int i = 1; i < dataLines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(dataLines[i])) continue;
                MapNodeData entry = new(dataLines[i]);
                data.Add(entry.id, entry);
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
    private static readonly TextAsset csvData = Resources.Load<TextAsset>("Database/enemynodedb");
    public static Dictionary<int, EnemyNodeData> data = new() { };
    static EnemyNodeDatabase()
    {
        if (csvData != null)
        {
            string[] dataLines = csvData.text.Split('\n');
            for (int i = 1; i < dataLines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(dataLines[i])) continue;
                EnemyNodeData entry = new(dataLines[i]);
                data.Add(entry.id, entry);
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
    public static Dictionary<int, string> nameData = new()
    {
        {000,"序幕"},
        {100,"第一章第一幕"},{101,"第一章第二幕"},{102,"第一章第三幕"},
        {120,"第一章第四幕"},{132,"第一章第五幕"},{199,"第一章终幕"},
        {200,"第二章第一幕"},{201,"第二章第二幕"},{209,"第二章第三幕"},
        {220,"第二章第四幕"},{228,"第二章第五幕"},{299,"第二章终幕"},
        {300,"第三章第一幕"},{301,"第三章第二幕"},{320,"第三章第三幕"},
        {332,"第三章第四幕"},{399,"第三章终幕"},
        {400,"第四章第一幕"},{404,"第四章第二幕"},{415,"第四章第三幕"},
        {423,"第四章第四幕"},{430,"第四章第五幕"},{499,"第四章终幕"},
        {500,"第五章第一幕"},{501,"第五章第二幕"},{509,"第五章第三幕"},
        {524,"第五章第四幕"},{529,"第五章第五幕"},{599,"第五章终幕"},
        {600,"第六章第一幕"},{606,"第六章第二幕"},{623,"第六章第三幕"},
        {632,"第六章第四幕"},{699,"第六章终幕"},
        {700,"第七章第一幕"},{702,"第七章第二幕"},{714,"第七章第三幕"},
        {730,"第七章第四幕"},{799,"第七章终幕"},
        {800,"第八章第一幕"},{805,"第八章第二幕"},{820,"第八章第三幕"},
        {830,"第八章第四幕"},{899,"第八章终幕"},
        {900,"第九章第一幕"},{902,"第九章第二幕"},{916,"第九章第三幕"},
        {920,"第九章第四幕"},{932,"第九章第五幕"},{999,"第九章终幕"},
    };
    public static Dictionary<int, string> pathData = new()
    {
        {000,"Scenes/story/s0-00"},
        {100,"Scenes/story/s1/s1-01"},{101,"Scenes/story/s1/s1-02"},{102,"Scenes/story/s1/s1-03"},
        {120,"Scenes/story/s1/s1-04"},{132,"Scenes/story/s1/s1-05"},{200,"Scenes/story/s2/s2-01"},
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
        {199,"Scenes/story/s1/s1-07"},{299,"Scenes/story/s2/s2-06"},{399,"Scenes/story/s3/s3-05"},
        {499,"Scenes/story/s4/s4-06"},{599,"Scenes/story/s5/s5-06"},{699,"Scenes/story/s6/s6-05"},
        {799,"Scenes/story/s7/s7-05"},{899,"Scenes/story/s8/s8-05"},{999,"Scenes/story/s9/s9-06"},
    };
}

public class MapNodeData
{
    public int id;
    public MapNodeType type;                                    // 节点类型
    public List<int> nextNodes;                                 // 下一节点
    public List<int> unlockCardsPool;                           // 解锁卡池
    private readonly int[] coinsRange = new int[2];               // 银币奖励范围
    public int RewardCardsNum { get; }                             // 卡牌奖励次数
    public int Level { get { return id / 100; } }
    private readonly int[] rewardCardChance = new int[2];         // 卡牌奖励概率(%)
    private readonly int[] rewardBookChance = new int[2];         // 典籍奖励概率(%)

    public MapNodeData(string rawData)
    {
        string[] values = rawData.Split(',');
        id = int.Parse(values[0]);
        type = values[1] switch
        {
            "剧情" => MapNodeType.Story,
            "休息" => MapNodeType.Break,
            "商铺" => MapNodeType.Shop,
            "宝箱" => MapNodeType.Box,
            "未知" => MapNodeType.Unknown,
            "敌人" => MapNodeType.Enemy,
            "精英" => MapNodeType.Senior,
            "Boss" => MapNodeType.Boss,
            _ => MapNodeType.Unknown,
        };
        nextNodes = values[2].Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => int.Parse(s.Trim())).ToList();
        unlockCardsPool = values[3].Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => int.Parse(s.Trim())).ToList();
        coinsRange[0] = int.Parse(values[4]);
        coinsRange[1] = int.Parse(values[5]);
        RewardCardsNum = int.Parse(values[6]);
        rewardCardChance[0] = int.Parse(values[7]);
        rewardCardChance[1] = int.Parse(values[8]);
        rewardBookChance[0] = int.Parse(values[9]);
        rewardBookChance[1] = int.Parse(values[10]);
    }
    public int RewardCoins() { return Random.Range(coinsRange[0], coinsRange[1] + 1); }
    public List<Card> RewardCards(List<Card> cardsPool, int num = 3)
    {
        List<Card> ordinaryCards = new();
        List<Card> rareCards = new();
        List<Card> epicCards = new();
        List<Card> rewardCards = new();

        foreach (var card in cardsPool)
        {
            switch (card.rarity)
            {
                case CardRarity.Ordinary: ordinaryCards.Add(card); break;
                case CardRarity.Rare: rareCards.Add(card); break;
                case CardRarity.Epic: epicCards.Add(card); break;
            }
        }

        int uniqueCardsCount = ordinaryCards.Distinct().Count() + rareCards.Distinct().Count() + epicCards.Distinct().Count();
        num = System.Math.Min(num, uniqueCardsCount);

        for (int i = 0; i < num; i++)
        {
            Card selectedCard = null;
            while (selectedCard == null)
            {
                int seed = Random.Range(0, 100);
                if (epicCards.Count > 0 && seed < rewardCardChance[1])
                {
                    selectedCard = epicCards[Random.Range(0, epicCards.Count)];
                }
                else if (rareCards.Count > 0 && seed < rewardCardChance[0])
                {
                    selectedCard = rareCards[Random.Range(0, rareCards.Count)];
                }
                else if (ordinaryCards.Count > 0)
                {
                    selectedCard = ordinaryCards[Random.Range(0, ordinaryCards.Count)];
                }
                if (rewardCards.Contains(selectedCard))
                {
                    selectedCard = null;
                }
            }

            rewardCards.Add(selectedCard);

            switch (selectedCard.rarity)
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
        switch (id)
        {
            case 133: return new Book(1);
            case 231: return new Book(2);
            case 335: return new Book(3);
            case 431: return new Book(4);
            case 532: return new Book(5);
            case 633: return new Book(6);
            case 732: return new Book(7);
            case 833: return new Book(8);
            case 933: return new Book(9);
            default: break;
        }
        if (Random.Range(0, 100) < rewardBookChance[1])
        {
            int seed = Random.Range(10, 21);
            return new Book(seed);
        }
        else if (Random.Range(0, 100) < rewardBookChance[0])
        {
            int seed = Random.Range(21, 35);
            return new Book(seed);
        }
        else return null;
    }
}

public class EnemyNodeData
{
    public int id;
    public List<List<int>> EnemyPool { get; private set; }
    public string Background { get; private set; }

    public EnemyNodeData(string rawData)
    {
        EnemyPool = new();
        string[] values = rawData.Split(',');
        id = int.Parse(values[0]);

        for (int i = 0; i < int.Parse(values[1]); i++)
        {
            List<int> enemyPoolItem = System.Array.ConvertAll(values[2 + i].Split(';'), s => int.Parse(s)).ToList();
            EnemyPool.Add(enemyPoolItem);
        }

        Background = values[7] switch
        {
            "荒野" => "Background/battle/b01",
            "农田" => "Background/battle/b02",
            "村落" => "Background/battle/b03",
            "码头" => "Background/battle/b04",
            "山麓" => "Background/battle/b05",
            "殿堂" => "Background/battle/b06",
            _ => "Background/battle/b01",
        };
    }
    public EnemyNodeData(int id)
    {
        int usingId = EnemyNodeDatabase.data.Keys.Contains(id) ? id : 103;
        this.id = id;
        EnemyPool = EnemyNodeDatabase.data[usingId].EnemyPool;
        Background = EnemyNodeDatabase.data[usingId].Background;
    }
    public List<int> GetEnemyIds()
    {
        int seed = Random.Range(0, EnemyPool.Count);
        return EnemyPool[seed];
    }
}