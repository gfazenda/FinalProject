using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {
  

        bool gameIsPaused = false;
    public GameObject pausePanel, pauseButton;

        public void Pause()
        {
            gameIsPaused = !gameIsPaused;
            Time.timeScale = gameIsPaused ? 0 : 1;
            UXManager.instance.GamePaused(gameIsPaused);
            EventManager.TriggerEvent(Events.GamePaused);
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

