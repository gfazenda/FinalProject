using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overcharge : Skill {
    public int range = 2, turnPenalty = 2;
	// Use this for initialization
	void Start () {
        base.Start();
        description = "Attack in area that affects up to 2 tiles of distance from you in all directions. \nOnce used you lose 3 rounds to recharge the robot`s battery.";
        Debug.Log("descccc");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void DoEffect(Coord position)
    {
        Debug.Log("ooooooo");
        BoardManager.tileType[] types = { BoardManager.tileType.enemy, BoardManager.tileType.ground };
        List<KeyValuePair<BoardManager.tileType, Coord>> neighbours = BoardManager.Instance.GetNeighbours(position, range, types, true);
        foreach (KeyValuePair<BoardManager.tileType, Coord> t in neighbours)
        {
            BoardManager.Instance.InstantiateEffect(Tags.ElectricExplosion, t.Value);
            if (t.Key == BoardManager.tileType.enemy)
                EnemyCoordinator.Instance.EnemyDamaged(damage, t.Value);
        }
    }

    public Overcharge()
    {

    }
}
