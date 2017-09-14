using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    //float speed  = 3 ;
    //bool moving = false;
    //Vector3 targetPos;
    //Coord position;
    private Vector2 touchOrigin = -Vector2.one;
    bool playerTurn = true;
    public enum Actions {Move, Skill1, Skill2, Skill3, BasicAtk };
    Actions currentAction;
    Coord tentativePos = new Coord();
    private void Awake()
    {
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

    //public void SetPosition(Coord pos)
    //{
    //    position = pos;
    //    targetPos = BoardManager.Instance.CoordToPosition(position, false);
    //    SetMoving();
    //}

    //public Coord GetPosition()
    //{
    //    return position;
    //}

    // Update is called once per frame
    void Update () {
        if (!playerTurn || moving)
            return;
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
            tentativePos = new Coord(position);
           // tentativePos.Copy(position);
            tentativePos.x += horizontal;
            tentativePos.y += vertical;
            //Debug.Log("t " + tentativePos.y);
            //Debug.Log("p " + position.y);
            //Debug.Log("22 " + position.x);
            if(BoardManager.Instance.GetPositionType(tentativePos) == BoardManager.tileType.enemy)
            {
                PerformAction(Actions.BasicAtk, tentativePos);
            }
            else if (BoardManager.Instance.GetPositionType(tentativePos) == BoardManager.tileType.ground)
            {
                PerformAction(Actions.Move, tentativePos);
            }
            Debug.Log("clicking");
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


    public void PerformAction(Actions _action, Coord target)
    {
        switch (_action)
        {
            case Actions.Move:
                base.SetPosition(target);
                break;
            case Actions.Skill1:
                break;
            case Actions.Skill2:
                break;
            case Actions.Skill3:
                break;
            case Actions.BasicAtk:
                GameManager.Instance.EnemyDamaged(damage, target);
                break;
            default:
                break;
        }
        playerTurn = false;
        BoardManager.Instance.DisableMarkers();
        EventManager.TriggerEvent(Events.EnemiesTurn);
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
