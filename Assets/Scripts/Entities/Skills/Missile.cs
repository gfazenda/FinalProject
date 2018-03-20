using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Skill {

    int turnsToImpact = 1;
    int currentTurns = 0;
    Coord targetPosition;
    public GameObject missile, target;
    GameObject instantiatedMissile, instantiatedTarget;
    Coord OutOfLimits = new Coord();
    private void Start()
    {
        base.Start();
        description = "Deadly missile that lands one turn after being launched. It kills any enemy and leaves a hole in the board.";
    }

    public override void DoEffect(Coord position)
    {
        EventManager.StartListening(Events.PlayerTurn, LaunchMissile);
        currentTurns = turnsToImpact;
        targetPosition = position;
        instantiatedMissile = null;
        //do not instantiate anymore, just enable/disable plz
        instantiatedMissile = Instantiate(missile, BoardManager.Instance._playerScript.transform.position, Quaternion.identity);
        instantiatedTarget = Instantiate(target, BoardManager.Instance.CoordToPosition(position), Quaternion.identity);// Quaternion.Euler(90,0,0));
        RepositionMissile();
    }

    //private void FixedUpdate()
    //{
    //    Vector3 missilePos = instantiatedMissile.transform.position;
    //}

    void RepositionMissile()
    {
        Vector3 newPosition = BoardManager.Instance.CoordToPosition(targetPosition);
        newPosition.y = 15;
        instantiatedMissile.GetComponent<MissileObject>().setCoordinates(targetPosition, damage);
        instantiatedMissile.GetComponent<MissileObject>().GoToTarget(newPosition);
        //instantiatedMissile.transform.position = newPosition;
    }

    void LaunchMissile()
    {
        Debug.Log("missile in the works");

        if(currentTurns <= 0)
        {
            Debug.Log("wowoow");
           
          //  BoardManager.Instance.InstantiateEffect(Tags.Explosion, targetPosition);
            EventManager.StopListening(Events.PlayerTurn, LaunchMissile);

            BoardManager.Instance._playerScript.invalidPos = OutOfLimits;
            instantiatedMissile.GetComponent<MissileObject>().LookDown();
            instantiatedMissile.GetComponent<MissileObject>().GoToTarget(BoardManager.Instance.CoordToPosition(targetPosition));
            Destroy(instantiatedTarget);
        }
        currentTurns--;
    }
}
