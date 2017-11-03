using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : Skill {

    GameObject newMine;

    public override void DoEffect(Coord position) {
        Debug.Log("minneeee");
        newMine = ObjectPooler.SharedInstance.GetPooledObject(Tags.Mine);
        newMine.transform.position = BoardManager.Instance.CoordToPosition(position,false);
        newMine.GetComponent<RemoteMine>().SetPosition(position);
        newMine.gameObject.SetActive(true);
    }
}
