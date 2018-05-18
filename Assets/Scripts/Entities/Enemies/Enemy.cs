using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AStar))]
public class Enemy : Character {

    public float range = 1;
    public float atkRange, moveRange, currentDistance = 100;
    Coord playerPosition = new Coord();
    public List<Coord> myPath = new List<Coord>();
    protected Player player = null;
    int failedAttempts = 0;
    public bool diagonalAtk = false, moved = false;


    void CreatePath()
    {        
        playerPosition = player.GetPosition();
        myPath = EnemyCoordinator.Instance.GetPath(this.position, playerPosition);//this.GetComponent<AStar>().FindPath(this.position, playerPosition);
    }

    public void Initialize()
    {
        player = BoardManager.Instance._playerScript;
        atkRange = range * Utility.distanceMultiplier;
        transform.position = BoardManager.Instance.CoordToPosition(position,false);
        moveRange = 1;
    }

    protected virtual void DamagePlayer()
    {
        player.TakeDamage(damage);
        LookAtCoord(player.GetPosition());
        GameLogs.Instance.AddLog(GameLogs.logType.enemyDamage, "", (int)damage);
    }

    public virtual void PerformMove(Coord destination)
    {
        //Debug.Log("doing move "+ BoardManager.Distance(destination, position));
        if (BoardManager.Distance(destination, position) == moveRange && BoardManager.Instance.GetPositionType(destination)== BoardManager.tileType.ground )
        {
            //Debug.Log("doing move2");
            this.SetPosition(destination);
            moved = true;
        }
    }

    public Coord GetNextMove()
    {
        return myPath[0];
    }

    protected bool AttackIsValid(Coord pos)
    {
        return (diagonalAtk == BoardManager.IsInDiagonal(pos, player.GetPosition()));
    }

    protected bool CanAttackAt(Coord pos)
    {
        return (BoardManager.Distance(pos, player.GetPosition())<=atkRange && AttackIsValid(pos));
    }

    public virtual void PerformAttack()
    {
        if(CanAttackAt(position))
            DamagePlayer();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        base.UpdateHPBar();
    }

    protected bool CheckWaitingTurns()
    {
        if (waitingTurns > 0)
        {
            waitingTurns -= 1;
            return true;
        }
        return false;
    }

    protected void PreActionSteps()
    {
        moved = false;
        currentDistance = BoardManager.Distance(position, player.GetPosition());
    }


    public virtual void DoAction()
    {
        PreActionSteps();
        if (CheckWaitingTurns())
            return;

        if (currentDistance <= atkRange && AttackIsValid(position))
        {
            // Debug.Log("close enough " + BoardManager.Distance(position, player.GetPosition()));
            //Debug.Log(atkRange);
            PerformAttack();
            return;
        }

        //if (currentDistance < 3)
        //{
        //    if (PerformAdvancedMove())
        //    {
        //        PerformMove(myPath[0]);
        //        myPath.RemoveAt(0);
        //        return;
        //    }
        //}


        PerformBasicMove();



       

    }

    protected bool PerformAdvancedMove()
    {
        Debug.Log("doing advanced");
        bool moveDone = false;
        Coord possibleMove = new Coord(position);


        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {

                if (i == 0 || j == 0)
                {
                    Debug.Log(i + " " + j);
                    possibleMove.x = position.x + i;
                    possibleMove.y = position.y + j;
                    if (BoardManager.Instance.GetPositionType(possibleMove) == BoardManager.tileType.ground)
                    {
                        if (CanAttackAt(possibleMove) && AttackIsValid(possibleMove))
                        {
                            myPath.Clear();
                            myPath.Add(possibleMove);
                            return true;
                        }
                    }
                }

            }
        }


        return moveDone;

        
    }

    protected void PerformBasicMove()
    {
        if (playerPosition == null || !playerPosition.CompareTo(player.GetComponent<Player>().GetPosition()) || myPath.Count == 0)
        {
            CreatePath();
            // Debug.Log("repath");
        }
        if (myPath.Count == 0)
            return;
        // Debug.Log("count " + myPath.Count);
        //Debug.Log("dist " + BoardManager.Instance.Distance(position, player.GetPosition()));
        if (BoardManager.Instance.GetPositionType(myPath[0]) == BoardManager.tileType.ground)
        {
            PerformMove(myPath[0]);
            if(moved)
                myPath.RemoveAt(0);
        }
        else
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
