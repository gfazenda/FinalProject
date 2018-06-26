using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour {

    public AudioMixer mixer;

    public GameObject soundOn, soundOff;
    float currVolume;
    public void Start()
    {
        mixer.GetFloat("Volume", out currVolume);
        if(currVolume == -80)
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
    }

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

    public void LoadSurvey()
    {
        Application.OpenURL("https://goo.gl/forms/1NrHrbTpGXzFeRz42");
    }

    public void DisableAudio()
    {
        mixer.SetFloat("Volume", -80);
    }

    public void EnableAudio()
    {
        mixer.SetFloat("Volume", 0);
    }
}
