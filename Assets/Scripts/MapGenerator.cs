using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform groundPrefab, blockPrefab, player, exitPrefab, venoalien, rockmonster, lavathing, drain, blocker, laserPrefab;
    List<Transform> enemyPrefab = new List<Transform>();
    List<Transform> trapPrefab = new List<Transform>();
    public Vector2 mapSize;
    public int maxNumberOfObstacles = 3;
    int laserTowers = 0;
    [Range(0, 1)]
    public float outlinePercent;

    public string mapObjName = "Map";

    List<Coord> allTileCoords, wallCoords, towerCoords;
    List<GameObject> enemyCoords;
    List<GameObject> obstacleCoords;
    Queue<Coord> shuffledTileCoords;
    public int baseObstacleNumber = 10, baseEnemyNumber = 1;
    int enemyCount = 0, obstacleCount = 0;
    public Coord playerCoord;
    public Coord exitCoord;
    Transform playerObj = null, currentObstacle = null, mapHolder, groundHolder, enemiesHolder, blocksHolder;
    int obstaclesPlaced = 0;
    bool placeObstacle = false;


    public GameObject GetPlayer()
    {
        if (!playerObj)
            CreatePlayer();
        return playerObj.gameObject;
    }

    void InitializeVariables()
    {
        allTileCoords = new List<Coord>();
        wallCoords = new List<Coord>();
        towerCoords = new List<Coord>();
        obstacleCoords = new List<GameObject>();
        enemyCoords = new List<GameObject>();
        mapHolder = new GameObject(mapObjName).transform;
        mapHolder.parent = this.transform;

        groundHolder = new GameObject("GroundObjs").transform;
        groundHolder.parent = mapHolder.transform;

        enemiesHolder = new GameObject("Enemies").transform;
        enemiesHolder.parent = mapHolder.transform;

        blocksHolder = new GameObject("Blocks").transform;
        blocksHolder.parent = mapHolder.transform;

        if (transform.Find(mapObjName))
        {
            DestroyImmediate(transform.Find(mapObjName).gameObject);
        }

        enemyPrefab.Add(venoalien);
        enemyPrefab.Add(rockmonster);
        enemyPrefab.Add(lavathing);

        trapPrefab.Add(drain);
        trapPrefab.Add(blocker);

    }

    public void GenerateMap()
    {
        InitializeVariables();
        enemyCount = (GameManager.Instance.currentLevel)/2 + baseEnemyNumber;
        obstacleCount = Random.Range(0,(GameManager.Instance.currentLevel+2)) + baseObstacleNumber;
       
        exitCoord = new Coord((int)Random.Range(0, mapSize.x), (int)(mapSize.y - 1));
        obstaclesPlaced = 0;

        InitializeMap();
        //seed = System.DateTime.UtcNow.Millisecond;
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), (int)System.DateTime.Now.Ticks));

        PlaceGround(groundHolder);

        GenerateObstacles(blocksHolder);

        GenerateEnemies(enemiesHolder);

        Vector3 exitPosition = CoordToPosition(exitCoord.x, exitCoord.y, false);
        Transform newExit = Instantiate(exitPrefab, exitPosition, Quaternion.identity) as Transform;
        newExit.parent = mapHolder;



    }

    private void InitializeMap()
    {
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }
    }

    private void PlaceGround(Transform groundHolder)
    {
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = (Transform)Instantiate(groundPrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));
                newTile.localScale = Vector3.one * (1 - outlinePercent);
                newTile.GetComponent<SpecialTile>().SetPosition(new Coord(x, y));
                newTile.parent = groundHolder.transform;
            }
        }
    }

    private void GenerateObstacles(Transform blocksHolder)
    {
        bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];
        int currentObstacleCount = 0;

        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;

            while (ObstacleIsInvalid(obstacleMap, currentObstacleCount, randomCoord))
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                randomCoord = GetRandomCoord();
                obstacleMap[randomCoord.x, randomCoord.y] = true;
                //currentObstacleCount--;
            }
            currentObstacle = blockPrefab;

            if (obstaclesPlaced < maxNumberOfObstacles)
            {
                placeObstacle = Random.Range(0.0f, 1.0f) < 0.1f ? true : false;
                if (placeObstacle)
                {
                    currentObstacle = trapPrefab[Random.Range(0, trapPrefab.Count)];
                    obstaclesPlaced++;
                }
            }

            Transform obstacle = InstantiatePrefab(randomCoord, currentObstacle, blocksHolder);
            obstacle.gameObject.GetComponent<SpecialTile>().SetPosition(randomCoord);
            if (placeObstacle)
            {
                obstacleCoords.Add(obstacle.gameObject);
            }
            else
            {
                wallCoords.Add(randomCoord);
            }
        }
    }

    private bool ObstacleIsInvalid(bool[,] obstacleMap, int currentObstacleCount, Coord randomCoord)
    {
        return randomCoord.CompareTo(playerCoord)
                          || randomCoord.CompareTo(exitCoord)
                          || BoardManager.Distance(randomCoord, playerCoord) < 2
                          || !MapIsFullyAccessible(obstacleMap, currentObstacleCount);
    }

    Transform InstantiatePrefab(Coord position, Transform prefab, Transform objHolder, bool ground = false)
    {
        Vector3 prefabPosition = CoordToPosition(position.x, position.y, ground);
        Transform newObstacle = Instantiate(prefab, prefabPosition, Quaternion.identity) as Transform;
        newObstacle.parent = objHolder;
        return newObstacle;
    }

    private void GenerateEnemies(Transform enemiesHolder)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            while (randomCoord.CompareTo(playerCoord) || randomCoord.CompareTo(exitCoord) || BoardManager.Distance(randomCoord, playerCoord) < 3)
            {
                randomCoord = GetRandomCoord();
            }

            InstantiateEnemy(randomCoord);
            //Vector3 enemyPosition = CoordToPosition(randomCoord.x, randomCoord.y, false);
            //Transform newEnemy = Instantiate(enemyPrefab[Random.Range(0,enemyPrefab.Count)], enemyPosition, Quaternion.identity) as Transform;
            //enemyCoords.Add(newEnemy.gameObject);
            //newEnemy.gameObject.GetComponent<Enemy>().position = (randomCoord);
            //newEnemy.parent = enemiesHolder;
        }
    }

    Transform InstantiateEnemy(Coord position, Transform prefab = null)
    {
        Vector3 enemyPosition = CoordToPosition(position.x, position.y, false);
        Transform newEnemy = null;
        if(prefab != null){
            newEnemy = Instantiate(prefab, enemyPosition, Quaternion.identity) as Transform;
        }else{
            newEnemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Count)], enemyPosition, Quaternion.identity) as Transform;
        }
        newEnemy.gameObject.GetComponent<Enemy>().position = (position);
        newEnemy.parent = enemiesHolder;
        enemyCoords.Add(newEnemy.gameObject);
        return newEnemy;
    }

    void CreatePlayer()
    {
        //  playerObj = GameObject.FindWithTag(Tags.Player).transform;
        playerObj = Instantiate(player, CoordToPosition(playerCoord.x, playerCoord.y, false), Quaternion.identity) as Transform;
        playerObj.GetComponent<Player>().position = (playerCoord);
        //playerObj.parent = transform.Find(mapObjName);
    }


    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(playerCoord);
        mapFlags[playerCoord.x, playerCoord.y] = true;
        queue.Enqueue(exitCoord);
        mapFlags[exitCoord.x, exitCoord.y] = true;

        int accessibleTileCount = 2;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (x == 0 || y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    //Vector3 CoordToPosition(int x, int y)
    //{
    //    return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
    //}


    Vector3 CoordToPosition(int x, int y, bool ground = true)
    {
        int yPos = 0;
        if (!ground) yPos = 1;
        float xOffset = (mapSize.x % 2 == 0) ? 0.5f : -0.5f;
        float yOffset = (mapSize.y % 2 == 0) ? 0.5f : -0.5f;
        return new Vector3(-mapSize.x / 2 + xOffset + x, yPos, -mapSize.y / 2 + yOffset + y);
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public void SetMapSize(Vector2 size)
    {
        mapSize = new Vector2(size.x, size.y);
    }

    public BoardManager.tileType[,] CreateBoardFromFile(LevelInformation level)
    {
        InitializeVariables();
        InitializeMap();
       // PlaceGround(groundHolder);
        Coord currentCoord;
        int currentObj;
        mapSize = level.mapSize;
        BoardManager.tileType[,] map = new BoardManager.tileType[(int)mapSize.x,(int)mapSize.y];
        Transform currObs = null;
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                 currentCoord = new Coord(x, y);
                 currentObj = level.objects[x, ((int)(mapSize.y - 1) - y)];
                if(currentObj == -1)
                {
                    currentObj = 12;
                }
                if ((BoardManager.tileType)currentObj == BoardManager.tileType.outOfLimits)
                {
                    continue;
                }
                else
                {
                    InstantiatePrefab(currentCoord, groundPrefab, groundHolder,true);
                }
                map[x, y] = (BoardManager.tileType)currentObj;
                switch ((BoardManager.tileType)currentObj)
                {
                    case BoardManager.tileType.enemy:
                        InstantiateEnemy(currentCoord);
                        break;
                    case BoardManager.tileType.player:
                        playerCoord = new Coord(currentCoord);
                        CreatePlayer();
                        break;
                    case BoardManager.tileType.wall:
                        InstantiatePrefab(currentCoord, blockPrefab, blocksHolder);
                        wallCoords.Add(currentCoord);
                        break;
                    case BoardManager.tileType.obstacle:
                        currObs = InstantiatePrefab(currentCoord, trapPrefab[Random.Range(0, trapPrefab.Count - 1)], blocksHolder);
                        obstacleCoords.Add(currObs.gameObject);
                        currObs.GetComponent<SpecialTile>().SetPosition(currentCoord);
                        break;
                    case BoardManager.tileType.exit:
                        exitCoord = new Coord(currentCoord);
                        InstantiatePrefab(exitCoord, exitPrefab, mapHolder);
                        break;
                    case BoardManager.tileType.venon:
                        InstantiateEnemy(currentCoord, venoalien);
                        break;
                    case BoardManager.tileType.rock:
                        InstantiateEnemy(currentCoord, rockmonster);
                        break;
                    case BoardManager.tileType.lava:
                        InstantiateEnemy(currentCoord, lavathing);
                        break;
                    case BoardManager.tileType.block:
                        currObs = InstantiatePrefab(currentCoord, blocker, blocksHolder);
                        obstacleCoords.Add(currObs.gameObject);
                        currObs.GetComponent<SpecialTile>().SetPosition(currentCoord);
                        break;
                    case BoardManager.tileType.laser:
                        laserTowers++;
                        towerCoords.Add(currentCoord);
                        if (laserTowers == 2)
                        {
                            currObs = InstantiatePrefab(currentCoord, laserPrefab, blocksHolder);
                            currObs.GetComponent<LaserTower>().CreateTrap(towerCoords[0], towerCoords[1]);
                            towerCoords.Clear();
                            laserTowers = 0;
                        }
                        break;
                    case BoardManager.tileType.drain:
                        currObs = InstantiatePrefab(currentCoord, drain, blocksHolder);
                        obstacleCoords.Add(currObs.gameObject);
                        currObs.GetComponent<SpecialTile>().SetPosition(currentCoord);
                        break;
                }
            }
        }
        return map;

    }


    public List<Coord> GetWalls()
    {
        return wallCoords;
    }

    public List<GameObject> GetObstacles()
    {
        return obstacleCoords;
    }

    public List<GameObject> GetEnemies()
    {
        return enemyCoords;
    }

}