using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AStar))]
public class Enemy : Character {

    public float range = 1;
    public float atkRange;
    Coord playerPosition = new Coord();
    List<Coord> myPath = new List<Coord>();
    protected Player player = null;
    int failedAttempts = 0;
    public bool diagonalAtk = false;


    void CreatePath()
    {        
        playerPosition = player.GetPosition();
        myPath = EnemyCoordinator.Instance.GetPath(this.position, playerPosition);//this.GetComponent<AStar>().FindPath(this.position, playerPosition);
    }

    public void Initialize()
    {
        player = BoardManager.Instance._playerScript;
        atkRange = range * Utility.distanceMultiplier;
    }

    protected virtual void DamagePlayer()
    {
        player.TakeDamage(damage);
        LookAtCoord(player.GetPosition());
        GameLogs.Instance.AddLog(GameLogs.logType.enemyDamage, "", (int)damage);
    }

    protected virtual void PerformMove()
    {
        if (BoardManager.Distance(myPath[0], position) == 1)
        {
            this.SetPosition(myPath[0]);
            myPath.RemoveAt(0);
        }
    }

    public Coord GetNextMove()
    {
        return myPath[0];
    }

    protected bool AttackIsValid()
    {
        return (diagonalAtk == BoardManager.IsInDiagonal(position, player.GetPosition()));
    }

    protected virtual void PerformAttack()
    {
        if(AttackIsValid())
            DamagePlayer();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        base.UpdateHPBar();
    }


    public virtual void DoAction()
    {
        if (BoardManager.Distance(position,player.GetPosition()) <= atkRange && AttackIsValid())
        {
            // Debug.Log("close enough " + BoardManager.Distance(position, player.GetPosition()));
            //Debug.Log(atkRange);
            PerformAttack();
            return;
        }
        

        if (playerPosition == null || playerPosition != player.GetComponent<Player>().GetPosition() || myPath.Count == 0)
        {           
            CreatePath();
           // Debug.Log("repath");
        }
        //if (myPath.Count == 0)
        //    return;
        // Debug.Log("count " + myPath.Count);
        //Debug.Log("dist " + BoardManager.Instance.Distance(position, player.GetPosition()));
        if (BoardManager.Instance.GetPositionType(myPath[0]) == BoardManager.tileType.ground)
        {
            PerformMove();
        }else
        {
            failedAttempts++;
            if (failedAttempts >= 2)
            {
                CreatePath();
                failedAttempts = 0;
            }
        }


    }
}
