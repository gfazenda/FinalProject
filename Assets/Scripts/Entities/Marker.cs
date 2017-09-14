using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {
    public Coord position = new Coord();
    public enum MarkerType {movement, attack, information};
    MarkerType _type;
    Material _material;
    Color currentColor;
    public void SetPosition(Coord newPosition)
    {
        position = newPosition;
    }

    public void EnableMarker(MarkerType type, Coord _position)
    {
        if(_material == null)
        {
            _material = this.GetComponent<MeshRenderer>().material;
        }
        SetPosition(_position);
        
        _type = type;
        switch (_type)
        {
            case MarkerType.movement:
                currentColor = Color.green;
                break;
            case MarkerType.attack:
                currentColor = Color.red;
                break;
            case MarkerType.information:
                currentColor = Color.red;
                break;
        }
        currentColor.a = 0.6f;
        _material.color = currentColor;
    }

    public Coord GetPosition()
    {
        return position;
    }

    private void OnMouseDown()
    {
        switch (_type)
        {
            case MarkerType.movement:
                BoardManager.Instance.SetPlayerAction(Player.Actions.Move, position);
                break;
            case MarkerType.attack:
                BoardManager.Instance.SetPlayerAction(Player.Actions.BasicAtk, position);
                break;
            case MarkerType.information:
                break;
        }
        
    }

    // Use this for initialization
    void Awake () {
        _material = this.GetComponent<MeshRenderer>().material;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
