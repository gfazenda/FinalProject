using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

	// Use this for initialization
	void Start () {
        Invoke("dopath", 2f);
        //
	}
	
    void dopath()
    {
        this.GetComponent<AStar>().FindPath(this.position, BoardManager.Instance._player.GetComponent<Player>().GetPosition());
    }
	// Update is called once per frame
	void Update () {
		
	}
}
