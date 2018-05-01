using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoordinator : MonoBehaviour {
    private static EnemyCoordinator _instance;

    public static EnemyCoordinator Instance { get { return _instance; } }
    bool configured = false;
    public bool enemiesTurn = false, useMinimax = false;
    public float turnDelay = 0.3f;
    float delay;

    public List<GameObject> enemies = new List<GameObject>();
    AStar _astar;
    EnemyMinimax _minimax;
    // Use this for initialization
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this);

        if (!configured)
            Configure();

        Initialize();
    }

    private void Configure()
    {
        configured = true;
        _astar = this.GetComponent<AStar>();
        _minimax = this.GetComponent<EnemyMinimax>();
    }

    public void RereadEnemies()
    {
        enemies.Clear();
        enemies.AddRange(GameObject.FindGameObjectsWithTag(Tags.Enemy));
        InitializeEnemies();
        UpdateDelayTime();
    }


    public List<Coord> GetPath(Coord enemyPosition, Coord playerPosition)
    {
        return _astar.FindPath(enemyPosition, playerPosition);
    }

    private void Initialize()
    {
        enemies = new List<GameObject>();
        EventManager.StartListening(Events.EnemiesTurn, CallEnemyActions);
        EventManager.StartListening(Events.EnemiesCreated, PopulateEnemies);
    }

    private void PopulateEnemies()
    {
        enemies.Clear();// = new List<GameObject>();
        //enemies.AddRange(GameObject.FindGameObjectsWithTag(Tags.Enemy));
        enemies = BoardManager.Instance.listOfEnemies;
        InitializeEnemies();
        // delay = (delayPerEnemy / enemies.Count);
        UpdateDelayTime();
        //Debug.Log("got enemies " + enemies.Count);
    }

    void InitializeEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Enemy>().Initialize();
        }
    }

    public float GetEnemyTurnDuration()
    {
        return turnDelay;
    }

    void CallEnemyActions()
    {
        enemiesTurn = true;
        //   Debug.Log("enemies doing");
        if (enemies.Count > 0)
            StartCoroutine(DoEnemiesAction());
        else
        {
            // Invoke("CallPlayerTurn", 0.1f);
            CallPlayerTurn();
        }

    }

    void CallPlayerTurn()
    {
        enemiesTurn = false;
        EventManager.TriggerEvent(Events.PlayerTurn);
    }

    void UpdateDelayTime()
    {
        delay = (0.4f / (1 + enemies.Count));
    }
    // Use this for initialization
    IEnumerator DoEnemiesAction()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].activeInHierarchy)
            {
                enemies.RemoveAt(i);
                UpdateDelayTime();
                continue;
            }
            if (useMinimax)
            {
                MiniMax.Instance.CheckNextAction(enemies[i]);
            }
            else
            {
                enemies[i].GetComponent<Enemy>().DoAction();
            }
           

        }
        yield return new WaitForSeconds(turnDelay);
        Debug.Log("player doing");
        CallPlayerTurn();
    }

    public void EnemyDamaged(float damage, Coord position)
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>().position.CompareTo(position))
            {
                enemy.GetComponent<Enemy>().TakeDamage(damage);
                return;
            }
        }
    }

    public void EnemyParalyzed(int turns, Coord position)
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>().position.CompareTo(position))
            {
                enemy.GetComponent<Enemy>().SetTurnPenalty(turns);
                return;
            }
        }
    }

    public bool HasEnemies()
    {
        return enemies.Count != 0;
    }

}
