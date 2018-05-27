using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfo : MonoBehaviour, IPointerDownHandler
{
    public Text name, description, damage, manacost;
    public List<Skill> _skillList = new List<Skill>();
    Dictionary<string, Skill> _skills = new Dictionary<string, Skill>();
    Skill currentSkill = null;

    private void Start()
    {
        InitializeSkills();
    }


    void InitializeSkills()
    {
        for (int i = 0; i < _skillList.Count; i++)
        {
           // Debug.Log(_skillList[i].name);
            _skills.Add(_skillList[i].name, _skillList[i]);
        }
        this.gameObject.SetActive(false);
    }

    public void ShowSkillDetails(string skillname)
    {
        Debug.Log(skillname);
        this.gameObject.SetActive(true);
        _skills.TryGetValue(skillname, out currentSkill);
        name.text = currentSkill.name;
        description.text = currentSkill.description;
        Debug.Log("desc "  + currentSkill.description);
        damage.text = "Damage: " + currentSkill.damage;
        manacost.text = "| \f Manacost: " + currentSkill.manacost;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.gameObject.SetActive(false);
    }

    public void DisablePanel()
    {
        this.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        EventManager.TriggerEvent(Events.DisableMoveButtons);
    }

    private void OnDisable()
    {
        EventManager.TriggerEvent(Events.EnableMoveButtons);
    }

}
