using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTile : SpecialTile {
    int turnsAlive = 0;
    public int turnsToLive = 2;

	// Use this for initialization
	void Start () {
        EventManager.StartListening(Events.EnemiesTurn, DecreaseTurn);
    }

    private void OnEnable()
    {
        turnsAlive = turnsToLive;
        this.GetComponent<Collider>().enabled = true;
    }

    private void OnDisable()
    {
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
