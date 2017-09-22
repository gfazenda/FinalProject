using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AStar))]
public class Enemy : Character {
    public float atkRange = 1;
    Coord playerPosition = null;
    List<Coord> myPath = new List<Coord>();
    Player player = null;
    int failedAttempts = 0;
    void CreatePath()
    {        
        playerPosition = player.GetPosition();
        myPath = this.GetComponent<AStar>().FindPath(this.position, playerPosition);
    }

    public void Initialize()
    {
        player = BoardManager.Instance._playerScript;
    }

    protected virtual void DamagePlayer()
    {
        player.TakeDamage(damage);
        LookAtCoord(player.GetPosition());
    }

    protected virtual void PerformMove()
    {
        this.SetPosition(myPath[0]);
        myPath.RemoveAt(0);
    }

    protected virtual void PerformAttack()
    {
        DamagePlayer();
    }



    public virtual void DoAction()
    {
        if (BoardManager.Distance(position,player.GetPosition()) <= atkRange)
        {

            // Debug.Log("close enough " + BoardManager.Instance.Distance(position, player.GetPosition()));
            PerformAttack();
            return;
        }


        if(playerPosition == null || playerPosition != player.GetComponent<Player>().GetPosition() || myPath.Count == 0)
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
            PerformMove();
        }
        else
        {
            failedAttempts++;
            if (failedAttempts >= 3)
            {
                CreatePath();
                failedAttempts = 0;
            }
        }


    }
}
