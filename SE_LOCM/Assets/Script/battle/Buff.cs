using System.Collections.Generic;
using UnityEngine;

public enum BuffStage{Intend,Effect,End};
public enum BuffType{Strengthen,Harden,Infirmity,Frail};

public class Buff
{
    public BuffType buffType;
    public BuffStage buffStage;
    public bool isPlused;
    public int remainRounds;
    public Player effectPlayer=null;
    public List<Enemy> effectEnemies=null;

    private readonly GameController gc;
    private int endRound;

    public Buff(GameController gc,BuffType buffType,int effectRounds,bool isPlused=false,Player effectPlayer=null,List<Enemy> effectEnemies=null)
    {
        if(effectPlayer==null&&effectEnemies==null) Debug.LogError("No Effective Object.");
        this.gc=gc;
        this.buffType=buffType;
        this.isPlused=isPlused;
        this.effectPlayer=effectPlayer;
        this.effectEnemies=effectEnemies;
        
        remainRounds=effectRounds;
        endRound=gc.roundCount+effectRounds;
    }

    public void RoundCountUpdate()
    {
        remainRounds=endRound-gc.roundCount;
    }
    public void AdjustEndRound(int val)
    {
        endRound+=val;
    }
}