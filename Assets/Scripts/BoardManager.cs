using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    private static BoardManager _instance;

    public static BoardManager Instance { get { return _instance; } }
    public enum tileType { ground, player, wall, enemy, exit }
    MapGenerator _mapGenerator;
    public GameObject _player;
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
    tileType[,] gameBoard;
    //List<int> gameBoard = new List<int>();
    int mapWidth = 10, mapHeight = 10, markerCount = 0;
    public List<Transform> markers = new List<Transform>();
    bool showingMarkers = false;
    Player _playerScript;
    //0 = ground
    //1 = player
    //2 = obs
    //3 = enemy
    //4 = exit


    // Use this for initialization
    void Start()
    {
        gameBoard = new tileType[mapWidth, mapHeight];
        _mapGenerator = this.GetComponent<MapGenerator>();
        _mapGenerator.GenerateMap();
        _player = _mapGenerator.GetPlayer();
        _playerScript = _player.GetComponent<Player>();
        InitializeBoard();
    }

    void InitializeBoard()
    {

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                gameBoard[x, y] = tileType.ground;
            }
        }

        List<Coord> obstacles = _mapGenerator.GetObstacles();
        for (int i = 0; i < obstacles.Count; i++)
        {
            gameBoard[obstacles[i].x, obstacles[i].y] = tileType.wall;
        }

        List<Coord> enemies = _mapGenerator.GetEnemies();
        for (int i = 0; i < enemies.Count; i++)
        {
            gameBoard[enemies[i].x, enemies[i].y] = tileType.enemy;
        }
        gameBoard[_mapGenerator.exitCoord.x, _mapGenerator.exitCoord.y] = tileType.exit;
        gameBoard[_playerScript.GetPosition().x, _playerScript.GetPosition().y] = tileType.player;
    }

    public void SetEmptyPosition(Coord position)
    {
        gameBoard[position.x, position.y] = tileType.ground;
    }

    //int PosToIndex(Coord pos)
    //{
    //    return ((int)pos.x + (int)pos.y * height);
    //}

    public Vector3 CoordToPosition(Coord pos, bool ground = true)
    {
        int yPos = 1;
        if (!ground) yPos = 1;
        return new Vector3(-mapWidth / 2 + 0.5f + pos.x, yPos, -mapHeight / 2 + 0.5f + pos.y);
    }

    public bool IsValid(Coord pos, bool isPlayer = true)
    {
        if (pos.x >= 0 && pos.x < mapWidth
                && pos.y >= 0 && pos.y < mapHeight)
        {
            if (gameBoard[pos.x, pos.y] == tileType.exit && isPlayer)
            {
                return true;
            }
            else
                return (gameBoard[pos.x, pos.y] == tileType.ground);
        }
        return false;
    }

    public void SetPlayerPosition(Coord pos)
    {
        DisableMarkers();
        gameBoard[_playerScript.GetPosition().x, _playerScript.GetPosition().y] = tileType.ground;
        gameBoard[pos.x, pos.y] = tileType.player;
        _player.GetComponent<Player>().PerformAction(Player.Actions.Move, pos);
    }

    public void DisableMarkers()
    {
        for (int i = 0; i < markerCount; i++)
        {
            markers[i].gameObject.SetActive(false);
        }
        markerCount = 0;
        showingMarkers = false;
    }


    public List<Coord> GetNeighbours(Coord pos, int radius, bool diagonal = false)
    {
        Coord newPosition = new Coord();
        List<Coord> neighbours = new List<Coord>();
        for (int x = pos.x - radius; x <= pos.x + radius; x++)
        {
            for (int y = pos.y - radius; y <= pos.y + radius; y++)
            {
                newPosition.x = x;
                newPosition.y = y;
                if (newPosition.Equals(pos)) continue;

                if (!diagonal && newPosition.x != pos.x && newPosition.y != pos.y) continue;

                neighbours.Add(newPosition);
            }
        }

        return neighbours;
    }


    public void DisplayMarkers(Coord pos, int radius, bool diagonal = false)
    {
        showingMarkers = !showingMarkers;
        if (!showingMarkers)
        {
            DisableMarkers();
            return;
        }
       // Coord newPosition = new Coord();
        markerCount = 0;
        List<Coord> pos_neighbours = GetNeighbours(pos, radius, diagonal);
        for (int i = 0; i < pos_neighbours.Count; i++)
        {
            if (IsValid(pos_neighbours[i]))
            {
                markers[markerCount].transform.position = CoordToPosition(pos_neighbours[i]);
                markers[markerCount].gameObject.GetComponent<Marker>().SetPosition(pos_neighbours[i]);
                markers[markerCount].gameObject.SetActive(true);
                markerCount++;
            }
        }
        //for (int x = pos.x - radius; x <= pos.x + radius; x++)
        //{
        //    for (int y = pos.y - radius; y <= pos.y + radius; y++)
        //    {
        //        newPosition.x = x;
        //        newPosition.y = y;
        //        if (newPosition.Equals(pos)) continue;

        //        if (!diagonal && newPosition.x != pos.x && newPosition.y != pos.y) continue;

        //        if (IsValid(newPosition))
        //        {
        //            markers[markerCount].transform.position = CoordToPosition(newPosition);
        //            markers[markerCount].gameObject.GetComponent<Marker>().SetPosition(newPosition);
        //            markers[markerCount].gameObject.SetActive(true);
        //            markerCount++;
        //        }
        //    }
        //}
        if (markerCount == 0)
        {
            showingMarkers = false;
        }
    }

}
