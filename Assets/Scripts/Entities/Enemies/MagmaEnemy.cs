using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaEnemy : Enemy {
    RaycastHit hit;
    bool found = false;

    public GameObject projectile;
    public override void PerformAttack(){
        
        LookAtCoord(player.GetPosition());
        Vector3 ppos = player.gameObject.transform.position;//BoardManager.Instance.CoordToPosition(player.GetPosition(), false);
        Vector3 d = ppos - this.transform.position;
        if (Physics.Raycast(this.transform.position, d.normalized, out hit,atkRange)) {
           // print("Found an object: " + hit.collider.tag);
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
