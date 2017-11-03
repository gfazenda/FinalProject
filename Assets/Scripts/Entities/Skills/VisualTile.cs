using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualTile : MonoBehaviour {
    int turnsAlive = 0;
    public int turnsToLive = 2;

    private void OnEnable()
    {
        turnsAlive = turnsToLive;
       // if (timeInTurns)
            EventManager.StartListening(Events.PlayerTurn, DecreaseTurn);

        //else
        //    Invoke("DisableTile", turnsToLive);
    }

    private void OnDisable()
    {
      //  if(timeInTurns)
            EventManager.StopListening(Events.PlayerTurn, DecreaseTurn);
        
    }

    void DisableTile()
    {
        this.gameObject.SetActive(false);
    }


    void DecreaseTurn()
    {
        turnsAlive--;
        if (turnsAlive <= 0)
            DisableTile();
    }
}
