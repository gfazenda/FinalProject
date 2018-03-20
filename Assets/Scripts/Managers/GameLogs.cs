using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogs : MonoBehaviour {
    private static GameLogs _instance;

    public static GameLogs Instance { get { return _instance; } }

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
    }

    public enum logType { plyerDamage, playerMovement, enemyDamage, skillUsed, enemyKilled, playerKilled}

    List<GameLog> logs = new List<GameLog>();

    struct GameLog
    {
        public logType type;
        public string info;
        public int value;
    }

    GameLog currentLog;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddLog(logType type, string info = "", int value = -111)
    {
        currentLog.type = type;
        currentLog.info = info;
        currentLog.value = value;

        logs.Add(currentLog);
       // Debug.Log(logs.ToString());
    }
}
