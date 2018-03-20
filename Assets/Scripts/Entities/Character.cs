using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Entity))]
public class Character : MonoBehaviour {

    public float speed = 3;// { get; set; }
  
    public float damage = 5;// { get; set; }

    public float yHeight = 0.5f;
    float step;
    protected bool moving = false, finishedMove = false;
    protected Vector3 targetPos, lookPos;
    public Coord position = new Coord();

    public BoardManager.tileType characterType;

    protected Entity _entityScript;

    protected int waitingTurns = 0;

    public GameObject deathParticle;

    float moveTimer = 0;
    public float moveDuration = 0.2f;

    protected void Start()
    {
        _entityScript = this.GetComponent<Entity>();
         //  position = new Coord((int)initPosition.x, (int)initPosition.y);
     //    targetPos = BoardManager.Instance.CoordToPosition(position, false);
    }

    public virtual void SetPosition(Coord newPos)
    {
        BoardManager.Instance.UpdatePosition(position, newPos, characterType);
        position = newPos;
        targetPos = BoardManager.Instance.CoordToPosition(position, false);
        targetPos.y = yHeight;
        LookAtCoord(newPos);
        SetMoving();
    }

    public Coord GetPosition()
    {
        return position;
    }

    public void SetTurnPenalty(int turns)
    {
        waitingTurns = turns;
    }

    protected void LookAtCoord(Coord pos)
    {
        lookPos = BoardManager.Instance.CoordToPosition(pos, false);
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
    }

    protected void SetMoving()
    {
       // if (characterType == BoardManager.tileType.player)
        {
            moving = true;
            finishedMove = false;
            moveTimer = 0;
            //
            Move();
        }
        //else
        //{
        //    StartCoroutine(Move2());
        //}
       // transform.LookAt(lookPos);
    }



    public virtual void TakeDamage(float damage) {
        _entityScript.UpdateHealthAmount(damage);

        if (_entityScript.Dead())
        {
            this.gameObject.SetActive(false);
            Instantiate(deathParticle, this.transform.position, this.transform.rotation);
            BoardManager.Instance.SetEmptyPosition(position);
        }
    }

    public virtual void UpdateHPBar()
    {
        _entityScript.DoHPBardUpdate();
    }


    private void FixedUpdate()
    {
        //if (characterType != BoardManager.tileType.player)
        //    return;

        if (moving && transform.position != targetPos)
        {
            Move();
        }
        else if (moving && transform.position == targetPos)
        {
            finishedMove = true;
            moving = false;
        }
    }

    protected void Move()
    {
        moveTimer += Time.deltaTime;
        float step = moveTimer / moveDuration;
        //transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        transform.position = Vector3.Lerp(transform.position, targetPos, step);
    }
}
