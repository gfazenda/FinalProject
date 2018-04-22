using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UXManager : MonoBehaviour {
    private static UXManager _instance;

    public static UXManager instance { get { return _instance; } }


    public enum textOption { middle, top, bottom };
    public enum msgType { info, warning };

    public enum moveDirection { Left, Right, Up, Down };

    public GameObject middleObj, topObj, bottomObj, dmgObj, hpObj, manaObj;
    TextMeshProUGUI middleText, topText, bottomText, dmgText;

    HealthBar hpScript, manaScript;
    bool skillsEnabled = true;
    public Button overcharge, mine, missile, path, cancelPath;

    public Button left,right,top, down;

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

        EventManager.StartListening(Events.LevelLoaded, ShowLevelOverlay);
        EventManager.StartListening(Events.DamageUpdate, UpdatePlayerDamage);
        EventManager.StartListening(Events.EnemiesTurn, DisableButtons);
        EventManager.StartListening(Events.DisableMoveButtons, () => ChangeMoveButtons(false));
        EventManager.StartListening(Events.EnableMoveButtons, () => ChangeMoveButtons(true));
    }


    public void UpdatePlayerHP(float hp, float maxhp)
    {
        hpScript.UpdateBarWithText(hp, maxhp);
    }


    public void UpdatePlayerMana(int mana, int maxMana)
    {
        manaScript.UpdateBarWithText(mana, maxMana);
    }

    void UpdatePlayerDamage()
    {
        dmgText.text = System.Math.Round(BoardManager.Instance._playerScript.damage, 1).ToString();
    }

    public bool TouchOverUI(int id)
    {
        return EventSystem.current.IsPointerOverGameObject(id) || EventSystem.current.currentSelectedGameObject != null;
    }

    public void DisableButtons()
    {
        if (!EnemyCoordinator.Instance.enemiesTurn && !BoardManager.Instance._playerScript.spellsBlocked)
            return;

        overcharge.interactable = false;
        mine.interactable = false;
        missile.interactable = false;
    }

    public void GamePaused(bool paused)
    {
        overcharge.interactable = !paused;
        mine.interactable = !paused;
        missile.interactable = !paused;
        BoardManager.Instance.DisableMarkers();
    }

    public void ChangeMoveButtons(bool activate)
    {
        return;
        left.gameObject.SetActive(activate);
        right.gameObject.SetActive(activate);
        top.gameObject.SetActive(activate);
        down.gameObject.SetActive(activate);
    }

    public void EnableButtons(List<string> skills)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if(skills[i] == Skills.Overcharge)
            {
                overcharge.interactable = true;
            }else if(skills[i] == Skills.RemoteMine)
            {
                mine.interactable = true;
            }else if (skills[i] == Skills.Missile)
            {
                missile.interactable = true;
            }
        }
    }


    void ConfigureButtons()
    {
        overcharge.GetComponent<ButtonPress>().onClick.AddListener(CallOvercharge);
        mine.GetComponent<ButtonPress>().onClick.AddListener(CallPlaceMines);
        missile.GetComponent<ButtonPress>().onClick.AddListener(CallMissile);

        path.onClick.AddListener(ConfirmPlayerPath);
        cancelPath.onClick.AddListener(CancelPlayerPath);
   

        //left.onClick.AddListener(() => CallPlayerMove(moveDirection.Left));
        //right.onClick.AddListener(() => CallPlayerMove(moveDirection.Right));
        //top.onClick.AddListener(() => CallPlayerMove(moveDirection.Up));
        //down.onClick.AddListener(() => CallPlayerMove(moveDirection.Down));

    }

    public void ShowPathButton(bool show)
    {
        path.gameObject.SetActive(show);
    }

    public void ShowCancelPath(bool show)
    {
        cancelPath.gameObject.SetActive(show);
    }

    void ConfirmPlayerPath()
    {
        BoardManager.Instance._playerScript.ConfirmPath();
    }

    void CancelPlayerPath()
    {
        BoardManager.Instance._playerScript.CancelPath();
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

    void CallMissile()
    {
        BoardManager.Instance._playerScript.ShowMissileMarkers();
    }

    void CallOvercharge()
    {
        BoardManager.Instance._playerScript.DoOvercharge();
    }

    void CallPlaceMines()
    {
        BoardManager.Instance._playerScript.ShowMineMarkers();
    }

    public void CallPlayerMove(moveDirection dir)
    {
        int horizontal = 0, vertical = 0;
        switch (dir)
        {
            case moveDirection.Up:
                vertical = 1;
                break;
            case moveDirection.Left:
                horizontal = -1;
                break;
            case moveDirection.Right:
                horizontal = 1;
                break;
            case moveDirection.Down:
                vertical = -1;
                break;
            default:
                break;
        }
        BoardManager.Instance._playerScript.TentativeMove(horizontal, vertical);
    }

    // Update is called once per frame
    public void DisplayMessage (string msg, float duration, textOption option = textOption.middle, bool alert = false) {
        StartCoroutine(ShowMessage(msg, duration, option,alert));
    }

    IEnumerator ShowMessage(string message, float delay, textOption option = textOption.middle, bool alert = false)
    {
        float timer = 0;
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
        if (alert)
            currentText.color = Color.red;
       // currentText.color = Color.red;
        currentText.gameObject.SetActive(true);
        while(timer < delay)
        {
            timer += Time.deltaTime;
            yield return null;

        }
        
        currentText.gameObject.SetActive(false);
    }

    //IEnumerator ShowMessage(string message, float delay, textOption option = textOption.middle)
    //{
    //    TextMeshProUGUI currentText = null;
    //    switch (option)
    //    {
    //        case textOption.middle:
    //            currentText = middleText;
    //            break;
    //        case textOption.top:
    //            currentText = topText;
    //            break;
    //        case textOption.bottom:
    //            break;
    //        default:
    //            break;
    //    }



    //    currentText.text = message;
    //    currentText.gameObject.SetActive(true);
    //    yield return new WaitForSeconds(delay);
    //    currentText.gameObject.SetActive(false);
    //}
}
