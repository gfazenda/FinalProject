using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(HealthBar))]
public class Character : MonoBehaviour {

    public float speed = 3;// { get; set; }
    public float HP = 10;// { get; set; }
    public float damage = 5;// { get; set; }
    float maxHP;

    float step;
    protected bool moving = false;
    protected Vector3 targetPos;
    public Coord position = new Coord();

    public BoardManager.tileType characterType;

    HealthBar hpScript;

    protected int rechargingTurns = 0;

    protected void Start()
    {
        maxHP = HP;
        hpScript = this.GetComponentInChildren<HealthBar>();
        //  position = new Coord((int)initPosition.x, (int)initPosition.y);
        targetPos = BoardManager.Instance.CoordToPosition(position, false);
    }

    public virtual void SetPosition(Coord newPos)
    {
        BoardManager.Instance.UpdatePosition(position, newPos, characterType);
        position = newPos;
        targetPos = BoardManager.Instance.CoordToPosition(position, false);
        SetMoving();
    }

    public Coord GetPosition()
    {
        return position;
    }

    protected void LookAtCoord(Coord pos)
    {
        transform.LookAt(BoardManager.Instance.CoordToPosition(pos, false));
    }

    protected void SetMoving()
    {
        moving = true;
        StartCoroutine(Move2());
        transform.LookAt(targetPos);
    }

    void UpdateHealthBar()
    {
        float healthAmount = (float)HP / (float)maxHP;
        hpScript.UpdateBar(healthAmount);
    }

    public virtual void TakeDamage(float damage) {
        this.HP -= damage;
        UpdateHealthBar();
        if (HP <= 0)
        {
            this.gameObject.SetActive(false);
            BoardManager.Instance.SetEmptyPosition(position);
        }
    }


    private void FixedUpdate()
    {
        if (moving && transform.position != targetPos)
        {
            Move();
        }
        else
        {
            moving = false;
        }
    }

    protected void Move()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
    }

    IEnumerator Move2()
    {
        while(transform.position != targetPos)
        {
            step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            yield return new WaitForSeconds(0.3f);
        }
      
        moving = false;
    }
}
