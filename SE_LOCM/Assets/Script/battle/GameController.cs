using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    public AudioClip sfxStart;          // 战斗开始音效
    public AudioClip sfxAttack;          // 战斗开始音效

    public int roundCount;              // 回合数
    public GameStage gameStage;         // 游戏阶段
    
    
    public void PlayAudio(AudioClip clip)
    {
        AudioSource source=gameObject.AddComponent<AudioSource>();
        source.clip=clip;
        source.Play();
    }
    void Start()
    {
        roundCount=1;

        PlayAudio(sfxStart);

        StartCoroutine(dc.AnimatePanelAndText(new(){"战","斗","开","始"},1f));
        gameStage=GameStage.Pre;
    }
    void Update()
    {
        if(dc.isAnimating) return;  // 系统动画时不进行操作
        if(gameStage==GameStage.Pre)
        {
            StartCoroutine(dc.AnimatePanelAndText(new(){"玩","家","回","合"}));
            player.sp=player.sp_init;
            player.shield=0;
            // 结束准备阶段
            gameStage=GameStage.Draw;
        }
        else if(gameStage==GameStage.Draw)
        {
            // 默认情况下可以摸5张牌
            int drawNum=5;
            DrawCards(drawNum);
            hcui.DrawCards();
            // 结束摸牌阶段
            gameStage=GameStage.Play;
        }
        else if(gameStage==GameStage.Play)
        {
            // 玩家操作,控制器不干预
            return;
        }
        else if(gameStage==GameStage.Discard)
        {
            // 丢弃所有手牌
            discardPile.AddCardToDiscard(handCards.handCards);
            StartCoroutine(hcui.DisCards());
            handCards.handCards.Clear();
            gameStage=GameStage.Enemy;
        }
        else if(gameStage==GameStage.Enemy)
        {
            StartCoroutine(dc.AnimatePanelAndText(new(){"敌","人","回","合"}));
            // 结束敌人阶段
            gameStage=GameStage.End;
        }
        else if(gameStage==GameStage.End)
        {
            // 回合数+1
            roundCount++;
            gameStage=GameStage.Pre;
        }
    }
    public void DrawCards(int drawNum)
    {
        // 不够抽的时候从弃牌堆补牌
        if(drawPile.drawPile.Count<drawNum)
        {
            StartCoroutine(hcui.ReturnCards());
            drawPile.AddCardToDraw(discardPile.discardPile);
            discardPile.discardPile.Clear();
        }
        drawNum=drawPile.drawPile.Count<drawNum ? drawPile.drawPile.Count : drawNum;    // 可能补完牌还不够抽
        handCards.DrawCard(drawPile.DrawCards(drawNum));
    }
    public void CardExecuteAction(int id,bool isPlused=false)
    {
        switch (id)
        {
            case 100: Card100ExecuteAction(isPlused); break;
            case 101: Card101ExecuteAction(isPlused); break;
            case 102: Card102ExecuteAction(isPlused); break;
            case 103: Card102ExecuteAction(isPlused); break;
            case 104: Card104ExecuteAction(isPlused); break;
            default: break;
        }
    }
    private void Card100ExecuteAction(bool isPlused)
    {
        int val=!isPlused ? 1 : 2;
        StartCoroutine(UpArrow());
        AttackAddonAdjust(val);
        DefenceAddonAdjust(val);
    }
    private void Card101ExecuteAction(bool isPlused)
    {
        int val=!isPlused ? 6 : 8;
        SingleAttack(val);
    }
    private void Card102ExecuteAction(bool isPlused)
    {
        int val=!isPlused ? 6 : 8;
        StartCoroutine(AddShield(val));
    }
    private void Card104ExecuteAction(bool isPlused)
    {
        int val=!isPlused ? 6 : 8;
        SingleAttack(val);
    }
    private void SingleAttack(int val)
    {
        PlayAudio(sfxStart);
        dc.playerObject.GetComponent<Animator>().SetTrigger("Attack");
    }
    private IEnumerator AddShield(int val)
    {
        GameObject shield=Instantiate(dc.shield,transform);
        shield.transform.SetParent(dc.higherCanvas.transform,false);
        shield.transform.position=new Vector3(500f,650f);
        shield.SetActive(true);
        shield.GetComponent<AudioSource>().Play();
        shield.transform.DOScale(0,0.25f).From();
        yield return new WaitForSeconds(0.25f);
        player.shield+=val;
        shield.transform.DOScale(0,0.1f);
        yield return new WaitForSeconds(0.1f);
    }
    private IEnumerator UpArrow()
    {
        GameObject arrow=Instantiate(dc.upArrow,transform);
        arrow.transform.SetParent(dc.higherCanvas.transform,false);
        arrow.transform.position=new Vector3(500f,650f);
        arrow.SetActive(true);
        arrow.GetComponent<AudioSource>().Play();
        arrow.transform.DOScale(0,0.25f).From();
        yield return new WaitForSeconds(0.25f);
        arrow.transform.DOScale(0,0.1f);
        yield return new WaitForSeconds(0.1f);
    }
    private void AttackAddonAdjust(int val)
    {
        player.attack_addon+=val;
    }
    private void DefenceAddonAdjust(int val)
    {
        player.defence_addon+=val;
    }
}
