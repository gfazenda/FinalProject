using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelController : MonoBehaviour {

    public List<GameObject> en_panels = new List<GameObject>();
    public List<GameObject> ptbr_panels = new List<GameObject>();
    List<GameObject> panels = new List<GameObject>();
    int index = 0;
    public Button nextPanelButton, backPanelButton, play;
    public bool english = true;

    bool initialized = false;


    public void SetLanguage(bool en)
    {
        english = en;
        initialized = false;
    }


    public void NextPanel()
    {
        if (!initialized)
        {
            panels = english ? en_panels : ptbr_panels;
            initialized = true;
        }
        panels[index].SetActive(false);
        index++;
        if(!backPanelButton.isActiveAndEnabled)
            backPanelButton.gameObject.SetActive(true);

        if (index < panels.Count)
            panels[index].SetActive(true);

        if(index == panels.Count-1)
        {
            nextPanelButton.gameObject.SetActive(false);
            play.gameObject.SetActive(true);
        }
    }

    public void PreviousPanel()
    {
        panels[index].SetActive(false);
        index--;

        if(play.isActiveAndEnabled)
            play.gameObject.SetActive(false);

        if (!nextPanelButton.isActiveAndEnabled)
            nextPanelButton.gameObject.SetActive(true);


        if (index >= 0)
            panels[index].SetActive(true);

        if (index == 1)
        {
            backPanelButton.gameObject.SetActive(false);
        }
    }


    public void LoadGame()
    {
        GameManager.Instance.LoadGameplay();
    }
}
