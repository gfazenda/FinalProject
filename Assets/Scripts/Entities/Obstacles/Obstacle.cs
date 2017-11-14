using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : SpecialTile {
    public int radius;
    public GameObject tile;
    //  List<GameObject> tiles = new List<GameObject>();
    BoardManager.tileType currTile;
    protected float maxEffectRange;
    protected Player player = null;
    protected bool affectedPlayer = false;
    // Use this for initialization
    void Start () {
		player = BoardManager.Instance._playerScript;
        maxEffectRange = ((float)radius * Utility.distanceMultiplier);
        CreateEffectTiles();
	}

    protected void CreateEffectTiles()
    {
        Coord currPos = new Coord(position);
        for (int i = position.x - radius; i <= position.x + radius; i++)
        {
            for (int j = position.y - radius; j <= position.y + radius; j++)
            {
                currPos.x = i;
                currPos.y = j;
                currTile = BoardManager.Instance.GetPositionType(currPos);
                if (currTile != (BoardManager.tileType.outOfLimits) && currTile != (BoardManager.tileType.wall) && currTile != (BoardManager.tileType.obstacle))
                {
                    Instantiate(tile, BoardManager.Instance.CoordToPosition(currPos), Quaternion.identity, this.transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
