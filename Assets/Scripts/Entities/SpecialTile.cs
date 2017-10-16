using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTile : MonoBehaviour {
    protected Coord position;
    public float yHeight = 0.5f;
    Vector3 worldPosition;
    public void SetPosition(Coord _position)
    {
        position = _position;
        worldPosition = BoardManager.Instance.CoordToPosition(position);
        worldPosition.y = yHeight;
        this.transform.position = worldPosition;
    }

    public Coord GetPosition()
    {
        return position;
    }

}
