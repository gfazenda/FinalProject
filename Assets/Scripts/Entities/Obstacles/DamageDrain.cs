public class DamageDrain : Obstacle {
    public float drainPercent = 0.25f;
    int instances = 0;
    float currentDistance = 10f, damageRemoved = 0, endingDamage = 0, currentDrain = 0;
 
    private void OnEnable()
    {
        EventManager.StartListening(Events.EnemiesTurn, DrainDamage);
    }

    private void OnDisable()
    {
        if (damageRemoved > 0)
            ReturnDamage();
        EventManager.StopListening(Events.EnemiesTurn, DrainDamage);
    }


    void DrainDamage()
    {
        if (!gameObject.activeInHierarchy)
            return;

        currentDistance = BoardManager.Distance(position, player.GetPosition());
        if (currentDistance <= maxEffectRange)
        {
            if (!affectedPlayer)
                affectedPlayer = true;

            if(instances < 3)
                CalculateDrain();
            instances++;

        }else if (affectedPlayer)
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
