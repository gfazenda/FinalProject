using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {
    bool poisoned = false, burned = false;
    public enum statuses { poisoned, burned};
    int poisonTurns = 0, poisonDuration = 3, poisonDMG = 5;
    Player _pScript;
    // Use this for initialization
    void Start()
    {
        _pScript = this.GetComponent<Player>();
        EventManager.StartListening(Events.PlayerTurn, NewTurn);
    }

    void NewTurn()
    {
        if (poisoned)
        {
            _pScript.TakeDamage(poisonDMG);
            DecreasePoison();
        }
    }

    void DecreasePoison()
    {
        poisonTurns -= 1;
        if(poisonTurns <= 0)
        {
            poisoned = false;
        }
    }

    public void SetStatus(statuses status)
    {
        switch (status)
        {
            case statuses.poisoned:
                {
                    if (!poisoned)
                    {
                        poisoned = true;
                        poisonTurns = poisonDuration;
                    }else
                    {
                        poisonTurns += 1;
                    }
                }
                break;
            case statuses.burned:
                break;
            default:
                break;
        }

    }

}
