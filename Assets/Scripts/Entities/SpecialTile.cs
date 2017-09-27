using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTile : MonoBehaviour {
    protected Coord position;

    public void SetPosition(Coord _position)
    {
        position = _position;
    }

    public Coord GetPosition()
    {
        return position;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
