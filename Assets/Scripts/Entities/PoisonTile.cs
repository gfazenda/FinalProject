using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTile : MonoBehaviour {
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
        if (turnsAlive <= 0)
            this.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
