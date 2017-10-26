using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockEnemy : Enemy {
    public float stunChance = 0.2f;
    float currentRandom; 
    public int stunCD = 3;
    int currentCD = 0;
    protected override void PerformAttack()
    {
        if (currentCD <= 0 && StunPlayer())
        {
            currentCD = stunCD;
            Debug.Log("stun player");
            player.SetTurnPenalty(1);
        }else if(currentCD > 0)
            currentCD--;

        base.DamagePlayer();
    }

    bool StunPlayer()
    {
        currentRandom = Random.Range(0f, 1f);
        //Debug.Log("result " + currentRandom);
        return (currentRandom < stunChance);
    }

}
