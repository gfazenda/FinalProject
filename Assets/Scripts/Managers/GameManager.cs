using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public float delayPerEnemy;
    public int maxFrameRate = 60;

    float delay;
    
    List<GameObject> enemies = new List<GameObject>();

    public int currentLevel = 1;
    bool initialized = false;
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = maxFrameRate;
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this);

        //EventManager.StartListening(Events.EnemiesTurn, CallEnemyActions);
        //EventManager.StartListening(Events.EnemiesCreated, PopulateEnemies);

        //EventManager.StartListening(Events.LevelWon, NextLevel);
        //EventManager.StartListening(Events.LevelLost, ReloadLevel);

        if (!initialized)
            Initialize();

        SceneManager.sceneLoaded += InitializeLevel;
    }


    private void Initialize()
    {
        EventManager.StartListening(Events.EnemiesTurn, CallEnemyActions);
        EventManager.StartListening(Events.EnemiesCreated, PopulateEnemies);

        EventManager.StartListening(Events.LevelWon, NextLevel);
        EventManager.StartListening(Events.LevelLost, ReloadLevel);

        initialized = true;
    }

    void Unsubscribe()
    {
        EventManager.StopListening(Events.EnemiesTurn, CallEnemyActions);
        EventManager.StopListening(Events.EnemiesCreated, PopulateEnemies);

        EventManager.StopListening(Events.LevelWon, NextLevel);
        EventManager.StopListening(Events.LevelLost, ReloadLevel);
    }


    void NextLevel()
    {
        Debug.Log("load");
        currentLevel++;
        enemies = new List<GameObject>();
      //  Unsubscribe();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void ReloadLevel()
    {
        Debug.Log("reload");
        enemies = new List<GameObject>();
     //   Unsubscribe();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void InitializeLevel(Scene scene, LoadSceneMode mode)
    {
        //BoardManager.Instance.DoInitialize();
        EventManager.TriggerEvent(Events.LevelLoaded);
    }




    private void PopulateEnemies()
    {
        enemies.Clear();// = new List<GameObject>();
        //enemies.AddRange(GameObject.FindGameObjectsWithTag(Tags.Enemy));
        enemies = BoardManager.Instance.listOfEnemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Enemy>().Initialize();
        }
        delay = (delayPerEnemy / enemies.Count);
        Debug.Log("got enemies " + enemies.Count);
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

    public float GetEnemyTurnDuration()
    {
        return (delayPerEnemy * enemies.Count);
    }

    void CallEnemyActions()
    {
        Debug.Log("enemies doing");
        if (enemies.Count > 0)
            StartCoroutine(DoEnemiesAction());
        else
        {
            Debug.Log("player doing");
            EventManager.TriggerEvent(Events.PlayerTurn);
        }

    }


    void UpdateDelayTime()
    {
        delay = (0.3f / enemies.Count);
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
            enemies[i].GetComponent<Enemy>().DoAction();
            yield return new WaitForSeconds(delay);

        }
        Debug.Log("player doing");
        EventManager.TriggerEvent(Events.PlayerTurn);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.TriggerEvent(Events.PlayerTurn);
        }else if (Input.GetKeyDown(KeyCode.X))
            {
                EventManager.TriggerEvent(Events.EnemiesTurn);
            }
        
    }

}
