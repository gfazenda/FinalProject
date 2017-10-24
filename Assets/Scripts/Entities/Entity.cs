using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthBar))]
public class Entity : MonoBehaviour {
    public float HP = 10, maxHP;

    [HideInInspector]
    public float healthAmount;

    HealthBar hpScript;
    // Use this for initialization
    void Start () {
        maxHP = HP;
        hpScript = this.GetComponentInChildren<HealthBar>();
    }

    public void UpdateHealthAmount(float  damage)
    {
        HP -= damage;
        healthAmount = (float)HP / (float)maxHP;
        if (HP <= 0)
            DoDeath();     
    }

    virtual public void DoHPBardUpdate()
    {
        hpScript.UpdateBar(healthAmount);
    }

    virtual protected void DoDeath()
    {
        this.gameObject.SetActive(false);
    }

    public bool Dead()
    {
        return HP <= 0;
    }
}
