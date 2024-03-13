using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Reflection;

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
    public List<FriendItem> friends;    // 友人
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
    public RoundPlayedCards roundPlayedCards;

    public Vector3 cameraPosition=new();
    
    public void PlayAudio(AudioClip clip)
    {
        AudioSource source=gameObject.AddComponent<AudioSource>();
        source.clip=clip;
        source.Play();
    }
    private void Start()
    {
        roundCount=1;
        cameraPosition=Camera.main.transform.position;
        
        sc.LoadLocalData();
        dc.UpdateHPSlider(-1);
        
        EnemyInit();

        roundPlayedCards=new(this);

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
                enemies[i].gameObject.SetActive(false);
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
            Camera.main.transform.position=cameraPosition;
            
            // 敌人产生回合意图
            foreach(Enemy enemy in enemies) enemy.Prepare();
            
            StartCoroutine(dc.AnimatePanelAndText(new(){"玩","家","回","合"}));
            int extraSP=player.buffContainer.ExtraSP;
            
            if(player.ContainsBook(10))
            {
                // 周髀算经效果:可以保留剩余的算术值到下一回合。
                player.sp+=player.spInit+extraSP;
                player.sp=player.sp<0?0:player.sp;
            }
            else
            {
                player.sp=player.spInit+extraSP<0 ? 0 : player.spInit+extraSP;
            }
            
            if(roundCount==1)
            {
                // 均输章残卷
                if(player.ContainsBook(4)) player.AddBuff(new(201,1));             
                // 少广章残卷
                if(player.ContainsBook(6)) player.AddBuff(new(205,1));
                // 谢察微算经
                if(player.ContainsBook(21)) AddShield(10);
                // 黄帝九章算法细草
                if(player.ContainsBook(23)) player.AddBuff(new(109,3));
                // 算学源流
                if(player.ContainsBook(24)) player.sp+=1;
                // 数书九章
                if(player.ContainsBook(25)) DrawCards(2);
                // 测圆海镜
                if(player.ContainsBook(26)) player.AddHP(5); dc.UpdateHPSlider(-1);
                // 益古演段
                if(player.ContainsBook(27)) player.ap++;
                // 算法统宗
                if(player.ContainsBook(28)) player.dp++;
            }
            // 衰分章残卷
            if(roundCount==2 && player.ContainsBook(3)) AddShield(20);
            
            // 五曹算经效果:第五回合准备阶段结束后，给予50点护盾值。
            if(roundCount==5 && player.ContainsBook(15)) AllAttack(50);
            
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
            discardPile.AddCardsToDiscard(handCards.Cards);
            StartCoroutine(hcui.DisCards());
            handCards.Cards.Clear();
            
            // 五经算术效果:第五回合弃牌阶段结束后，给予所有敌人50点伤害。
            if(roundCount==5 && player.ContainsBook(15)) AllAttack(50);
            // 商功章残卷
            if(roundCount==2 && player.ContainsBook(5)) AllAttack(20);
            
            // 结束弃牌阶段
            gameStage=GameStage.Enemy;
        }
        else if(gameStage==GameStage.Enemy)
        {
            StartCoroutine(dc.AnimatePanelAndText(new(){"敌","人","回","合"}));
            
            // 敌人根据意图行动
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
            // 移除玩家护盾
            if(!player.buffContainer.ExistBuff(109))
            {
                player.shield=!player.ContainsBook(17) ? 0 : player.shield-15;
                player.shield=player.shield<0 ? 0 : player.shield;
            }
            roundPlayedCards.ClearCards();
            
            // 回合数+1
            roundCount++;
            
            // 结束结束阶段
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
        if(drawPile.Cards.Count<val)
        {
            hcui.ReturnCards();
            drawPile.AddCards(discardPile.discards);
            discardPile.discards.Clear();
            // 张邱建算经:每当弃牌堆返回卡牌给摸牌堆时，算术值+1。
            if(player.ContainsBook(13)) player.sp++;
        }
        val=drawPile.Cards.Count<val ? drawPile.Cards.Count : val;    // 可能补完牌还不够抽
        if(val==0) return;
        handCards.AddCards(drawPile.DrawCards(val));
        hcui.DrawCards();
        StartCoroutine(hcui.CardDisplayUpdate());
    }
    // 等待弃牌
    public void WaitingDiscards(int val)
    {
        if(handCards.Cards.Count<val) return;
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
            Camera.main.transform.DOShakePosition(0.35f,0.5f).OnComplete(() =>
            {
                Camera.main.transform.position=cameraPosition;
            });
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
            Camera.main.transform.DOShakePosition(0.35f,0.5f).OnComplete(() =>
            {
                Camera.main.transform.position=cameraPosition;
            });
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
        string methodName=$"Card{id:D3}ExecuteAction";
        MethodInfo methodInfo=GetType().GetMethod(methodName,BindingFlags.NonPublic|BindingFlags.Instance);

        if(methodInfo!=null)
        {
            methodInfo.Invoke(this,new object[]{isPlused});
        }
        else
        {
            // 处理方法不存在的情况
            //Debug.Log("id "+id+" do not have execute action.");
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
        Card card=discardPile.discards[^1];
        if(card.id!=114) Debug.LogError("id114 error");
        int val=!isPlused ? 9+(card.playTimes-1)*3 : 9+(card.playTimes-1)*3;
        SelectAttack(val+player.ap);
    }
    private void Card115ExecuteAction(bool isPlused)
    {
        // 圭田术
        int val1=!isPlused ? 8 : 12;
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
        int val=!isPlused ? 2 : 3;
        player.sp+=val;
        player.ReduceHP(3);
    }
    private void Card118ExecuteAction(bool isPlused)
    {
        // 圆田术
        int val1=!isPlused ? 8 : 12;
        int val2=!isPlused ? 1 : 2;
        DrawCards(val2);
        AddShield(val1+player.dp);
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
