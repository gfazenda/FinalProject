using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public float speed = 3;
    public int HP;

    protected bool moving = false;
    protected Vector3 targetPos;
    public Coord position = new Coord();


    protected void Start()
    {
      //  position = new Coord((int)initPosition.x, (int)initPosition.y);
        targetPos = BoardManager.Instance.CoordToPosition(position, false);
    }

    public virtual void SetPosition(Coord pos)
    {
        position = pos;
        targetPos = BoardManager.Instance.CoordToPosition(position, false);
        SetMoving();
    }

    public Coord GetPosition()
    {
        return position;
    }

    protected void SetMoving()
    {
        moving = true;
        transform.LookAt(targetPos);
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
}
