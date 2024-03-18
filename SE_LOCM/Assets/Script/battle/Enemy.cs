using UnityEngine;
using DG.Tweening;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public enum IntendType{Unknown,Attack,Defence,Recover,Buff,Debuff,MAttack,HAttack,ADefence,ARecover,ABuff,ADebuff,Sleep};

public class Enemy : Creature
{
    public bool active=false;
    public int id;
    public int index;
    public IntendType intendType=IntendType.Unknown;    // 回合意图
    public int intendValue=-1;                          // 意图数值 
    public int intendTimes=-1;                          // 意图倍数
    public GameObject selector;
    public TextMeshProUGUI displayName;
    private EnemyInfo info;
    private Buff giveBuff=null;
    private Buff extraGiveBuff=null;
    private Creature buffReceiver;


    private bool waitSelect=false;

    public void Init(int id)
    {
        active=true;

        info=EnemyDatabase.data[id].Copy();
        this.id=id;
        hpLimit=info.hpLimit;
        hp=hpLimit;
        ap=info.ap;
        dp=info.dp;
        displayName.text=info.name;
        buffContainer.creature=this;
        
        for(int i=0;i<info.initBuffs.Count;i++)
        {
            var buff=info.initBuffs[i];
            if(buff.Style==BuffStyle.Positive)
            {
                AddBuff(buff);
                if(new[]{301,306,307,308,309}.Contains(buff.id)) AddBuff(info.initBuffs[++i]);
            }
            else
            {
                gc.player.AddBuff(buff);
                if(new[]{301,306,307,308,309}.Contains(buff.id)) gc.player.AddBuff(info.initBuffs[++i]);
            }
        }
    }
    
    // 产生回合意图
    public void Prepare()
    {
        if(!active) return;
        DecideGrowth();
        intendType=GetIntendType();
        intendValue=GetIntendValue(intendType);
    }
    private void DecideGrowth()
    {
        int diceA=Random.Range(0,100),diceD=Random.Range(0,100);

        if(diceA<info.apAddChance) ap+=info.apAddSpeed;
        if(diceD<info.dpAddChance) dp+=info.dpAddSpeed;
    }
    private IntendType GetIntendType()
    {
        int dice=Random.Range(0,100);
        for(int i=0;i<10;i++)
        {
            if(dice>=info.intendChances[i]) continue;
            
            IntendType intend=info.intends[i];
            
            giveBuff=info.giveBuffs[i];
            extraGiveBuff=info.extraGiveBuffs[i];
            
            if(intend==IntendType.Buff||intend==IntendType.ABuff)
            {
                buffReceiver=this;
            }
            else if(intend==IntendType.Debuff||intend==IntendType.ADebuff)
            {
                buffReceiver=gc.player;
            }
            else buffReceiver=null;

            return intend;
        }
        return IntendType.Unknown;
    }
    private int GetIntendValue(IntendType intend)
    {
        int val=intend switch
        {
            IntendType.Attack=>buffContainer.CallAttack(ap),
            IntendType.MAttack=>buffContainer.CallAttack((int)(ap-info.ap+0.25f*info.ap)),
            IntendType.HAttack=>buffContainer.CallAttack((int)(1.5f*ap)),
            IntendType.Defence=>dp,
            IntendType.Buff=>giveBuff.Level,
            IntendType.Debuff=>giveBuff.Level,
            IntendType.Recover=>info.dp,
            IntendType.Sleep=>-1,
            IntendType.ADefence=>buffContainer.CallAttack(ap),
            IntendType.ABuff=>buffContainer.CallAttack(ap),
            IntendType.ADebuff=>buffContainer.CallAttack(ap),
            IntendType.ARecover=>buffContainer.CallAttack(ap),
            _=>-1
        };
        intendTimes=intend==IntendType.MAttack ? info.mAttackTimes : 1;
        return val;
    }
    public bool IsIntendAttack()
    {
        return new[]{IntendType.Attack,IntendType.MAttack,IntendType.HAttack,IntendType.ADefence,
                     IntendType.ABuff,IntendType.ADebuff,IntendType.ARecover}.Contains(intendType);
    }
    public void Execute()
    {
        if(!active) return;
        if(IsIntendAttack())
        {
            Attack(intendValue,intendTimes);
        }
        if(new[]{IntendType.Defence,IntendType.ADefence}.Contains(intendType))
        {
            AddShield(dp);
        }
        if(new[]{IntendType.Recover,IntendType.ARecover}.Contains(intendType))
        {
            AddHP(info.dp);
        }
        if(buffReceiver!=null)
        {
            if(giveBuff is not null) buffReceiver.AddBuff(giveBuff);
            if(extraGiveBuff is not null) buffReceiver.AddBuff(extraGiveBuff);
        }

        intendType=IntendType.Unknown;
        intendValue=-1;
        intendTimes=1;
    }
    private void Attack(int val,int times=1)
    {
        if(!active) return;
        StartCoroutine(AttackCoroutine(val,times));
    }
    private IEnumerator AttackCoroutine(int val,int times)
    {
        for(int i=0;i<times;i++)
        {
            gc.dc.enemyObjects[index].GetComponent<Animator>().SetTrigger("Attack");
            Camera.main.transform.DOShakePosition(0.35f,0.5f).OnComplete(() =>
            {
                Camera.main.transform.position=gc.cameraPosition;
            });
            gc.player.ReduceHP(val);
            gc.dc.playerObject.GetComponent<Animator>().SetTrigger("Hurt");
            gc.dc.UpdateHPSlider(-1);
            yield return new WaitForSeconds(0.15f);
        }
    }
    public override void AddShield(int val)
    {
        if(!active) return;
        gc.dc.enemyObjects[index].GetComponent<Animator>().SetTrigger("Buff");
        gc.PlayAudio(gc.sfxDefence);
        shield+=val;
    }
    public override void AddHP(int val)
    {
        if(!active) return;
        gc.dc.enemyObjects[index].GetComponent<Animator>().SetTrigger("Buff");
        base.AddHP(val);
        gc.dc.UpdateHPSlider(index);
    }
    public override void ReduceHP(int val)
    {
        if(!active) return;
        gc.dc.enemyObjects[index].GetComponent<Animator>().SetTrigger("Hurt");
        base.ReduceHP(val);
        gc.dc.UpdateHPSlider(index);
        if(hp==0)
        {
            gc.dc.enemyObjects[index].GetComponent<Animator>().SetBool("Die",true);
            active=false;
            gc.enemyCount--;
            gc.player.AddCoins(buffContainer.StealedCoins);
            gameObject.SetActive(false);
            if(gc.player.ContainsBook(11))
            {
                // 孙子算经效果:每当有1名敌人死亡，算术值+1，抽一张牌。
                gc.player.sp+=1;
                gc.DrawCards(1);
            }
        }
    }
    public override void AddBuff(Buff buff)
    {
        if(!active) return;
        gc.dc.enemyObjects[index].GetComponent<Animator>().SetTrigger("Buff");
        base.AddBuff(buff);
    }
    private void OnMouseEnter()
    {
        if(!active) return;
        if(gc.selectedEnemyIndex==-1&&hp>0)
        {
            waitSelect=true;
            selector.SetActive(true);
        }
    }
    private void OnMouseExit()
    {
        if(!active) return;
        if(waitSelect)
        {
            selector.SetActive(false);
            waitSelect=false;
        }
    }
    private void OnMouseDown()
    {
        if(!active) return;
        if(waitSelect)
        {
            gc.selectedEnemyIndex=index;
            selector.SetActive(false);
            waitSelect=false;
        }
    }
}
