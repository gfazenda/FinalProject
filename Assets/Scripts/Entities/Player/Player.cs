using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    public int manaPool = 100;
  //  public int overchargeManacost = 10, mineManacost = 4, overchargeTurns = 3;
    int maxMana;
    float timer, minimumSwipe = 100f;

    private Vector2 touchOrigin = -Vector2.one;

    bool playerTurn = true, performedAction = false, usedOverCharge = false, usedSkill = false, checkSkills = true, isClcked = false;

    public bool spellsBlocked = false;
    public Coord invalidPos = new Coord(-1,-1);
    public enum Actions { Move, Overcharge, Missile, Mine, BasicAtk };

    Actions currentAction;

    MineController _mineScript;
    PlayerExtendedMove _extendedMove;

    BoardManager.tileType targetType;
    Coord tentativePos = new Coord();

    public GameObject damagePrefab;

    GameObject explosion = null;

    List<GameObject> explosions = new List<GameObject>();

    string feedbackMessage;
    PlayerStatus _status;

    public List<Skill> _skillList = new List<Skill>();

    List<string> activeSkills = new List<string>();
    Dictionary<string, Skill> _skills = new Dictionary<string, Skill>();
    Skill currentSkill = null;

    private void Awake()
    {
        _mineScript = this.GetComponent<MineController>();
        _status = this.GetComponent<PlayerStatus>();
        _extendedMove = this.GetComponent<PlayerExtendedMove>();

        EventManager.StartListening(Events.PlayerTurn, PlayerTurn);
        EventManager.StartListening(Events.GamePaused, GamePaused);

    }

    void GamePaused()
    {
        playerTurn = !playerTurn;
    }

    void Start()
    {
        base.Start();
        maxMana = manaPool;
        UXManager.instance.UpdatePlayerMana(manaPool, maxMana);
        UXManager.instance.UpdatePlayerHP(_entityScript.HP, _entityScript.maxHP);
        InitializeSkills();
    }

        void InitializeSkills()
        {
            for (int i = 0; i < _skillList.Count; i++)
            {
                _skills.Add(_skillList[i].name, _skillList[i]);
            }
        }

    void PlayerTurn()
    {
        playerTurn = CanAct();

    #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
           EventManager.TriggerEvent(Events.EnableMoveButtons);
    #endif
        if (_extendedMove.PathConfigured())
            GoToNextWaypoint();

        if (playerTurn && !spellsBlocked)
            CheckAndActivateSkills();
    }


    void CheckAndActivateSkills()
    {
        if (checkSkills)
        {
            activeSkills.Clear();
            foreach (KeyValuePair<string, Skill> skill in _skills)
            {
                if (CanUseSpell(skill.Value.manacost)){
                    activeSkills.Add(skill.Key);
                }
            }
        }

        UXManager.instance.EnableButtons(activeSkills);
        checkSkills = false;
    }

    void SetEnemiesTurn()
    {

        waitingTurns -= 1;
        if (waitingTurns <= 0)
            usedOverCharge = false;
       // turnInvoked = false;
        EventManager.TriggerEvent(Events.EnemiesTurn);
    }

    public void SetStatus(PlayerStatus.statuses status)
    {
        _status.SetStatus(status);
    }

    public void BlockSpells(bool block)
    {
        spellsBlocked = block;
        if (spellsBlocked)
        {
            UXManager.instance.DisableButtons();
        }else
        {
            CheckAndActivateSkills();
        }
    }

    private void OnMouseDown()
    {
        if (!moving && playerTurn)
        {
            isClcked = !isClcked;
            _extendedMove.PlayerClicked(isClcked);
        }
            //BoardManager.Instance.DisplayMarkers(position, 1);
    }

    void GoToNextWaypoint()
    {
        Coord nextPos = _extendedMove.NextPosition();
        isClcked = false;
        if (IsGround(nextPos))
            PerformAction(Actions.Move, nextPos);
        else
        {
            CancelPath();
        }
    }

    public void ConfirmPath()
    {
        _extendedMove.ConfirmPath();
        if (!_extendedMove.PathConfigured()) { 
            isClcked = false;
            _extendedMove.PlayerClicked(isClcked);
        }   
        GoToNextWaypoint();
    }

    public void CancelPath()
    {
        _extendedMove.CancelMovement();
    }

    bool IsGround(Coord pos)
    {
        return BoardManager.Instance.GetPositionType(pos) == BoardManager.tileType.ground;
    }

    public override void TakeDamage(float damage)
    {
        ShowDamagePrefab();
        base.TakeDamage(damage);
        UXManager.instance.UpdatePlayerHP(_entityScript.HP,_entityScript.maxHP);
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
            timer = EnemyCoordinator.Instance.GetEnemyTurnDuration();
            ShowWaiting();
            if (waitingTurns >= 2)
            {
                DisableExplosions();
            }
            Invoke("SetEnemiesTurn", (timer));
            // Debug.Log("s1");
            return false;
        }

        currentSkill = null;
        usedSkill = false;
        finishedMove = false;
        performedAction = false;

        return true;
       
    }

    private void ShowWaiting()
    {
        if (usedOverCharge)
            feedbackMessage = "Recharging for " + waitingTurns + " turn(s)";
        else
        {
            feedbackMessage = "Actions disabled for " + waitingTurns + " turn(s)";
        }
        UXManager.instance.DisplayMessage(feedbackMessage,timer*2);
    }



    // Update is called once per frame
    void Update ()
    {
        if (performedAction || moving || !playerTurn)
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
        TouchInput(ref horizontal, ref vertical);
#endif
        //End of mobile platform dependendent compilation section started above with #elif
        //Check if we have a non-zero value for horizontal or vertical


        if (!moving && horizontal != 0 || vertical != 0 && !isClcked)
        {
            TentativeMove(horizontal, vertical);
        }
    }

    private void TouchInput(ref int horizontal, ref int vertical)
    {
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
                return;
            }

            //Set touchEnd to equal the position of this touch
            Vector2 touchEnd = myTouch.position;

            //Calculate the difference between the beginning and end of the touch on the x axis.
            float x = touchEnd.x - touchOrigin.x;

            //Calculate the difference between the beginning and end of the touch on the y axis.
            float y = touchEnd.y - touchOrigin.y;


            if (UXManager.instance.TouchOverUI(myTouch.fingerId) || !ValidSwipe(x, y))
                return;

            if (isClcked)
            {
                if (myTouch.phase == TouchPhase.Moved)
                {
                    tentativePos = new Coord(position);
                    tentativePos.x += horizontal;
                    tentativePos.y += vertical;
                    targetType = BoardManager.Instance.GetPositionType(tentativePos);

                    if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        _extendedMove.CalculateSwipe(false, x, minimumSwipe);
                    }else
                    {
                        _extendedMove.CalculateSwipe(true,y, minimumSwipe);
                    }

                }
                else if (myTouch.phase == TouchPhase.Ended)
                {
                    ConfirmPath();
                }

            }

            //If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
            else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
            {
               
                //Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
                touchOrigin.x = -1;

                //Check if the difference along the x axis is greater than the difference along the y axis.
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    horizontal = x > 0 ? 1 : -1;
                }else
                {
                    vertical = y > 0 ? 1 : -1;
                }
                //    //If x is greater than zero, set horizontal to 1, otherwise set it to -1
                //    horizontal = x > 0 ? 1 : -1;
               
                //    //If y is greater than zero, set horizontal to 1, otherwise set it to -1
                //    
            }
        }
    }

    bool ValidSwipe(float xswipe, float yswipe)
    {
        return (Mathf.Abs(xswipe) >= minimumSwipe || Mathf.Abs(yswipe) >= minimumSwipe);
    }


    public void TentativeMove(int horizontal, int vertical)
    {
        tentativePos = new Coord(position);
        tentativePos.x += horizontal;
        tentativePos.y += vertical;
        targetType = BoardManager.Instance.GetPositionType(tentativePos);
        //switch (targetType)
        //{
        //    case BoardManager.tileType.ground:
        //        break;
        //    case BoardManager.tileType.player:
        //        break;
        //    case BoardManager.tileType.wall:
        //        break;
        //    case BoardManager.tileType.enemy:
        //        break;
        //    case BoardManager.tileType.exit:
        //        break;
        //    case BoardManager.tileType.obstacle:
        //        break;
        //    case BoardManager.tileType.outOfLimits:
        //        break;
        //    default:
        //        break;
        //}
        if (targetType == BoardManager.tileType.enemy || targetType == BoardManager.tileType.obstacle)
        {
            PerformAction(Actions.BasicAtk, tentativePos);
        }
        else if ((targetType == BoardManager.tileType.ground) && !tentativePos.CompareTo(invalidPos))
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
        return (manaPool >= cost);
    }

    void UpdateMana(int cost)
    {
        manaPool -= cost;
        checkSkills = true;
        UXManager.instance.UpdatePlayerMana(manaPool,maxMana);
    }

    public void PerformAction(Actions _action, Coord target = null)
    {
        EventManager.TriggerEvent(Events.DisableMoveButtons);
        finishedMove = true;
        string actionInfo = "";
        switch (_action)
        {
            case Actions.Move:
                base.SetPosition(target);
                BoardManager.Instance.DisableMarkers();
                GameLogs.Instance.AddLog(GameLogs.logType.playerMovement, target.DebugInfo());
                break;
            case Actions.Overcharge:
                _skills.TryGetValue("Overcharge", out currentSkill);
                if (CanUseSpell(currentSkill.manacost))
                {
                  //  _skills.TryGetValue("Overcharge", out currentSkill);
                    usedSkill = true;
                    waitingTurns = currentSkill.cooldown;
                    usedOverCharge = true;
                    target = position;
                    actionInfo = "Overcharge";
                    //   Overcharge();
                }
                else
                {
                    DisplayNoMana();
                }
                break;
            case Actions.Missile:
                _skills.TryGetValue("Missile", out currentSkill);
                invalidPos = target;
                usedSkill = true;
                actionInfo = "Missile";
                break;
            case Actions.Mine:
                //   UpdateMana(mineManacost);
                _skills.TryGetValue("RemoteMine", out currentSkill);
                usedSkill = true;
                actionInfo = "Mine";
                // _mineScript.PlaceMine(target);
                break;
            case Actions.BasicAtk:
                LookAtCoord(target);
                BoardManager.Instance.TileAttacked(target, damage);
                GameLogs.Instance.AddLog(GameLogs.logType.plyerDamage, actionInfo, (int)damage);
                break;
            default:
                break;
        }
        if (usedSkill)
        {
            UpdateMana(currentSkill.manacost);
            currentSkill.DoEffect(target);
            GameLogs.Instance.AddLog(GameLogs.logType.skillUsed, actionInfo);
        }
        
        performedAction = true;

        if (finishedMove)
            CallNextTurn();
    }

    private static void DisplayNoMana()
    {
        UXManager.instance.DisplayMessage("Not enough mana", 0.3f,alert:true);
    }

    private void LateUpdate()
    {
        if (playerTurn && performedAction && finishedMove)
            CallNextTurn();
    }

    private void CallNextTurn()
    {
        playerTurn = false;
        BoardManager.Instance.DisableMarkers();
        EventManager.TriggerEvent(Events.EnemiesTurn);
            
    }

    void DisableExplosions()
    {
        //Debug.Log("EWROWERJKWERHKWEHRKJEHRJKHW");
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

    public void ShowMissileMarkers()
    {
        _skills.TryGetValue("Missile", out currentSkill);
        if (CanUseSpell(currentSkill.manacost))
        {
             BoardManager.Instance.DisplayMarkers(position, 4, true, missile:true);
        }
        else
        {
            DisplayNoMana();
        }
    }

    public void ShowMineMarkers()
    {
        _skills.TryGetValue("RemoteMine", out currentSkill);
        if (CanUseSpell(currentSkill.manacost))
        {
            BoardManager.Instance.DisplayMarkers(position, 2, true, true);
        }
        else
        {
            DisplayNoMana();
        }
    }
}
