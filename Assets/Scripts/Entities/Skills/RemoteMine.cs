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
        BoardManager.Instance.InstantiateEffect(Tags.Explosion, position);
        BoardManager.tileType[] types = { BoardManager.tileType.enemy, BoardManager.tileType.ground};
        List<KeyValuePair<BoardManager.tileType, Coord>> neighbours = BoardManager.Instance.GetNeighbours(position, 1, types, true);
        foreach (KeyValuePair<BoardManager.tileType, Coord> t in neighbours)
        {
            BoardManager.Instance.InstantiateEffect(Tags.Explosion, t.Value);
            if (t.Key == BoardManager.tileType.enemy)
                EnemyCoordinator.Instance.EnemyDamaged(damage, t.Value);
        }
    }

    private void InstantiateExplosion(Vector3 position)
    {
        explosion = ObjectPooler.SharedInstance.GetPooledObject(Tags.Explosion);
        explosion.transform.position = position;
        explosion.SetActive(true);
    }

}
