using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaEnemy : Enemy {
    RaycastHit hit;
    bool found = false;
    Vector3 origin;
    public GameObject projectile;

    private void Start()
    {
        base.Start();
    }


    void RecalculateOrigin()
    {
        origin = transform.position;
        origin.y += 2;
    }
    
    public override void PerformAttack() {

        RecalculateOrigin();


        LookAtCoord(player.GetPosition());
        Vector3 ppos = player.gameObject.transform.position;//BoardManager.Instance.CoordToPosition(player.GetPosition(), false);
        Vector3 d = ppos - origin;
        if (Physics.Raycast(origin, d.normalized, out hit, atkRange)) {
           //  print("Found an object: " + hit.collider.tag);
            if (hit.collider.tag == Tags.Player)
            {
                base.PerformAttack();
                projectile.GetComponent<Projectile>().Shot(this.transform.position, ppos);
            }
        }
        else
        {
            found = false;
        }


    }

    public override void DoAction()
    {
        PreActionSteps();
        if (CheckWaitingTurns())
            return;

        if (currentDistance <= atkRange)
        {
            if (AttackIsValid(position))
            {
                PerformAttack();
                return;
            }else
            {
                Debug.Log("doing adv1");
                if (PerformAdvancedMove())
                {
                    Debug.Log("doing adv2");
                    PerformMove(myPath[0]);
                    myPath.RemoveAt(0);
                    return;
                }
                //MoveToAttack();
            }

        }

        PerformBasicMove();



    }


    void MoveToAttack()
    {

    }


    private void LateUpdate()
    {
        //Vector3 ppos = BoardManager.Instance.CoordToPosition(player.GetPosition(), false);
        ////ppos.y = 0.5f;
        //Vector3 d = ppos - this.transform.position;
        //if (Physics.Raycast(this.transform.position, d.normalized, out hit, atkRange))
        //{
        //    print("Found an object: " + hit.collider.tag);
            Debug.DrawLine(this.transform.position, hit.point);
       // }
           
    }

}
