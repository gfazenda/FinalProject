public class SpellBlocker : Obstacle {

    float currentDistance = 10f;

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
