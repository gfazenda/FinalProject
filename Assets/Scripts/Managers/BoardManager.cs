using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private static BoardManager _instance;

    public static BoardManager Instance { get { return _instance; } }
    public enum tileType { outOfLimits, ground, wall, obstacle , enemy, player, exit, rock, venon, lava, drain, block  };

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
        EventManager.StartListening(Events.LevelLoaded, DoInitialize);
    }
    tileType[,] gameBoard;
    public Node[,] gameGrid;
    //List<int> gameBoard = new List<int>();
    int mapWidth, mapHeight, markerCount = 0;
    List<GameObject> mapObstacles = new List<GameObject>();
    bool showingMarkers = false, boardCreated = false;

    public Player _playerScript;
    public List<GameObject> markers = new List<GameObject>();
    public List<GameObject> listOfEnemies = new List<GameObject>();
    public bool generateAtRuntime = false;

    Coord exitCoord;
    GameObject currentMarker, obsAttacked = null, currentEffect;
    //0 = ground
    //1 = player
    //2 = obs
    //3 = enemy
    //4 = exit

    #region Initialization

    public void DoInitialize()
    {
        if (boardCreated) return;
        Debug.Log("initializing");
        
        _mapGenerator = this.GetComponent<MapGenerator>();

        if (GameManager.Instance.currentLevel < 10)
        {
            LevelInformation currentBoard = GameManager.Instance.LoadLevelFile();
            mapHeight = (int)currentBoard.mapSize.y;
            mapWidth = (int)currentBoard.mapSize.x;
            gameBoard = new tileType[mapWidth, mapHeight];
            gameBoard = _mapGenerator.CreateBoardFromFile(currentBoard);
        }
        else
        {

            mapWidth = (int)_mapGenerator.mapSize.x;
            mapHeight = (int)_mapGenerator.mapSize.y;
            _mapGenerator.GenerateMap();
        }

       
        gameGrid = new Node[mapWidth, mapHeight];

        _player = _mapGenerator.GetPlayer();
        _playerScript = _player.GetComponent<Player>();

        //  if(generateAtRuntime)


        exitCoord = _mapGenerator.exitCoord;
        boardCreated = true;
        InitializeGrid();
        InitializeBoard();
    }

    void InitializeBoard()
    {

        //for (int x = 0; x < mapWidth; x++)
        //{
        //    for (int y = 0; y < mapHeight; y++)
        //    {
        //        gameBoard[x, y] = tileType.ground;
        //    }
        //}

        //List<Coord> walls = _mapGenerator.GetWalls();
        //for (int i = 0; i < walls.Count; i++)
        //{
        //    gameBoard[walls[i].x, walls[i].y] = tileType.wall;
        //}

        //mapObstacles = _mapGenerator.GetObstacles();
        //for (int i = 0; i < mapObstacles.Count; i++)
        //{
        //    Coord position = mapObstacles[i].GetComponent<SpecialTile>().GetPosition();
        //    gameBoard[position.x, position.y] = tileType.obstacle;
        //}

        ///* List<Coord> */
        listOfEnemies = _mapGenerator.GetEnemies();
        //for (int i = 0; i < listOfEnemies.Count; i++)
        //{
        //    Coord position = listOfEnemies[i].GetComponent<Enemy>().GetPosition();
        //    gameBoard[position.x, position.y] = tileType.enemy;
        //}
        EventManager.TriggerEvent(Events.EnemiesCreated);

        //gameBoard[_mapGenerator.exitCoord.x, _mapGenerator.exitCoord.y] = tileType.exit;
        //gameBoard[_playerScript.GetPosition().x, _playerScript.GetPosition().y] = tileType.player;
    }

    void InitializeGrid()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                gameGrid[x, y] = new Node(x, y);
            }
        }
    }
    #endregion

    public int BoardSize()
    {
        return mapHeight * mapWidth;
    }



    #region AStar methods
    public Node GetNode(Coord pos)
    {
        return gameGrid[pos.x, pos.y];
    }

    public List<Node> GetNodeNeighbours(Node node, int radius, bool diagonal = false)
    {
        List<Node> neighbours = new List<Node>();
        Coord newPosition = new Coord();
        for (int x = node.posX - radius; x <= node.posX + radius; x++)
        {
            for (int y = node.posY - radius; y <= node.posY + radius; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = x;
                int checkY = y;


                if (!diagonal && checkX != node.posX && checkY != node.posY) continue;

                newPosition.x = checkX;
                newPosition.y = checkY;

                if (GetPositionType(newPosition) == tileType.ground || GetPositionType(newPosition) == tileType.player)
                {
                    neighbours.Add(gameGrid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    #endregion

    #region Positioning
    public void UpdatePosition(Coord oldPos, Coord newPos, tileType type)
    {
        gameBoard[oldPos.x, oldPos.y] = tileType.ground;
        gameBoard[newPos.x, newPos.y] = type;
    }

    public void SetEmptyPosition(Coord pos)
    {
        gameBoard[pos.x, pos.y] = tileType.ground;
    }

    public void SetInvalidPosition(Coord pos)
    {
        gameBoard[pos.x, pos.y] = tileType.outOfLimits;
    }

    public Vector3 CoordToPosition(Coord pos, bool marker = true)
    {
        float yPos = 0.5f;
        if (!marker) yPos = 1f;
        float xOffset = (mapWidth % 2 == 0) ? 0.5f : -1f;
        float yOffset = (mapHeight % 2 == 0) ? 0.5f : -1f;      
        return new Vector3((-mapWidth / 2 + xOffset) + pos.x, yPos, (-mapHeight / 2 + yOffset) + pos.y);
    }


    public tileType GetPositionType(Coord pos)
    {
        if (pos.x >= 0 && pos.x < mapWidth
                && pos.y >= 0 && pos.y < mapHeight)
        {
            if (exitCoord.CompareTo(pos))
            {
                return tileType.exit;
            }
            return gameBoard[pos.x, pos.y];
        }
        else
            return tileType.outOfLimits;
    }

    public GameObject InstantiateEffect(string tag, Coord pos)
    {
        currentEffect = ObjectPooler.SharedInstance.GetPooledObject(tag);
        currentEffect.transform.position = CoordToPosition(pos);
        currentEffect.SetActive(true);
        return currentEffect;
    }

    #endregion


    GameObject ObstacleAt(Coord _pos)
    {
        Coord position;
        for (int i = 0; i < mapObstacles.Count; i++)
        {
            position = mapObstacles[i].GetComponent<SpecialTile>().GetPosition();
            if (!mapObstacles[i].activeInHierarchy)
            {
                mapObstacles.RemoveAt(i);
                SetEmptyPosition(position);
                continue;
            }
            Debug.Log(_pos.DebugInfo() + " " + position.DebugInfo());
            if (_pos.CompareTo(position))
            {
                Debug.Log("found it");
                return mapObstacles[i];
            }
        }
        return null;
    }

    public void TileAttacked(Coord position, float damage)
    {
        switch (GetPositionType(position))
        {
            case tileType.enemy:
                EnemyCoordinator.Instance.EnemyDamaged(damage, position);
                break;
            case tileType.obstacle:
                obsAttacked = ObstacleAt(position);
                if (obsAttacked != null && obsAttacked.GetComponent<Entity>())
                {
                    obsAttacked.GetComponent<Entity>().UpdateHealthAmount(damage);
                    obsAttacked.GetComponent<Entity>().DoHPBardUpdate();
                }
                break;
        }
    }

    public static float Distance(Coord a, Coord b)
    {
        Vector2 pointA = new Vector2((float)a.x, (float)a.y);
        Vector2 pointB = new Vector2((float)b.x, (float)b.y);
        return Vector2.Distance(pointA, pointB);
    }

    public static bool IsInDiagonal(Coord a, Coord b)
    {
        return (a.x != b.x && a.y != b.y);
    }

    public List<KeyValuePair<tileType, Coord>> GetNeighbours(Coord pos, int radius, tileType[] types, bool diagonal = false)
    {
        List<KeyValuePair<tileType, Coord>> neighbours = new List<KeyValuePair<tileType, Coord>>();
        Coord newPosition = new Coord();
        tileType currentType;
        for (int x = pos.x - radius; x <= pos.x + radius; x++)
        {
            for (int y = pos.y - radius; y <= pos.y + radius; y++)
            {
                newPosition = new Coord();
                if (x == 0 && y == 0)
                    continue;


                if (!diagonal && newPosition.x != pos.x && newPosition.y != pos.y) continue;

                newPosition.x = x;
                newPosition.y = y;
                currentType = GetPositionType(newPosition);
                for (int i = 0; i < types.Length; i++)
                {
                    if (types[i] == currentType)
                        neighbours.Add(new KeyValuePair<tileType, Coord>(currentType, newPosition));
                }
                //if (types.Contains(currentType))
                //{

                //}
            }
        }
        //for (int i = 0; i < neighbours.Count; i++)
        //{
        //    //List<Coord> a;
        //    //a.AddRange(neighbours.Values);
        //    neighbours.
        //}

        return neighbours;
    }

    public void SetPlayerAction(Player.Actions action, Coord pos)
    {
        _playerScript.PerformAction(action, pos);
    }

    #region Markers

    public void DisableMarkers()
    {
        for (int i = 0; i < markerCount; i++)
        {
            markers[i].SetActive(false);
        }
        markerCount = 0;
        showingMarkers = false;
    }

    public void DisplayMarkers(Coord pos, int radius, bool diagonal = false, bool placeMines = false, bool missile = false)
    {
        showingMarkers = !showingMarkers;
        if (!showingMarkers)
        {
            DisableMarkers();
            return;
        }

        markerCount = 0;
        markers.Clear();
        Coord newPosition = new Coord();
        bool enable = false;
        Marker.MarkerType type = Marker.MarkerType.movement;

        for (int x = pos.x - radius; x <= pos.x + radius; x++)
        {
            for (int y = pos.y - radius; y <= pos.y + radius; y++)
            {
                enable = false;
                newPosition = new Coord();
                newPosition.x = x;
                newPosition.y = y;
                // Debug.Log("coord2 " + newPosition.x + " " + newPosition.y);
                if (newPosition.CompareTo(pos)) continue;

                if (!diagonal && newPosition.x != pos.x && newPosition.y != pos.y) continue;

                type = Marker.MarkerType.movement;
                switch (GetPositionType(newPosition))
                {
                    case tileType.enemy:
                        enable = true;
                        type = Marker.MarkerType.attack;
                        break;
                    case tileType.obstacle:
                        if (!placeMines)
                        {
                            enable = true;
                            type = Marker.MarkerType.attack;

                        }
                        break;
                    case tileType.ground:
                        enable = true;
                        break;
                    case tileType.exit:
                        if (!placeMines)
                        {
                            enable = true;
                        }
                        break;
                }


                if (placeMines)
                    type = Marker.MarkerType.placemine;
                else if(missile)
                    type = Marker.MarkerType.missile;
                //if (IsValid(newPosition))
                //{
                //    enable = true;
                //    type = Marker.MarkerType.movement;
                //}else if(gameBoard[newPosition.x, newPosition.y] == tileType.enemy)
                //{
                //    enable = true;
                //    type = Marker.MarkerType.attack;
                //}

                if (enable)
                {
                    currentMarker = null;
                    currentMarker = ObjectPooler.SharedInstance.GetPooledObject(Tags.Marker);
                    if (currentMarker != null)
                    {
                        currentMarker.transform.position = CoordToPosition(newPosition);
                        currentMarker.GetComponent<Marker>().EnableMarker(type, newPosition);
                        currentMarker.SetActive(true);
                        markers.Add(currentMarker);
                        //markers[markerCount].transform.position = CoordToPosition(newPosition);
                        //markers[markerCount].gameObject.GetComponent<Marker>().EnableMarker(type, newPosition);
                        //markers[markerCount].gameObject.SetActive(true);
                        markerCount++;
                    }

                }
            }
        }
        if (markerCount == 0)
        {
            showingMarkers = false;
        }
    }
    #endregion
}
