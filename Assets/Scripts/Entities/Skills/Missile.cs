using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Skill {

    public int turnsToImpact = 1;
    int currentTurns = 0;
    bool initialized = false;
    Coord targetPosition;
    public GameObject missile, target;
    GameObject instantiatedMissile, instantiatedTarget = null;
    Vector3 impactPosition;
    Coord OutOfLimits = new Coord(-1,-1);
    private void Start()
    {
        base.Start();
        description = "Deadly missile that lands one turn after being launched. It kills any enemy and leaves a hole in the board.";
    }

    void Initialize(){
         instantiatedTarget = Instantiate(target, new Vector3(200,200,200), Quaternion.identity);
         instantiatedMissile = Instantiate(missile, new Vector3(200,200,200), Quaternion.identity);
    }

    public override void DoEffect(Coord position)
    {
        if(!initialized)
            Initialize();

        EventManager.StartListening(Events.PlayerTurn, LaunchMissile);
        currentTurns = turnsToImpact;
        targetPosition = position;

        impactPosition = BoardManager.Instance.CoordToPosition(targetPosition);
        
        instantiatedMissile.transform.position = (BoardManager.Instance._playerScript.transform.position);
        instantiatedMissile.SetActive(true);

        instantiatedTarget.transform.position = impactPosition;
        instantiatedTarget.SetActive(true);
        
        RepositionMissile();
    }

    //private void FixedUpdate()
    //{
    //    Vector3 missilePos = instantiatedMissile.transform.position;
    //}

    void RepositionMissile()
    {
        Vector3 newPosition = impactPosition;
        newPosition.y = 15;
        instantiatedMissile.GetComponent<MissileObject>().setCoordinates(targetPosition, damage);
        instantiatedMissile.GetComponent<MissileObject>().FlyAway();
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
            //instantiatedMissile.GetComponent<MissileObject>().GoToTarget(impactPosition);
             instantiatedMissile.GetComponent<MissileObject>().GoToTarget();
            //Destroy(instantiatedTarget);
            instantiatedTarget.SetActive(false);
        }
        currentTurns--;
    }
}
