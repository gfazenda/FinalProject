using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UXManager : MonoBehaviour {
    private static UXManager _instance;

    public static UXManager instance { get { return _instance; } }


    public enum textOption {middle,top,bottom};
    public enum msgType { info, warning };

    public GameObject middleObj, topObj, bottomObj, dmgObj, hpObj, manaObj;
    TextMeshProUGUI middleText, topText, bottomText, dmgText;

    HealthBar hpScript, manaScript;
    bool skillsEnabled = true;
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
        hpScript = hpObj.GetComponent<HealthBar>();
        manaScript = manaObj.GetComponent<HealthBar>();
        // DontDestroyOnLoad(this);
        EventManager.StartListening(Events.LevelLoaded, ShowLevelOverlay);
        EventManager.StartListening(Events.DamageUpdate, UpdatePlayerDamage);
        EventManager.StartListening(Events.EnemiesTurn, DisableButtons);
       // EventManager.StartListening(Events.PlayerTurn, EnableButtons);
    }


    public void UpdatePlayerHP(float value)
    {
        hpScript.UpdateBar(value);
    }


    public void UpdatePlayerMana(float value)
    {
        manaScript.UpdateBar(value);
    }

    void UpdatePlayerDamage()
    {
        dmgText.text = "Damage: " + System.Math.Round(BoardManager.Instance._playerScript.damage, 1);
    }

    void DisableButtons()
    {
        Debug.Log("disable");
        overcharge.interactable = false;
        mine.interactable = false;
    }

    public void EnableButtons()
    {
        Debug.Log("enable");
      //  skillsEnabled = BoardManager.Instance._playerScript.CanAct();
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
        dmgText = dmgObj.GetComponent<TextMeshProUGUI>();
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
