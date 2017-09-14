using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    float delay;
    List<GameObject> enemies;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        EventManager.StartListening(Events.EnemiesTurn, CallEnemyactions);
        EventManager.StartListening(Events.EnemiesCreated, PopulateEnemies);
    }

    private void PopulateEnemies()
    {
        enemies = new List<GameObject>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag(Tags.Enemy));
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Enemy>().Initialize();
        }
        delay = (0.3f / enemies.Count);
    }

    public void EnemyDamaged(int damage, Coord position)
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

    void CallEnemyactions()
    {
        StartCoroutine(DoEnemiesAction());
        Debug.Log("enemies doing");
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


        //foreach (GameObject enemy in enemies)
        //{

        //    enemy.GetComponent<Enemy>().DoAction();
        //    yield return new WaitForSeconds(delay);
        //}
        EventManager.TriggerEvent(Events.PlayerTurn);
        Debug.Log("player doing");
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
