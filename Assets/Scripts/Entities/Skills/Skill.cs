using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {
    public string name;
    public int manacost;
    public int damage;
    public int cooldown;
    int currentCD;

    private void Start()
    {
        EventManager.StartListening(Events.PlayerTurn, DecreaseTurn);
    }

    void DecreaseTurn()
    {
        if (currentCD > 0)
            currentCD--;
    }

    public bool IsAvailable()
    {
        return (currentCD == 0);
    }


    public virtual void DoEffect(Coord position)
    {
        currentCD = cooldown;
    }

}
