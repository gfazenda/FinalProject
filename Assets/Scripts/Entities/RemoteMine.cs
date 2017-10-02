using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoteMine : SpecialTile {

    public int damage = 20;
    int turnsAlive = 0;
    public int turnsToLive = 2;
    public Text turnsInfo;
    // Use this for initialization
    void Start()
    {
        EventManager.StartListening(Events.PlayerTurn, DecreaseTurn);
        turnsInfo.text = turnsAlive.ToString();
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
        turnsInfo.text = turnsAlive.ToString();
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
