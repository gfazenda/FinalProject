using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : Skill {

    GameObject newMine;

    private void Start()
    {
        base.Start();
        description = "Powerful mine that explodes and affects all the tiles around it. Please chose where to plant it and it will explode after a number of turns";
    }

    public override void DoEffect(Coord position) {
        Debug.Log("minneeee");
        newMine = ObjectPooler.SharedInstance.GetPooledObject(Tags.Mine);
        newMine.transform.position = BoardManager.Instance.CoordToPosition(position,false);
        newMine.GetComponent<RemoteMine>().SetPosition(position);
        newMine.gameObject.SetActive(true);
    }
}
