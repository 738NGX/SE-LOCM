using System.Data.Common;
using UnityEngine;

public enum IntendType{None,Attack,Defence,Recover,Buff,Debuff};

public class Enemy : MonoBehaviour
{
    public GameController gc;
    public int index;
    public IntendType intendType=IntendType.None;   // 回合意图
    public int intendValue=-1;
    public int hp;                  // 体力值
    public int ap;                  // 基础攻击力
    public int dp;                  // 基础防御力
    public float attack_factor;     // 攻击倍率
    public float defence_factor;    // 防御倍率
    public int hp_limit;            // 体力上限
    public int shield;              // 护盾值
    public GameObject selector;

    private bool waitSelect=false;

    // 产生回合意图
    public void Prepare()
    {
        int dice=Random.Range(0,12);
        // 强制回血
        if(hp<hp_limit*0.25)
        {
            //intendType=IntendType.
        }
        // 0,1,2,3,4:攻击
        
        if(dice<5)
        {

        }
        // 5,6,7,8,9:叠甲或回血
        if(dice<9)
        {
            if(hp<hp_limit*0.5||(hp<hp_limit*0.75&&dice<7))
            {

            }
            else
            {

            }
        }
        // 10,11:强化
        if(dice<11)
        {
            
        }
        else
        {

        }
    }


    public void AddHP(int val)
    {
        if(val<1) return;
        if(hp+val>=hp_limit) hp=hp_limit;
        else hp+=val;
    }
    public void ReduceHP(int val)
    {
        int trueVal=(int)(val*defence_factor);
        if(trueVal<1) return;
        if(hp+shield-trueVal<=0)
        {
            hp=0;
            gc.dc.enemyObjects[index].GetComponent<Animator>().SetBool("Die",true);
            gc.enemyCount--;
        }
        else if(trueVal>shield)
        {
            hp-=trueVal-shield;
            shield=0;
        }
        else shield-=trueVal;
    }
    private void Start()
    {
        hp_limit=hp;
        attack_factor=1;
        defence_factor=1;
    }
    private void OnMouseEnter()
    {
        Debug.Log(gc.selectedEnemyIndex);
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
