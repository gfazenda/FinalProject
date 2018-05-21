using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEnemy : Enemy
{

    //List<GameObject> poisonTiles = new List<GameObject>();
    GameObject poisonPrefab;
    int currentIndex = 0;


    private void Start()
    {
        base.Start();
    }

    void PlacePoisonTile(GameObject poisonTile)
    {
        poisonTile.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        poisonTile.GetComponent<SpecialTile>().SetPosition(position);
        poisonTile.SetActive(true);
    }


    public override void PerformMove(Coord destination)
    {
        poisonPrefab = null;
        poisonPrefab = ObjectPooler.SharedInstance.GetPooledObject(Tags.Poison);
        if (poisonPrefab != null)
        {
            PlacePoisonTile(poisonPrefab);
        }
        base.PerformMove(destination);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (_entityScript.Dead())
        {
            PlacePoisonTile(ObjectPooler.SharedInstance.GetPooledObject(Tags.Poison));
        }
    }
}
