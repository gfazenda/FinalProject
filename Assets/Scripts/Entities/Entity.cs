using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthBar))]
public class Entity : MonoBehaviour {
    public float HP = 10;// { get; set; }
    float maxHP;
    HealthBar hpScript;
    // Use this for initialization
    void Start () {
        maxHP = HP;
        hpScript = this.GetComponentInChildren<HealthBar>();
    }

    public void UpdateHealthBar(float  damage)
    {
        HP -= damage;
        float healthAmount = (float)HP / (float)maxHP;
        hpScript.UpdateBar(healthAmount);
        if (HP <= 0)
            this.gameObject.SetActive(false);
    }

    public bool Dead()
    {
        return HP <= 0;
    }
}
