using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    
    public int maxFrameRate = 30;

    [HideInInspector]
    public TextFileReader _fileReader;

    public bool loadPlayerLevel = true;
    

    public int currentLevel = 1;

    bool initialized = false, firstLevelPlayed = true;

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
        _fileReader = this.GetComponent<TextFileReader>();
        //EventManager.StartListening(Events.EnemiesTurn, CallEnemyActions);
        //EventManager.StartListening(Events.EnemiesCreated, PopulateEnemies);

        //EventManager.StartListening(Events.LevelWon, NextLevel);
        //EventManager.StartListening(Events.LevelLost, ReloadLevel);

        if (!initialized)
        {
            Initialize();
            //print("do loaginddfsfdf");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EventManager.TriggerEvent(Events.LevelLoaded);
    }

    private void Initialize()
    {
        EventManager.StartListening(Events.LevelWon, NextLevel);
        EventManager.StartListening(Events.LevelLost, ReloadLevel);

        initialized = true;
    }

    void Unsubscribe()
    {
        EventManager.StopListening(Events.LevelWon, NextLevel);
        EventManager.StopListening(Events.LevelLost, ReloadLevel);
    }

    public void ShakeCamera()
    {
        CameraShaker.Instance.ShakeOnce(6f, 4f, .1f, 1f);
    }


    public LevelInformation LoadLevelFile()
    {
        if (loadPlayerLevel && firstLevelPlayed)
        {
            int levelFromFile = _fileReader.ReadCurrentLevel();
            currentLevel = levelFromFile == -1 ? currentLevel : levelFromFile;
            firstLevelPlayed = false;
        }
        UXManager.instance.ShowLevelOverlay();
        return _fileReader.LoadFile("Level" + currentLevel + ".txt");
    }

    public void ResetLevelInfo()
    {
        PlayerPrefs.SetInt(Prefs.Level, 1);
    }

    void NextLevel()
    {
        Debug.Log("load");

        currentLevel++;
        _fileReader.SaveLevel(currentLevel);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ReloadLevel()
    {
        Debug.Log("reload");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void BackToMenu()
    {
        Debug.Log("back");
        SceneManager.LoadScene(Scenes.MainMenu);
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
    }




    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            NextLevel();
        }

    }

}
