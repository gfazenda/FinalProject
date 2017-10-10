using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoteMine : SpecialTile {

    public int damage = 20;
    int turnsAlive = 0;
    public int turnsToLive = 2;
    public Text turnsInfo;
    GameObject explosion = null;
    List<GameObject> explosions = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        EventManager.StartListening(Events.PlayerTurn, DecreaseTurn);
        turnsInfo.text = turnsAlive.ToString();
    }

    private void OnEnable()
    {
        turnsAlive = turnsToLive;
        turnsInfo.text = turnsAlive.ToString();
    }

    private void OnDisable()
    {

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
        explosions.Add(BoardManager.Instance.InstantiateEffect(Tags.Explosion, position));
        BoardManager.tileType[] types = { BoardManager.tileType.enemy, BoardManager.tileType.ground};
        List<KeyValuePair<BoardManager.tileType, Coord>> neighbours = BoardManager.Instance.GetNeighbours(position, 1, types, true);
        foreach (KeyValuePair<BoardManager.tileType, Coord> t in neighbours)
        {
            explosions.Add(BoardManager.Instance.InstantiateEffect(Tags.Explosion, t.Value));
            if (t.Key == BoardManager.tileType.enemy)
                GameManager.Instance.EnemyDamaged(damage, t.Value);
        }
        Invoke("DisableExplosions", 1f);
    }

    private void InstantiateExplosion(Vector3 position)
    {
        explosion = ObjectPooler.SharedInstance.GetPooledObject(Tags.Explosion);
        explosion.transform.position = position;
        explosion.SetActive(true);
        explosions.Add(explosion);
    }

    void DisableExplosions()
    {
        for (int i = 0; i < explosions.Count; i++)
        {
            explosions[i].SetActive(false);
        }
        explosions.Clear();
    }

}
