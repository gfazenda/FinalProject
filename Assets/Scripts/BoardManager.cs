using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    private static BoardManager _instance;

    public static BoardManager Instance { get { return _instance; } }

    MapGenerator _mapGenerator;
    GameObject _player;
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
    }

    List<int> gameBoard = new List<int>();
    int width = 10, height = 10;
    public List<Transform> markers = new List<Transform>();
    bool showingMarkers = false;
    //0 = ground
    //1 = player
    //2 = obs
    //3 = enemy
    //4 = exit


    // Use this for initialization
    void Start()
    {
        _mapGenerator = this.GetComponent<MapGenerator>();
        _mapGenerator.GenerateMap();
        _player = _mapGenerator.GetPlayer();
        InitializeBoard();     
    }

    void InitializeBoard()
    {
        for (int i = 0; i < (width * height); i++)
        {
            gameBoard.Add(0);
        }
        List<Coord> obstacles = _mapGenerator.GetObstacles();
        for (int i = 0; i < obstacles.Count; i++)
        {
            gameBoard[PosToIndex(obstacles[i])] = 2;
        }
        gameBoard[PosToIndex(_mapGenerator.exitCoord)] = 4;
        gameBoard[PosToIndex(_player.GetComponent<Player>().GetPosition())] = 1;
    }

    public void SetEmptyPosition(Coord position)
    {
        gameBoard[PosToIndex(position)] = 0;
    }

    int PosToIndex(Coord pos)
    {
        return ((int)pos.x + (int)pos.y * height);
    }

    public Vector3 CoordToPosition(Coord pos, bool ground = true)
    {
        int yPos = 1;
        if (!ground) yPos = 1;
        return new Vector3(-width / 2 + 0.5f + pos.x, yPos, -height / 2 + 0.5f + pos.y);
    }

   public bool IsValid(Coord pos, bool isPlayer = true)
    {
        if (pos.x >= 0 && pos.x < width
                && pos.y >= 0 && pos.y < height)
        {
            if (gameBoard[PosToIndex(pos)] == 4 && isPlayer)
            {
                return true;
            }
            else
                return (gameBoard[PosToIndex(pos)] == 0);
        }
        return false;
    }

    public void SetPlayerPosition(Coord pos)
    {
        DisableMarkers();
        showingMarkers = !showingMarkers;
        gameBoard[PosToIndex(_player.GetComponent<Player>().GetPosition())] = 0;
        gameBoard[PosToIndex(pos)] = 1;
        _player.GetComponent<Player>().SetPosition(pos);
    }
    
    void DisableMarkers()
    {
        for (int i = 0; i < markers.Count; i++)
        {
            markers[i].gameObject.SetActive(false);
        }
    }

    public void ShowMarkers(Coord pos)
    {
        showingMarkers = !showingMarkers;
        if (!showingMarkers)
        {
            DisableMarkers();
            return;
        }
        Coord newPosition;        
        for (int i = 0; i < markers.Count; i++)
        {
            newPosition = pos;
            switch (i)
            {
                case 0:
                    newPosition.y += 1;
                    break;
                case 1:
                    newPosition.x += 1;
                    break;
                case 2:
                    newPosition.y -= 1;
                    break;
                case 3:
                    newPosition.x -= 1;
                    break;

            }
            if (IsValid(newPosition))
            {
                markers[i].transform.position = CoordToPosition(newPosition);
                markers[i].gameObject.GetComponent<Marker>().SetPosition(newPosition);
                markers[i].gameObject.SetActive(true);
            }
        }
    }
}
