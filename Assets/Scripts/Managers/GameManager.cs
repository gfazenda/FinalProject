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
    bool showedFeedback = false;

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
        if(currentLevel == BoardManager.Instance.preMadeLevels + 1 && !showedFeedback)
        {
            Debug.Log("showing");
            showedFeedback = true;
            PauseGame.instance.Pause(false);
            UXManager.instance.ShowFeedbackCongrats();
            
        }
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
        if (firstLevelPlayed && PlayerPrefs.GetInt(Prefs.SeenTutorial, 0) == 0)
        {
            PlayerPrefs.SetInt(Prefs.SeenTutorial, 1);
            SceneManager.LoadScene(Scenes.Tutorial);
            
            return null;
        }

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
        PlayerPrefs.SetInt(Prefs.SeenTutorial, 0);
    }

    void NextLevel()
    {
        Debug.Log("load");

        if (currentLevel == BoardManager.Instance.preMadeLevels)
        {
            Debug.Log("you made it lol");
        }


        currentLevel++;
        _fileReader.SaveLevel(currentLevel);

        SceneManager.LoadScene(Scenes.Gameplay);
    }

    public void ReloadLevel()
    {
        Debug.Log("reload");
        if (currentLevel > BoardManager.Instance.preMadeLevels)
        {
            currentLevel = BoardManager.Instance.preMadeLevels+1;
        }

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

    public void LoadGameplay()
    {
        SceneManager.LoadScene(Scenes.Gameplay);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            NextLevel();
        }

    }

}
