using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Fungus;

// 准备阶段、摸牌阶段、出牌阶段、弃牌阶段、敌人阶段、结束阶段
public enum GameStage{Pre,Draw,Play,Discard,Enemy,End,Victory,Defeat,Null,Reward};

public class GameController : MonoBehaviour
{
    public SceneFader sf;
    public List<int> enemyIds=new(){};
    public DrawPile drawPile;           // 摸牌堆
    public HandCards handCards;         // 手牌堆
    public DiscardPile discardPile;     // 弃牌堆
    public Player player;               // 玩家
    public List<Enemy> enemies;         // 敌人
    public DisplayController dc;        // 显示控制
    public SavesController sc;          // 存档控制
    public HandCardsUI hcui;            // 手牌显示控制
    public AudioClip sfxStart;          // 开始音效
    public AudioClip sfxAttack;         // 攻击音效
    public AudioClip sfxDefence;        // 防御音效
    public AudioClip sfxBuff;           // 正面效果音效
    public AudioClip sfxDeuff;          // 负面效果音效
    public AudioClip sfxRecover;        // 回复效果音效
    public AudioClip sfxHurt;           // 受伤音效
    public AudioClip sfxVictory;        // 战胜音效
    public AudioClip sfxDefeat;         // 战败音效

    public int roundCount;              // 回合数
    public GameStage gameStage;         // 游戏阶段
    public int enemyCount;              // 存活敌人数量
    public int waitingDiscardCount=0;   // 等待回合内弃牌数量
    public int selectedEnemyIndex=-2;   // 选中敌人序号,-2不选择,-1待选择
    
    public void PlayAudio(AudioClip clip)
    {
        AudioSource source=gameObject.AddComponent<AudioSource>();
        source.clip=clip;
        source.Play();
    }
    private void Start()
    {
        roundCount=1;
        
        sc.LoadLocalData();
        dc.UpdateHPSlider(-1);
        
        EnemyInit();

        PlayAudio(sfxStart);
        StartCoroutine(dc.AnimatePanelAndText(new(){"战","斗","开","始"},1f));
        gameStage=GameStage.Pre;
    }
    private void EnemyInit()
    {
        for(int i=0;i<3;i++)
        {
            if(enemyIds[i]==0)
            { 
                dc.enemyObjects[i].SetActive(false);
                continue;
            }
            
            enemyCount++;
            enemies[i].Init(enemyIds[i]);
        }
    }
    private void Update()
    {
        if(gameStage==GameStage.Null||dc.isAnimating) return;  // 系统动画时不进行操作
        if(gameStage==GameStage.Reward)
        {
            sc.SaveLocalData();
            sf.FadeOut("Scenes/reward");
            gameStage=GameStage.Null;
            return;
        }
        
        // 游戏结束检查
        if(player.hp<=0) gameStage=GameStage.Defeat;
        else if(enemyCount<=0) gameStage=GameStage.Victory;
        
        // 游戏阶段检查

        if(gameStage==GameStage.Pre)
        {
            // 敌人产生回合意图
            foreach(Enemy enemy in enemies) enemy.Prepare();
            
            StartCoroutine(dc.AnimatePanelAndText(new(){"玩","家","回","合"}));
            int extraSP=player.buffContainer.ExtraSP;
            int sp=player.spInit+extraSP<0 ? 0 : player.spInit+extraSP;
            player.sp=sp;
            
            // 结束准备阶段
            gameStage=GameStage.Draw;
        }
        else if(gameStage==GameStage.Draw)
        {
            // 默认情况下可以摸5张牌
            int extraCard=player.buffContainer.ExtraCard;
            int drawNum=5+extraCard<0 ? 0 : 5+extraCard;
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
            foreach(Enemy enemy in enemies)
            {
                if(!enemy.buffContainer.ExistBuff(109)) enemy.shield=0;
                enemy.Execute();
            } 
            // 结束敌人阶段
            gameStage=GameStage.End;
        }
        else if(gameStage==GameStage.End)
        {
            if(!player.buffContainer.ExistBuff(109)) player.shield=0;
            // 回合数+1
            roundCount++;
            gameStage=GameStage.Pre;
        }
        else if(gameStage==GameStage.Victory)
        {
            PlayAudio(sfxVictory);
            StartCoroutine(dc.AnimatePanelAndText(new(){"战","斗","胜","利"},2f));
            gameStage=GameStage.Reward;
        }
        else if(gameStage==GameStage.Defeat)
        {
            dc.playerObject.GetComponent<Animator>().SetBool("Die",true);
            PlayAudio(sfxDefeat);
            StartCoroutine(dc.AnimatePanelAndText(new(){"战","斗","失","败"},2f));
            gameStage=GameStage.Null;
        }
    }
    
