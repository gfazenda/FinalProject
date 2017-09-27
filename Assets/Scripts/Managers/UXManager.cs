using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UXManager : MonoBehaviour {
    private static UXManager _instance;

    public static UXManager instance { get { return _instance; } }


    public enum textOption {middle,top,bottom};


    public GameObject middleObj, topObj, bottomObj;
    TextMeshProUGUI middleText, topText, bottomText;

    public Button overcharge, mine;

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

        
        
        ConfigureTexts();
        ConfigureButtons();
        
       // DontDestroyOnLoad(this);
        EventManager.StartListening(Events.LevelLoaded, ShowLevelOverlay);
        EventManager.StartListening(Events.EnemiesTurn, DisableButtons);
        EventManager.StartListening(Events.PlayerTurn, EnableButtons);
    }

    void DisableButtons()
    {
        Debug.Log("disable");
        overcharge.interactable = false;
        mine.interactable = false;
    }

    void EnableButtons()
    {
        Debug.Log("enable");
        overcharge.interactable = true;
        mine.interactable = true;
    }


    void ConfigureButtons()
    {
        overcharge.onClick.AddListener(CallOvercharge);
        mine.onClick.AddListener(CallPlaceMines);
    }

    void ConfigureTexts()
    {
        middleText = middleObj.GetComponent<TextMeshProUGUI>();
        topText = topObj.GetComponent<TextMeshProUGUI>();

        middleText.text = "";
        topText.text = "";
    }

    void ShowLevelOverlay()
    {
        DisplayMessage("Level " + GameManager.Instance.currentLevel, 2f, textOption.top);
    }

    void CallOvercharge()
    {
        BoardManager.Instance._playerScript.DoOvercharge();
    }

    void CallPlaceMines()
    {
        BoardManager.Instance._playerScript.ShowMineMarkers();
    }

    // Update is called once per frame
    public void DisplayMessage (string msg, float duration, textOption option = textOption.middle) {
        StartCoroutine(ShowMessage(msg, duration, option));
    }

    IEnumerator ShowMessage(string message, float delay, textOption option = textOption.middle)
    {
        TextMeshProUGUI currentText = null;
        switch (option)
        {
            case textOption.middle:
                currentText = middleText;
                break;
            case textOption.top:
                currentText = topText;
                break;
            case textOption.bottom:
                break;
            default:
                break;
        }



        currentText.text = message;
        currentText.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        currentText.gameObject.SetActive(false);
    }
}
