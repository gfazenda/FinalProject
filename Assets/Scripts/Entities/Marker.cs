using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {
    public Coord position = new Coord();
    public float transparency = 0.5f;
    public enum MarkerType {movement, attack, information, placemine, missile};
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
            case MarkerType.missile:
                currentColor = Color.red;
                break;
            case MarkerType.placemine:
                currentColor = Color.cyan;
                break;
            case MarkerType.information:
                currentColor = Color.yellow;
                break;
        }
        currentColor.a = transparency;
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
            case MarkerType.placemine:
                BoardManager.Instance.SetPlayerAction(Player.Actions.Mine, position);
                break;
            case MarkerType.attack:
                BoardManager.Instance.SetPlayerAction(Player.Actions.BasicAtk, position);
                break;
            case MarkerType.missile:
                BoardManager.Instance.SetPlayerAction(Player.Actions.Missile, position);
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
