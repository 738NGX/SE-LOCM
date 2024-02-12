using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 准备阶段、摸牌阶段、出牌阶段、弃牌阶段、敌人阶段、结束阶段
public enum GameStage{Pre,Draw,Play,Discard,Enemy,End};

public class GameController : MonoBehaviour
{
    public DrawPile drawPile;           // 摸牌堆
    public HandCards handCards;         // 手牌堆
    public DiscardPile discardPile;     // 弃牌堆
    public Player player;               // 玩家
    public List<Enemy> enemies;         // 敌人
    public DisplayController dc;        // 显示控制
    public HandCardsUI hcui;            // 手牌显示控制

    public int roundCount;              // 回合数
    public GameStage gameStage;         // 游戏阶段
    
    void Start()
    {
        roundCount=1;
        StartCoroutine(dc.AnimatePanelAndText(new(){"战","斗","开","始"}));
        gameStage=GameStage.Pre;
    }
    void Update()
    {
        if(dc.isAnimating) return;
        if(gameStage==GameStage.Pre)
        {
            StartCoroutine(dc.AnimatePanelAndText(new(){"玩","家","回","合"}));
            // 结束准备阶段
            gameStage=GameStage.Draw;
        }
        else if(gameStage==GameStage.Draw)
        {
            // 默认情况下可以摸5张牌
            int drawNum=5;
            handCards.DrawCards(drawPile.DrawCards(drawNum));
            hcui.DrawCards();
            // 结束摸牌阶段
            gameStage=GameStage.Play;
        }
        else if(gameStage==GameStage.Play)
        {
            if(handCards.handCards.Count==0) gameStage=GameStage.Discard;
        }
    }
}
