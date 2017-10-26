using UnityEngine;

public class SpellBlocker : SpecialTile {

   
    public int effectRadius = 1;
    float maxEffectRange;
    Player player = null;
    bool affectedPlayer = false;
    int instances = 0;
    float currentDistance = 10f, damageRemoved = 0, endingDamage = 0, currentDrain = 0;
   
    void Start()
    {
        player = BoardManager.Instance._playerScript;
        maxEffectRange = ((float)effectRadius * Utility.distanceMultiplier);
        position = new Coord(0, 0);
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
                affectedPlayer = true;
                DoBlock();
            }

        }
        else if (affectedPlayer)
        {
            UnblockSpells();
        }
    }



    private void DoBlock()
    {
        print("blocking");
        player.BlockSpells(true);
    }

    void UnblockSpells()
    {
        print("unblocking");
        affectedPlayer = false;
        player.BlockSpells(false);
    }

}
