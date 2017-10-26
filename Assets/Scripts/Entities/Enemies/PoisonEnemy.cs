using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEnemy : Enemy {

    //List<GameObject> poisonTiles = new List<GameObject>();
    GameObject poisonPrefab;
    int currentIndex = 0;


    private void Start()
    {
        base.Start();
    }


    protected override void PerformMove()
    {
        poisonPrefab = null;
        poisonPrefab = ObjectPooler.SharedInstance.GetPooledObject(Tags.Poison);
        if(poisonPrefab != null)
        {
            poisonPrefab.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            poisonPrefab.GetComponent<SpecialTile>().SetPosition(position);
            poisonPrefab.SetActive(true);
        }
        base.PerformMove();
    }
}
