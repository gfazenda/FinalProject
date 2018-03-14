using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : MonoBehaviour {

    public enum BehaviorType {Aggresive, Passive, Neutral, ObjetiveDriven}
    BehaviorType _behavior;

    public enum WorstEnemy { Rock, Venom, Lava};
    WorstEnemy _enemy;

    public enum MostUsedSkill { Basic, Mine, Overcharge, Missile};
    MostUsedSkill _skill;

    public float killPercentage;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
