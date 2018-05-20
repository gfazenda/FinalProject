using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void LoadGame()
    {
        SceneManager.LoadScene(Scenes.Gameplay);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(Scenes.Gameplay);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
