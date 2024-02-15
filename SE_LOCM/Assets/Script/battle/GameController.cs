using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Fungus;

// 准备阶段、摸牌阶段、出牌阶段、弃牌阶段、敌人阶段、结束阶段
public enum GameStage{Pre,Draw,Play,Discard,Enemy,End,Victory,Defeat,Null};

public class GameController : MonoBehaviour
{
    public DrawPile drawPile;           // 摸牌堆
    public HandCards handCards;         // 手牌堆
    public DiscardPile discardPile;     // 弃牌堆
    public Player player;               // 玩家
    public List<Enemy> enemies;         // 敌人
    public DisplayController dc;        // 显示控制
    public HandCardsUI hcui;            // 手牌显示控制
    public AudioClip sfxStart;          // 开始音效
    public AudioClip sfxAttack;         // 攻击音效
    public AudioClip sfxHurt;           // 受伤音效
    public AudioClip sfxVictory;        // 战胜音效
    public AudioClip sfxDefeat;         // 战败音效

    public int roundCount;              // 回合数
    public GameStage gameStage;         // 游戏阶段
    public int enemyCount;              // 存活敌人数量
    public int selectedEnemyIndex=-2;
    
    
    public void PlayAudio(AudioClip clip)
    {
        AudioSource source=gameObject.AddComponent<AudioSource>();
        source.clip=clip;
        source.Play();
    }
    void Start()
    {
        roundCount=1;
        enemyCount=enemies.Count;

        PlayAudio(sfxStart);

        StartCoroutine(dc.AnimatePanelAndText(new(){"战","斗","开","始"},1f));
        gameStage=GameStage.Pre;
    }
    void Update()
    {
        if(gameStage==GameStage.Null||dc.isAnimating) return;  // 系统动画时不进行操作
        
        // 游戏结束检查
        if(player.hp<=0) gameStage=GameStage.Defeat;
        else if(enemyCount<=0) gameStage=GameStage.Victory;
        
        // 游戏阶段检查
        if(gameStage==GameStage.Pre)
        {
            StartCoroutine(dc.AnimatePanelAndText(new(){"玩","家","回","合"}));
            player.sp=player.sp_init;
            
            // 结束准备阶段
            gameStage=GameStage.Draw;
        }
        else if(gameStage==GameStage.Draw)
        {
            // 默认情况下可以摸5张牌
            int drawNum=5;
            DrawCards(drawNum);
            // 结束摸牌阶段
            gameStage=GameStage.Play;
        }
        else if(gameStage==GameStage.Play)
        {
            // 玩家操作,系统不干预
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
            player.shield=0;
            // 回合数+1
            roundCount++;
            gameStage=GameStage.Pre;
        }
        else if(gameStage==GameStage.Victory)
        {
            PlayAudio(sfxVictory);
            StartCoroutine(dc.AnimatePanelAndText(new(){"战","斗","胜","利"},1f));
            gameStage=GameStage.Null;
        }
        else if(gameStage==GameStage.Defeat)
        {
            PlayAudio(sfxDefeat);
            StartCoroutine(dc.AnimatePanelAndText(new(){"战","斗","失","败"},1f));
            gameStage=GameStage.Null;
        }
    }
    
