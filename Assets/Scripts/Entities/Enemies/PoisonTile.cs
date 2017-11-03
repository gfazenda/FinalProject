using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTile : SpecialTile {
    int turnsAlive = 0;
    public int turnsToLive = 2;

	// Use this for initialization
	void Start () {
       
    }

    private void OnEnable()
    {
        turnsAlive = turnsToLive;
        EventManager.StartListening(Events.EnemiesTurn, DecreaseTurn);
        this.GetComponent<Collider>().enabled = true;
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.EnemiesTurn, DecreaseTurn);
        this.GetComponent<Collider>().enabled = false;
    }

    void DecreaseTurn()
    {
        if (!this.gameObject.activeInHierarchy)
            return;

        turnsAlive--;

        if (BoardManager.Instance._playerScript.position.CompareTo(position))
        {
            turnsAlive = 0;
            BoardManager.Instance._playerScript.SetStatus(PlayerStatus.statuses.poisoned);
        }

        if (turnsAlive <= 0)
            this.gameObject.SetActive(false);
    }

}
