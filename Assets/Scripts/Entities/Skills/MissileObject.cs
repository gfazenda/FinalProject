using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileObject : MonoBehaviour {
    float moveTimer = 0;
    float moveDuration = 10f;
    Vector3 targetPos;
    bool moving = false, attacking = false;
    Coord whereToHit;
    float damage;
    public void setCoordinates(Coord coord, float dmg)
    {
        whereToHit = coord;
        damage = dmg;
    }

    public void GoToTarget(Vector3 target)
    {
        targetPos = target;
        moveTimer = 0;
        moving = true;
    }

    public void LookDown()
    {
        this.transform.rotation = new Quaternion(-180, 0, 0, 0);
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
            moving = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (attacking)
        {
            BoardManager.Instance.InstantiateEffect(Tags.Explosion, whereToHit);
            BoardManager.Instance.TileAttacked(whereToHit, damage);
            Destroy(this.gameObject);
        }
    }
}
