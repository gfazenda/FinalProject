using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileObject : MonoBehaviour {
    float moveTimer = 0;
    public float moveDuration = 10f;
    Vector3 targetPos;
    bool moving = false, attacking = false;
    Coord whereToHit;
    Vector3 hitPosition;
    float damage;
    public void setCoordinates(Coord coord, float dmg)
    {
        whereToHit = coord;
        hitPosition = BoardManager.Instance.CoordToPosition(whereToHit);
        damage = dmg;
    }

    public void GoToTarget()
    {
        targetPos = hitPosition;
        Move();
    }

    void Move(){
        moveTimer = 0;
        moving = true;
    }

    public void FlyAway(){
        Vector3 farPosition = hitPosition;
        farPosition.y += 15;
        targetPos = farPosition;
        Move();
    }

    public void LookDown()
    {
        this.transform.rotation = new Quaternion(-180, 0, 0, 0);
        this.GetComponent<Rigidbody>().isKinematic = false;
        moveDuration = 0.8f;
        attacking = true;
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            moveTimer += Time.deltaTime;
            float step = moveTimer / moveDuration;
            transform.position = Vector3.Lerp(transform.position, targetPos, step);
        }

        if (transform.position == targetPos && moving)
        {
            moving = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (attacking && collision.gameObject.tag == Tags.Ground /*|| collision.gameObject.tag == Tags.Enemy*/)
        {
            BoardManager.Instance.InstantiateEffect(Tags.Explosion, whereToHit);
            BoardManager.Instance.TileAttacked(whereToHit, damage);
            BoardManager.Instance.SetInvalidPosition(whereToHit);
            Destroy(collision.gameObject);
            //Destroy(this.gameObject);
            gameObject.SetActive(false);
            moving = false;
        }
    }
}