    // 抽牌
    public void DrawCards(int val)
    {
        // 不够抽的时候从弃牌堆补牌
        if(drawPile.drawPile.Count<val)
        {
            hcui.ReturnCards();
            drawPile.AddCardToDraw(discardPile.discardPile);
            discardPile.discardPile.Clear();
        }
        val=drawPile.drawPile.Count<val ? drawPile.drawPile.Count : val;    // 可能补完牌还不够抽
        handCards.DrawCard(drawPile.DrawCards(val));
        hcui.DrawCards();
        StartCoroutine(hcui.CardDisplayUpdate());
    }
    // 等待弃牌
    public void WaitingDiscards(int val)
    {
        if(handCards.handCards.Count<val) return;
        waitingDiscardCount+=val;
        StartCoroutine(dc.AnimatePanelAndText(new(){"弃",val.ToString(),"张","牌"},0f));
    }
    // 选择攻击
    private void SelectAttack(int val,int times=1,Buff giveBuff=null)
    {
        StartCoroutine(SelectAttackCoroutine(player.buffContainer.CallAttack(val),times,giveBuff));
    }
    private IEnumerator SelectAttackCoroutine(int val,int times,Buff giveBuff)
    {
        dc.bezierArrow.SetActive(true);
        selectedEnemyIndex=-1;

        yield return new WaitUntil(()=>selectedEnemyIndex!=-1);

        dc.bezierArrow.SetActive(false);

        for(int i=0;i<times;i++)
        {
            enemies[selectedEnemyIndex].ReduceHP(val);
            PlayAudio(sfxAttack);
            dc.playerObject.GetComponent<Animator>().SetTrigger("Attack");
            Camera.main.transform.DOShakePosition(0.5f,0.5f);
        }

        if(giveBuff is not null)
        {
            enemies[selectedEnemyIndex].AddBuff(giveBuff);
        }

        selectedEnemyIndex=-2;
    }
    // 普遍攻击
    public void AllAttack(int val,Buff giveBuff=null)
    {
        StartCoroutine(AllAttackCoroutine(player.buffContainer.CallAttack(val),giveBuff));
    }
    private IEnumerator AllAttackCoroutine(int val,Buff giveBuff)
    {
        PlayAudio(sfxAttack);
        dc.playerObject.GetComponent<Animator>().SetTrigger("Attack");
        for(int i=0;i<enemies.Count;i++)
        {
            enemies[i].ReduceHP(val);
            if(giveBuff is not null) enemies[i].AddBuff(giveBuff);
            dc.UpdateHPSlider(i);
            dc.enemyObjects[i].GetComponent<Animator>().SetTrigger("Hurt");
            PlayAudio(sfxHurt);
            Camera.main.transform.DOShakePosition(0.5f,0.5f);
            yield return new WaitForSeconds(0.15f);
        }
    }
    // 叠盾
    public void AddShield(int val)
    {
        StartCoroutine(AddShieldCoroutine(val));
    }
    private IEnumerator AddShieldCoroutine(int val)
    {
        GameObject shield=Instantiate(dc.shield,transform);
        shield.transform.SetParent(dc.higherCanvas.transform,false);
        shield.transform.position=new Vector3(500f,650f);
        shield.SetActive(true);
        PlayAudio(sfxDefence);
        shield.transform.DOScale(0,0.25f).From();
        yield return new WaitForSeconds(0.25f);
        player.shield+=val;
        shield.transform.DOScale(0,0.1f);
        yield return new WaitForSeconds(0.1f);
    }
    // buff
    public void UpArrow()
    {
        StartCoroutine(UpArrowCoroutine());
    }
    private IEnumerator UpArrowCoroutine()
    {
        GameObject arrow=Instantiate(dc.upArrow,transform);
        arrow.transform.SetParent(dc.higherCanvas.transform,false);
        arrow.transform.position=new Vector3(500f,650f);
        arrow.SetActive(true);
        arrow.transform.DOScale(0,0.25f).From();
        yield return new WaitForSeconds(0.25f);
        arrow.transform.DOScale(0,0.1f);
        yield return new WaitForSeconds(0.1f);
    }
    public void DownArrow()
    {
        StartCoroutine(DownArrowCoroutine());
    }
    private IEnumerator DownArrowCoroutine()
    {
        GameObject arrow=Instantiate(dc.downArrow,transform);
        arrow.transform.SetParent(dc.higherCanvas.transform,false);
        arrow.transform.position=new Vector3(500f,650f);
        arrow.SetActive(true);
        arrow.transform.DOScale(0,0.25f).From();
        yield return new WaitForSeconds(0.25f);
        arrow.transform.DOScale(0,0.1f);
        yield return new WaitForSeconds(0.1f);
    }
    // 攻击力调整
    private void AttackPointsAdjust(int val)
    {
        player.ap+=val;
    }
    // 防御力调整
    private void DefencePointsAdjust(int val)
    {
        player.dp+=val;
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
            case 105: Card105ExecuteAction(isPlused); break;
            case 106: Card106ExecuteAction(isPlused); break;
            case 107: Card107ExecuteAction(isPlused); break;
            case 108: Card108ExecuteAction(isPlused); break;
            case 109: Card109ExecuteAction(isPlused); break;
            case 110: Card110ExecuteAction(isPlused); break;
            case 111: Card111ExecuteAction(isPlused); break;
            case 112: Card112ExecuteAction(isPlused); break;
            case 113: Card113ExecuteAction(isPlused); break;
            case 114: Card114ExecuteAction(isPlused); break;
            case 115: Card115ExecuteAction(isPlused); break;
            case 116: Card116ExecuteAction(isPlused); break;
            case 117: Card117ExecuteAction(isPlused); break;
            case 118: Card118ExecuteAction(isPlused); break;
            case 119: Card119ExecuteAction(isPlused); break;
            case 120: Card120ExecuteAction(isPlused); break;
            case 121: Card121ExecuteAction(isPlused); break;
            default: break;
        }
    }
    private void Card100ExecuteAction(bool isPlused)
    {
        // 算筹
        int val=!isPlused ? 1 : 2;
        UpArrow();
        AttackPointsAdjust(val);
        DefencePointsAdjust(val);
    }
    private void Card101ExecuteAction(bool isPlused)
    {
        // 筹算加法
        int val=!isPlused ? 6 : 8;
        SelectAttack(val+player.ap);
    }
    private void Card102ExecuteAction(bool isPlused)
    {
        // 筹算减法
        int val=!isPlused ? 6 : 8;
        AddShield(val+player.dp);
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
        AllAttack(val/enemyCount+player.ap);
    }
    private void Card105ExecuteAction(bool isPlused)
    {
        // 合分术
        int val=!isPlused ? 3 : 5;
        AddShield(val+player.dp);
        SelectAttack(val+player.ap);
    }
    private void Card106ExecuteAction(bool isPlused)
    {
        // 减分术
        int val=!isPlused ? 6 : 8;
        AddShield(val+player.dp);
        WaitingDiscards(1);
    }
    private void Card107ExecuteAction(bool isPlused)
    {
        // 约分术
        int val1=!isPlused ? 5 : 7;
        int val2=!isPlused ? 1 : 2;
        SelectAttack(val1+player.ap,1,new(103,val2));
    }
    private void Card108ExecuteAction(bool isPlused)
    {
        // 课分术
        DrawCards(1);
        int val=!isPlused ? handCards.Top().cost : handCards.Top().cost/2;
        WaitingDiscards(val);
    }
    private void Card109ExecuteAction(bool isPlused)
    {
        // 平分术
        StartCoroutine(Card109ExecuteActionCoroutine(isPlused));
    }
    private IEnumerator Card109ExecuteActionCoroutine(bool isPlused)
    {
        int val=!isPlused ? 8 : 12;
        float factor=!isPlused ? 1 : 1.5f;
        
        dc.bezierArrow.SetActive(true);
        selectedEnemyIndex=-1;

        yield return new WaitUntil(()=>selectedEnemyIndex!=-1);
        
        var selectedEnemy=enemies[selectedEnemyIndex];
        dc.bezierArrow.SetActive(false);

        selectedEnemy.ReduceHP(val+player.ap);
        PlayAudio(sfxAttack);
        dc.playerObject.GetComponent<Animator>().SetTrigger("Attack");
        Camera.main.transform.DOShakePosition(0.5f,0.5f);

        if(selectedEnemy.IsIntendAttack())
        {
            AddShield((int)(selectedEnemy.intendValue*selectedEnemy.intendTimes*factor)+player.dp);
        }

        selectedEnemyIndex=-2;
    }
    private void Card110ExecuteAction(bool isPlused)
    {
        // 经分术
        int val1=!isPlused ? 12 : 18;
        int val2=!isPlused ? 1 : 2;
        AllAttack(val1/enemyCount+player.ap,new Buff(104,val2));
    }
    private void Card111ExecuteAction(bool isPlused)
    {
        // 乘分术
        int val=!isPlused ? 3 : 4;
        DrawCards(val);
        WaitingDiscards(1);
    }
    private void Card112ExecuteAction(bool isPlused)
    {
        // 方田术
        int val=!isPlused ? 4 : 6;
        SelectAttack(val+player.ap,2);
    }
    private void Card113ExecuteAction(bool isPlused)
    {
        // 里田术
        DrawCards(1);
        if(isPlused||handCards.Top().type==CardType.Spell) AddShield(5+player.dp);
    }
    private void Card114ExecuteAction(bool isPlused)
    {
        // 大广田术
        Card card=discardPile.Top();
        if(card.id!=114) Debug.LogError("id114 error");
        int val=!isPlused ? 9+(card.playTimes-1)*3 : 9+(card.playTimes-1)*3;
        SelectAttack(val+player.ap);
    }
    private void Card115ExecuteAction(bool isPlused)
    {
        // 圭田术
        int val1=!isPlused ? 8 : 10;
        int val2=!isPlused ? 1 : 2;
        DrawCards(val2);
        SelectAttack(val1+player.ap);
    }
    private void Card116ExecuteAction(bool isPlused)
    {
        // 邪田术
        int val=!isPlused ? 3 : 5;
        SelectAttack(0,1,new(106,val));
    }
    private void Card117ExecuteAction(bool isPlused)
    {
        // 箕田术
        int val=!isPlused ? 1 : 2;
        player.sp+=val;
        WaitingDiscards(1);
    }
    private void Card118ExecuteAction(bool isPlused)
    {
        // 圆田术
        int val1=!isPlused ? 10 : 15;
        int val2=!isPlused ? 1 : 2;
        AddShield(val1+player.dp);
        player.AddBuff(new(102,val2));
    }
    private void Card119ExecuteAction(bool isPlused)
    {
        // 宛田术
        AddShield(player.shield);
    }
    private void Card120ExecuteAction(bool isPlused)
    {
        // 弧田术
        player.AddBuff(new(201,1));
    }
    private void Card121ExecuteAction(bool isPlused)
    {
        // 环田术
        SelectAttack(player.shield+player.ap);
    }
}
