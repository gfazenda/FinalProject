using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDrain : SpecialTile {
    public float drainPercent = 0.25f;
    public int effectRadius = 2;
    float maxEffectRange;
    Player player = null;
    bool affectedPlayer = false;
    int instances = 0;
    float currentDistance = 10f, damageRemoved = 0, endingDamage = 0, currentDrain = 0;
    // Use this for initialization
    void Start () {
        EventManager.StartListening(Events.EnemiesTurn, DrainDamage);
        player = BoardManager.Instance._playerScript;
        maxEffectRange = ((float)effectRadius * 1.42f);
    }
	
    void DrainDamage()
    {
        currentDistance = BoardManager.Distance(position, player.GetPosition());
        if (currentDistance <= maxEffectRange)
        {
            if (!affectedPlayer)
                affectedPlayer = true;

            if(instances < 3)
                CalculateDrain();
            instances++;

        }
        else if (affectedPlayer)
        {
            ReturnDamage();
        }
    }

    private void ReturnDamage()
    {
       
        player.damage += damageRemoved;
        damageRemoved = 0;
        affectedPlayer = false;
        instances = 0;
        EventManager.TriggerEvent(Events.DamageUpdate);
    }

    void CalculateDrain()
    {
      
        endingDamage = player.damage * (1 - drainPercent);
        currentDrain = player.damage - endingDamage;
        damageRemoved += currentDrain;
        player.damage -= currentDrain;
        EventManager.TriggerEvent(Events.DamageUpdate);
    }
}
