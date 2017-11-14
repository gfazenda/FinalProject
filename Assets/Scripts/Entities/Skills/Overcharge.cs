using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overcharge : Skill {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void DoEffect(Coord position)
    {
        Debug.Log("ooooooo");
        BoardManager.tileType[] types = { BoardManager.tileType.enemy, BoardManager.tileType.ground };
        List<KeyValuePair<BoardManager.tileType, Coord>> neighbours = BoardManager.Instance.GetNeighbours(position, 2, types, true);
        foreach (KeyValuePair<BoardManager.tileType, Coord> t in neighbours)
        {
            BoardManager.Instance.InstantiateEffect(Tags.ElectricExplosion, t.Value);
            if (t.Key == BoardManager.tileType.enemy)
                EnemyCoordinator.Instance.EnemyDamaged((damage), t.Value);
        }
    }

    public Overcharge()
    {

    }
}
