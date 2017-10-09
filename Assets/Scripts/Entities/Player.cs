﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    //float speed  = 3 ;
    //bool moving = false;
    //Vector3 targetPos;
    //Coord position;
    private Vector2 touchOrigin = -Vector2.one;
    bool playerTurn = true, turnInvoked = false;
    public enum Actions {Move, Overcharge, Skill2, Mine, BasicAtk };
    MineController _mineScript;
    Actions currentAction;
    BoardManager.tileType targetType;
    Coord tentativePos = new Coord();

    GameObject explosion = null;
    List<GameObject> explosions = new List<GameObject>();

    private void Awake()
    {
        _mineScript = this.GetComponent<MineController>();
        EventManager.StartListening(Events.PlayerTurn, PlayerTurn);
    }

    void PlayerTurn()
    {
        playerTurn = true;
    }
    // Use this for initialization
    //void Start () {
    //    base.Start();
    //}

    private void OnMouseDown()
    {
        if(!moving && playerTurn)
            BoardManager.Instance.DisplayMarkers(position,1);
    }

    public void ShowMineMarkers()
    {
        BoardManager.Instance.DisplayMarkers(position, 3,true,true);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UXManager.instance.UpdatePlayerHP(_entityScript.healthAmount);
        if (_entityScript.Dead())
            EventManager.TriggerEvent(Events.LevelLost);
    }

    void SetEnemiesTurn()
    {
        EventManager.TriggerEvent(Events.EnemiesTurn);
        turnInvoked = false;
        rechargingTurns -= 1;
    }


    // Update is called once per frame
    void Update () {
        if (!playerTurn || moving)
            return;
        else if (rechargingTurns > 0)
        {
            
            if (!turnInvoked)
            {
                float timer = GameManager.Instance.GetEnemyTurnDuration();
                UXManager.instance.DisplayMessage("Recharging for " + rechargingTurns + " turn(s)", timer);
                turnInvoked = true;
                Invoke("SetEnemiesTurn", (timer+0.1f));
            }
           
            return;
        }
       // if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;     //Used to store the horizontal move direction.
        int vertical = 0;       //Used to store the vertical move direction.

        //Check if we are running either in the Unity editor or in a standalone build.
#if UNITY_STANDALONE || UNITY_WEBPLAYER

        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        ////Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //Check if moving horizontally, if so set vertical to zero.
        if (horizontal != 0)
        {
            vertical = 0;
        }
        //Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            
            //Check if Input has registered more than zero touches
            if (Input.touchCount > 0)
            {
                //Store the first touch detected.
                Touch myTouch = Input.touches[0];
                
                //Check if the phase of that touch equals Began
                if (myTouch.phase == TouchPhase.Began)
                {
                    //If so, set touchOrigin to the position of that touch
                    touchOrigin = myTouch.position;
                }
                
                //If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
                else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
                {
                    //Set touchEnd to equal the position of this touch
                    Vector2 touchEnd = myTouch.position;
                    
                    //Calculate the difference between the beginning and end of the touch on the x axis.
                    float x = touchEnd.x - touchOrigin.x;
                    
                    //Calculate the difference between the beginning and end of the touch on the y axis.
                    float y = touchEnd.y - touchOrigin.y;
                    
                    //Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
                    touchOrigin.x = -1;
                    
                    //Check if the difference along the x axis is greater than the difference along the y axis.
                    if (Mathf.Abs(x) > Mathf.Abs(y))
                        //If x is greater than zero, set horizontal to 1, otherwise set it to -1
                        horizontal = x > 0 ? 1 : -1;
                    else
                        //If y is greater than zero, set horizontal to 1, otherwise set it to -1
                        vertical = y > 0 ? 1 : -1;
                }
            }
            
#endif //End of mobile platform dependendent compilation section started above with #elif
        //Check if we have a non-zero value for horizontal or vertical
       

        if (!moving && horizontal != 0 || vertical != 0)
        {
            // playerTurn = false;
            //Debug.Log("h " + horizontal);
            // Debug.Log("v " + vertical);
            TentativeMove(horizontal, vertical);


            //Debug.Log("p " + position.x + " " + position.y);
            //  targetPos = BoardManager.Instance.CoordToPosition(tentativePos, false);
            //  BoardManager.Instance.SetEmptyPosition(position);
            //  BoardManager.Instance.DisableMarkers();
            //position = tentativePos;


        }

        //if (moving && transform.position != targetPos)
        //{
        //    Move();
        //}
        //else
        //{
        //    moving = false;
        //}

    }

    private void TentativeMove(int horizontal, int vertical)
    {
        tentativePos = new Coord(position);
        tentativePos.x += horizontal;
        tentativePos.y += vertical;
        targetType = BoardManager.Instance.GetPositionType(tentativePos);
        if (targetType == BoardManager.tileType.enemy || targetType == BoardManager.tileType.obstacle)
        {
            PerformAction(Actions.BasicAtk, tentativePos);
        }
        else if ((targetType == BoardManager.tileType.ground))
        {
            PerformAction(Actions.Move, tentativePos);
        }
        else if (targetType == BoardManager.tileType.exit)
        {
            PerformAction(Actions.Move, tentativePos);
            EventManager.TriggerEvent(Events.LevelWon);
        }
    }

    public void PerformAction(Actions _action, Coord target = null)
    {
        switch (_action)
        {
            case Actions.Move:
                base.SetPosition(target);
                
                break;
            case Actions.Overcharge:
                Overcharge();
                rechargingTurns = 3;
                break;
            case Actions.Skill2:
                break;
            case Actions.Mine:
                _mineScript.PlaceMine(target);
                break;
            case Actions.BasicAtk:
                LookAtCoord(target);
                BoardManager.Instance.TileAttacked(target, damage);
                break;
            default:
                break;
        }
        playerTurn = false;
        BoardManager.Instance.DisableMarkers();
        EventManager.TriggerEvent(Events.EnemiesTurn);
    }

    void Overcharge()
    {
        BoardManager.tileType[] types = { BoardManager.tileType.enemy, BoardManager.tileType.ground };
        List < KeyValuePair < BoardManager.tileType, Coord>> neighbours = BoardManager.Instance.GetNeighbours(position, 2,types, true);
        foreach (KeyValuePair<BoardManager.tileType, Coord> t in neighbours)
        {
            InstantiateExplosion(BoardManager.Instance.CoordToPosition(t.Value));
            if (t.Key == BoardManager.tileType.enemy)
                GameManager.Instance.EnemyDamaged((damage*3), t.Value);
        }
        Invoke("DisableExplosions", 1f);
    }

    private void InstantiateExplosion(Vector3 position)
    {
        explosion = ObjectPooler.SharedInstance.GetPooledObject(Tags.ElectricExplosion);
        explosion.transform.position = position;
        explosion.SetActive(true);
        explosions.Add(explosion);
    }

    void DisableExplosions()
    {
        for (int i = 0; i < explosions.Count; i++)
        {
            explosions[i].SetActive(false);
        }
        explosions.Clear();
    }

    public void DoOvercharge()
    {
        PerformAction(Actions.Overcharge);
    }

    //public override void SetPosition(Coord pos)
    //{
    //    base.SetPosition(pos);
    //    playerTurn = false;
    //}

    //void SetMoving()
    //{
    //    moving = true;
    //    transform.LookAt(targetPos);
    //}


    //void Move()
    //{
    //    float step = speed * Time.deltaTime;
    //    transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
    //}
}
