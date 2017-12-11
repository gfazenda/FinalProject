using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {
    public List<GameObject> objsToDeactivate = new List<GameObject>();

        bool gameIsPaused = false;

        void Start()
        {
            this.gameObject.SetActive(!gameIsPaused);
        }


        public void Pause()
        {
            Time.timeScale = gameIsPaused ? 0 : 1;
            this.gameObject.SetActive(gameIsPaused);
            ChangeUIObjects();
            //Disable scripts that still work while timescale is set to 0
        }

        private void ChangeUIObjects(){
            for (int i = 0; i < objsToDeactivate.Count; i++)
            {
                objsToDeactivate[i].SetActive(gameIsPaused);
            }
        }
}

