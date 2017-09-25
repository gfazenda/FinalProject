using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy {
    
	// Use this for initialization
	void Start () {
        atkRange = 6f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void PerformAttack()
    {

        base.PerformAttack();
    }
}
