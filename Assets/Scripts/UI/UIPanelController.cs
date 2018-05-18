using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelController : MonoBehaviour {

    public List<GameObject> panels = new List<GameObject>();
    int index = 0;
    public Button nextPanel, play;

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
        if(index < panels.Count)
            panels[index].SetActive(true);

        if(index == panels.Count-1)
        {
            nextPanel.gameObject.SetActive(false);
            play.gameObject.SetActive(true);
        }
    }
}
