using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {
    public Coord position = new Coord();


    public void SetPosition(Coord newPosition)
    {
        position = newPosition;
    }

    public Coord GetPosition()
    {
        return position;
    }

    private void OnMouseDown()
    {
        BoardManager.Instance.SetPlayerPosition(position);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
