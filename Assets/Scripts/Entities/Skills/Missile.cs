using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Skill {

    public int turnsToImpact = 2;
    int currentTurns = 0;
    Coord targetPosition;
    public override void DoEffect(Coord position)
    {
        EventManager.StartListening(Events.PlayerTurn, LaunchMissile);
        currentTurns = turnsToImpact;
        targetPosition = position;
    }

    void LaunchMissile()
    {
        Debug.Log("missile in the works");
        if (currentTurns > 0)
            currentTurns--;
        else
        {
            Debug.Log("wowoow");
            BoardManager.Instance.TileAttacked(targetPosition, damage);
            EventManager.StopListening(Events.PlayerTurn, LaunchMissile);
        }

    }
}
