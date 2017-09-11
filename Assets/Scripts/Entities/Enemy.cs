using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {
    Coord playerPosition = null;
    List<Coord> myPath = new List<Coord>();
    Player player = null;

    void CreatePath()
    {        
        playerPosition = player.GetPosition();
        myPath = this.GetComponent<AStar>().FindPath(this.position, playerPosition);
    }

    public void Initialize()
    {
        player = BoardManager.Instance._playerScript;
    }

    public void DoAction()
    {
        if (BoardManager.Instance.Distance(position,player.GetPosition()) <= 1)
        {
            Debug.Log("close enough " + BoardManager.Instance.Distance(position, player.GetPosition()));
            return;
        }


        if(playerPosition == null || playerPosition != player.GetComponent<Player>().GetPosition() || myPath.Count == 0)
        {           
            CreatePath();
            Debug.Log("repath");
        }
        Debug.Log("count " + myPath.Count);
        Debug.Log("dist " + BoardManager.Instance.Distance(position, player.GetPosition()));
        if (BoardManager.Instance.IsValid(myPath[0], false))
        {
            this.SetPosition(myPath[0]);
            myPath.RemoveAt(0);
        }else
        {
            CreatePath();
        }


    }
}
