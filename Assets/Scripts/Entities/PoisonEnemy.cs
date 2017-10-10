using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEnemy : Enemy {

    //List<GameObject> poisonTiles = new List<GameObject>();
    GameObject poisonPrefab;
    int currentIndex = 0;


    private void Start()
    {
        //poisonTiles.Add(GameObject.Instantiate(poisonPrefab));
        //poisonTiles.Add(GameObject.Instantiate(poisonPrefab));
        //poisonTiles[0].SetActive(false);
        //poisonTiles[1].SetActive(false);
        base.Start();
    }


    protected override void PerformMove()
    {
        //if (currentIndex == 0){
        poisonPrefab = null;
        poisonPrefab = ObjectPooler.SharedInstance.GetPooledObject(Tags.Poison);
        if(poisonPrefab != null)
        {
            poisonPrefab.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            poisonPrefab.GetComponent<SpecialTile>().SetPosition(position);
            poisonPrefab.SetActive(true);
        }
        //   poisonTiles[currentIndex].transform.position = new Vector3(transform.position.x,0.5f,transform.position.z);
        //    poisonTiles[currentIndex].SetActive(true);
            //currentIndex++;
            //if (currentIndex > poisonTiles.Count - 1)
            //    currentIndex = 0;
       // }
        base.PerformMove();
    }
}
