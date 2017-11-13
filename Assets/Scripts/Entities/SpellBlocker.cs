using UnityEngine;
using System.Collections.Generic;

public class SpellBlocker : SpecialTile {

    public GameObject tile;
    public int effectRadius = 1;
    float maxEffectRange;
    Player player = null;
    bool affectedPlayer = false;
    int instances = 0;
    float currentDistance = 10f, damageRemoved = 0, endingDamage = 0, currentDrain = 0;
    List<GameObject> tiles = new List<GameObject>();

    void Start()
    {
        player = BoardManager.Instance._playerScript;
        maxEffectRange = ((float)effectRadius * Utility.distanceMultiplier);
        CreateEffectTiles();
    }

    void CreateEffectTiles()
    {
        Coord currPos = new Coord(position);
        BoardManager.tileType currTile;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                currPos.x = position.x + i;
                currPos.y = position.y + j;
                currTile = BoardManager.Instance.GetPositionType(currPos);
                if (currTile != BoardManager.tileType.outOfLimits && currTile != BoardManager.tileType.obstacle)
                {
                    tiles.Add(Instantiate(tile, BoardManager.Instance.CoordToPosition(currPos), Quaternion.identity,this.transform));
                }
            }
        }
    }


    private void OnEnable()
    {
        EventManager.StartListening(Events.EnemiesTurn, BlockSpells);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.EnemiesTurn, BlockSpells);
    }


    void BlockSpells()
    {
        if (!gameObject.activeInHierarchy)
            return;

        currentDistance = BoardManager.Distance(position, player.GetPosition());
        if (currentDistance <= maxEffectRange)
        {
            if (!affectedPlayer) { 
                DoBlock();
            }

        }else if (affectedPlayer)
        {
            UnblockSpells();
        }
    }



    private void DoBlock()
    {
        print("blocking");
        affectedPlayer = true;
        player.BlockSpells(affectedPlayer);
    }

    void UnblockSpells()
    {
        print("unblocking");
        affectedPlayer = false;
        player.BlockSpells(affectedPlayer);
    }

}
