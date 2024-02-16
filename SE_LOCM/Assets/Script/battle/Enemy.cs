using System.Data.Common;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public enum IntendType{Unknown,Attack,Defence,Recover,Buff,Debuff,Sleep};

public class Enemy : MonoBehaviour
{
    public GameController gc;
    public int index;
    public IntendType intendType=IntendType.Unknown;   // 回合意图
    public int intendValue=-1;
    public int intendTimes=-1;
    public int hp;                  // 体力值
    public int ap;                  // 基础攻击力
    public int dp;                  // 基础防御力
    public int hp_limit;            // 体力上限
    public int shield;              // 护盾值
    public GameObject selector;

    private bool waitSelect=false;

    // 产生回合意图
    public void Prepare()
    {
        int dice=Random.Range(0,12);
        intendTimes=1;
        // 强制回血
        if(hp<hp_limit*0.25)
        {
            intendType=IntendType.Recover;
            intendValue=dp;
        }
        // 0,1,2,3,4:攻击
        else if(dice<5)
        {
            intendType=IntendType.Attack;
            intendValue=ap;
        }
        // 5,6,7,8,9:叠甲或回血
        else if(dice<9)
        {
            if(hp<hp_limit*0.5||(hp<hp_limit*0.75&&dice<7))
            {
                intendType=IntendType.Recover; 
            }
            else intendType=IntendType.Defence;
            intendValue=dp;
        }
        // 10,11:强化
        else
        {
            intendType=IntendType.Sleep;
            intendValue=-1;
        }
    }
    public void Execute()
    {
        switch(intendType)
        {
            case IntendType.Attack: StartCoroutine(Attack(intendValue,intendTimes)); break;
            case IntendType.Defence: AddShield(intendValue); break;
            case IntendType.Recover: AddHP(intendValue); break;
            default: break;
        }
        intendType=IntendType.Unknown;
        intendValue=-1;
        intendTimes=1;
    }
    private IEnumerator Attack(int val,int times)
    {
        for(int i=0;i<times;i++)
        {
            gc.dc.enemyObjects[index].GetComponent<Animator>().SetTrigger("Attack");
            gc.PlayAudio(gc.sfxHurt);
            Camera.main.transform.DOShakePosition(0.5f,0.5f);
            gc.player.ReduceHP(val);
            gc.dc.playerObject.GetComponent<Animator>().SetTrigger("Hurt");
            gc.dc.UpdateHPSlider(-1);
            yield return new WaitForSeconds(0.15f);
        }
    }
    private void AddShield(int val)
    {
        gc.dc.enemyObjects[index].GetComponent<Animator>().SetTrigger("Buff");
        gc.PlayAudio(gc.sfxDefence);
        shield+=val;
    }
    public void AddHP(int val)
    {
        gc.dc.enemyObjects[index].GetComponent<Animator>().SetTrigger("Buff");
        gc.PlayAudio(gc.sfxRecover);
        if(val<1) return;
        if(hp+val>=hp_limit) hp=hp_limit;
        else hp+=val;
        gc.dc.UpdateHPSlider(index);
    }
    public void ReduceHP(int val)
    {
        if(val<1) return;
        gc.dc.enemyObjects[index].GetComponent<Animator>().SetTrigger("Hurt");
        gc.PlayAudio(gc.sfxHurt);
        if(hp+shield-val<=0)
        {
            hp=0;
            gc.dc.enemyObjects[index].GetComponent<Animator>().SetBool("Die",true);
            gc.enemyCount--;
        }
        else if(val>shield)
        {
            hp-=val-shield;
            shield=0;
        }
        else shield-=val;
        gc.dc.UpdateHPSlider(index);
    }
    private void Start()
    {
        hp_limit=hp;
    }
    private void OnMouseEnter()
    {
        if(gc.selectedEnemyIndex==-1&&hp>0)
        {
            waitSelect=true;
            selector.SetActive(true);
        }
    }
    private void OnMouseExit()
    {
        if(waitSelect)
        {
            selector.SetActive(false);
            waitSelect=false;
        }
    }
    private void OnMouseDown()
    {
        if(waitSelect)
        {
            gc.selectedEnemyIndex=index;
            selector.SetActive(false);
            waitSelect=false;
        }
    }
}
