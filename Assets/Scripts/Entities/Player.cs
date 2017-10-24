using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    //float speed  = 3 ;
    //bool moving = false;
    //Vector3 targetPos;
    //Coord position;
    public int manaPool = 100;
    public int overchargeManacost = 10, mineManacost = 4, overchargeTurns = 3;
    int maxMana;
    private Vector2 touchOrigin = -Vector2.one;
    bool playerTurn = true, turnInvoked = false, usedOverCharge = false;
    public enum Actions { Move, Overcharge, Skill2, Mine, BasicAtk };
    MineController _mineScript;
    Actions currentAction;
    BoardManager.tileType targetType;
    Coord tentativePos = new Coord();
    public GameObject damagePrefab;
    GameObject explosion = null;
    List<GameObject> explosions = new List<GameObject>();
    float timer;
    PlayerStatus _status;
    [HideInInspector]
    public float dmgMultiplier = 1f;

    private void Awake()
    {
        _mineScript = this.GetComponent<MineController>();
        _status = this.GetComponent<PlayerStatus>();
        EventManager.StartListening(Events.PlayerTurn, PlayerTurn);
        
    }

    void Start()
    {
        base.Start();
        maxMana = manaPool;
        UXManager.instance.UpdatePlayerMana(manaPool, maxMana);
        UXManager.instance.UpdatePlayerHP(_entityScript.HP, _entityScript.maxHP);
    }

    void PlayerTurn()
    {
        playerTurn = CanAct();

        if (playerTurn)
            UXManager.instance.EnableButtons();
    }

    void SetEnemiesTurn()
    {

        waitingTurns -= 1;
        if (waitingTurns <= 0)
            usedOverCharge = false;
        turnInvoked = false;
        EventManager.TriggerEvent(Events.EnemiesTurn);
    }

    public void SetStatus(PlayerStatus.statuses status)
    {
        _status.SetStatus(status);
    }

    private void OnMouseDown()
    {
        if (!moving && playerTurn)
            BoardManager.Instance.DisplayMarkers(position, 1);
    }

    public void ShowMineMarkers()
    {
        if (CanUseSpell(mineManacost))
        {
            BoardManager.Instance.DisplayMarkers(position, 3, true, true);
        } else
        {
            UXManager.instance.DisplayMessage("Not enough mana", 0.3f);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage * dmgMultiplier);
        UXManager.instance.UpdatePlayerHP(_entityScript.HP,_entityScript.maxHP);
        ShowDamagePrefab();
        if (_entityScript.Dead())
            EventManager.TriggerEvent(Events.LevelLost);
    }

    void ShowDamagePrefab()
    {
        damagePrefab.SetActive(true);
        CancelInvoke();
        Invoke("DisableDamagePrefab", 1f);
    }

    void DisableDamagePrefab()
    {
        damagePrefab.SetActive(false);
    }

    public bool CanAct()
    {
        if (waitingTurns > 0)
        {

            if (!turnInvoked)
            {
                timer = GameManager.Instance.GetEnemyTurnDuration();
                if(usedOverCharge)
                    UXManager.instance.DisplayMessage("Recharging for " + waitingTurns + " turn(s)", timer);
                else
                {
                    UXManager.instance.DisplayMessage("Actions disabled for " + waitingTurns + " turn(s)", timer);
                }
                turnInvoked = true;
                if(waitingTurns <= (overchargeTurns - 1))
                {
                    DisableExplosions();
                }
                Invoke("SetEnemiesTurn", (timer * 2));
            }
           // Debug.Log("s1");
            return false;
        }
        return true;
       
    }



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
            TentativeMove(horizontal, vertical);
        }
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

    bool CanUseSpell(int cost)
    {
        if( manaPool >= cost)
        {
            return true;
        }
        return false;
    }

    void UpdateMana(int cost)
    {
        manaPool -= cost;
        UXManager.instance.UpdatePlayerMana(manaPool,maxMana);
    }

    public void PerformAction(Actions _action, Coord target = null)
    {
        switch (_action)
        {
            case Actions.Move:
                base.SetPosition(target);
                break;
            case Actions.Overcharge:
                if (CanUseSpell(overchargeManacost))
                {
                    Overcharge();
                    UpdateMana(overchargeManacost);
                    waitingTurns = overchargeTurns;
                    usedOverCharge = true;
                }
                else
                {
                    UXManager.instance.DisplayMessage("Not enough mana", 0.3f);
                }
                break;
            case Actions.Skill2:
                break;
            case Actions.Mine:
                    UpdateMana(mineManacost);
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
            explosions.Add(BoardManager.Instance.InstantiateEffect(Tags.ElectricExplosion, t.Value));
            if (t.Key == BoardManager.tileType.enemy)
                GameManager.Instance.EnemyDamaged((damage*3), t.Value);
        }
    }

    void DisableExplosions()
    {
        Debug.Log("EWROWERJKWERHKWEHRKJEHRJKHW");
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
}
