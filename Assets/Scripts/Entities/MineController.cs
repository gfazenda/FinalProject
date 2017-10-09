using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour {
    public GameObject minePrefab;
    GameObject newMine;

    public void PlaceMine(Coord position){
        newMine = ObjectPooler.SharedInstance.GetPooledObject(Tags.Mine);
        newMine.transform.position = BoardManager.Instance.CoordToPosition(position,false);
        newMine.GetComponent<RemoteMine>().SetPosition(position);
        newMine.gameObject.SetActive(true);
    }
}