    // 抽牌
    public void DrawCards(int drawNum)
    {
        // 不够抽的时候从弃牌堆补牌
        if(drawPile.drawPile.Count<drawNum)
        {
            hcui.ReturnCards();
            drawPile.AddCardToDraw(discardPile.discardPile);
            discardPile.discardPile.Clear();
        }
        drawNum=drawPile.drawPile.Count<drawNum ? drawPile.drawPile.Count : drawNum;    // 可能补完牌还不够抽
        handCards.DrawCard(drawPile.DrawCards(drawNum));
        hcui.DrawCards();
        StartCoroutine(hcui.CardDisplayUpdate());
    }
    // 选择攻击
    private void SelectAttack(int val)
    {
        StartCoroutine(SelectAttackCoroutine(val));
    }
    private IEnumerator SelectAttackCoroutine(int val)
    {
        dc.bezierArrow.SetActive(true);
        selectedEnemyIndex=-1;

        yield return new WaitUntil(()=>selectedEnemyIndex!=-1);

        dc.bezierArrow.SetActive(false);

        if(selectedEnemyIndex>=0&&selectedEnemyIndex<enemies.Count)
        {
            enemies[selectedEnemyIndex].ReduceHP(val);
            PlayAudio(sfxAttack);
            dc.playerObject.GetComponent<Animator>().SetTrigger("Attack");
            dc.UpdateHPSlider(selectedEnemyIndex);
            dc.enemyObjects[selectedEnemyIndex].GetComponent<Animator>().SetTrigger("Hurt");
            Camera.main.transform.DOShakePosition(0.5f,0.5f);
            PlayAudio(sfxHurt);
        }
        else Debug.LogError("Selected enemy index is out of range.");

        selectedEnemyIndex=-2;
    }
    // 普遍攻击
    private void AllAttack(int val)
    {
        StartCoroutine(AllAttackCoroutine(val));
    }
    private IEnumerator AllAttackCoroutine(int val)
    {
        PlayAudio(sfxAttack);
        dc.playerObject.GetComponent<Animator>().SetTrigger("Attack");
        for(int i=0;i<enemies.Count;i++)
        {
            enemies[i].ReduceHP(val);
            dc.UpdateHPSlider(i);
            dc.enemyObjects[i].GetComponent<Animator>().SetTrigger("Hurt");
            PlayAudio(sfxHurt);
            Camera.main.transform.DOShakePosition(0.5f,0.5f);
            yield return new WaitForSeconds(0.15f);
        }
    }
    // 叠盾
    private void AddShield(int val)
    {
        StartCoroutine(AddShieldCoroutine(val));
    }
    private IEnumerator AddShieldCoroutine(int val)
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
    // 正面buff
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
    // 攻击力调整
    private void AttackAddonAdjust(int val)
    {
        player.attack_addon+=val;
    }
    // 防御力调整
    private void DefenceAddonAdjust(int val)
    {
        player.defence_addon+=val;
    }

    /**-------------------------------------
     *
     *          卡牌打出后效果
     *
    --------------------------------------**/
    public void CardExecuteAction(int id,bool isPlused=false)
    {
        switch (id)
        {
            case 100: Card100ExecuteAction(isPlused); break;
            case 101: Card101ExecuteAction(isPlused); break;
            case 102: Card102ExecuteAction(isPlused); break;
            case 103: Card103ExecuteAction(isPlused); break;
            case 104: Card104ExecuteAction(isPlused); break;
            default: break;
        }
    }
    private void Card100ExecuteAction(bool isPlused)
    {
        // 算筹
        int val=!isPlused ? 1 : 2;
        StartCoroutine(UpArrow());
        AttackAddonAdjust(val);
        DefenceAddonAdjust(val);
    }
    private void Card101ExecuteAction(bool isPlused)
    {
        // 筹算加法
        int val=!isPlused ? 6 : 8;
        SelectAttack(val+player.attack_addon);
    }
    private void Card102ExecuteAction(bool isPlused)
    {
        // 筹算减法
        int val=!isPlused ? 6 : 8;
        AddShield(val+player.defence_addon);
    }
    private void Card103ExecuteAction(bool isPlused)
    {
        // 筹算乘法
        int val=!isPlused ? 1 : 2;
        DrawCards(1);
        if(handCards.Top().type==CardType.Attack) DrawCards(val);
    }
    private void Card104ExecuteAction(bool isPlused)
    {
        // 筹算除法
        int val=!isPlused ? 12 : 18;
        AllAttack(val/enemies.Count);
    }
}
