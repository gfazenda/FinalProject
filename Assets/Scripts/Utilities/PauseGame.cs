using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {
    private static PauseGame _instance;

    public static PauseGame instance { get { return _instance; } }

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
        gameIsPaused = false;
    }
        public bool gameIsPaused { get; private set; }
        public GameObject pausePanel, pauseButton;

        public void Pause(bool showPanel = true)
        {
            gameIsPaused = !gameIsPaused;
            Time.timeScale = gameIsPaused ? 0 : 1;
            UXManager.instance.GamePaused(gameIsPaused);
            EventManager.TriggerEvent(Events.GamePaused);

            if (showPanel)
               pausePanel.gameObject.SetActive(gameIsPaused);

            pauseButton.gameObject.SetActive(!gameIsPaused);
        }

        public void Restart()
        {
            Pause();
            GameManager.Instance.ReloadLevel();
        }

        public void BackToMenu()
        {
            Pause();
            GameManager.Instance.BackToMenu();
        }


}

