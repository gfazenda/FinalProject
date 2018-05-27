using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelController : MonoBehaviour {

    public List<GameObject> panels = new List<GameObject>();
    int index = 0;
    public Button nextPanelButton, backPanelButton, play;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void NextPanel()
    {
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

        if (index == 0)
        {
            backPanelButton.gameObject.SetActive(false);
        }
    }


    public void LoadGame()
    {
        GameManager.Instance.LoadGameplay();
    }
}
