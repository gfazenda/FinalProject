using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteMine : SpecialTile {

    public int damage = 20;
    int turnsAlive = 0;
    public int turnsToLive = 2;

    // Use this for initialization
    void Start()
    {
        EventManager.StartListening(Events.PlayerTurn, DecreaseTurn);
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
        {
            Explode();
            this.gameObject.SetActive(false);
        }
    }

    void Explode()
    {
        BoardManager.tileType[] types = { BoardManager.tileType.enemy };
        List<KeyValuePair<BoardManager.tileType, Coord>> neighbours = BoardManager.Instance.GetNeighbours(position, 1, types, true);
        foreach (KeyValuePair<BoardManager.tileType, Coord> t in neighbours)
        {
            GameManager.Instance.EnemyDamaged(damage, t.Value);
        }
    }

}
