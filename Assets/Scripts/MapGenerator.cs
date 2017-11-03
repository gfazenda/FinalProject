using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab, blockPrefab, player, exitPrefab;
    public List<Transform> enemyPrefab = new List<Transform>();
    public List<Transform> trapPrefab = new List<Transform>();
    public Vector2 mapSize;
    public int maxNumberOfObstacles = 3;
    [Range(0, 1)]
    public float outlinePercent;

    public string mapObjName = "Map";

    List<Coord> allTileCoords, wallCoords;
    List<GameObject> enemyCoords;
    List<GameObject> obstacleCoords;
    Queue<Coord> shuffledTileCoords;
    public int baseObstacleNumber = 10, baseEnemyNumber = 1;
    int enemyCount = 0, obstacleCount = 0;
    public Coord playerCoord;
    public Coord exitCoord;
    Transform playerObj = null, currentObstacle = null;
    int obstaclesPlaced = 0;
    bool placeObstacle = false;


    public GameObject GetPlayer()
    {
        if (!playerObj)
            CreatePlayer();
        return playerObj.gameObject;
    }

    public void GenerateMap()
    {
        enemyCount = (GameManager.Instance.currentLevel) + baseEnemyNumber;
        obstacleCount = Random.Range(0,(GameManager.Instance.currentLevel+2)) + baseObstacleNumber;
        allTileCoords = new List<Coord>();
        wallCoords = new List<Coord>();
        obstacleCoords = new List<GameObject>();
        enemyCoords = new List<GameObject>();
        exitCoord = new Coord((int)Random.Range(0, mapSize.x), (int)(mapSize.y - 1));
        obstaclesPlaced = 0;

        InitializeMap();
        //seed = System.DateTime.UtcNow.Millisecond;
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), (int)System.DateTime.Now.Ticks));

        if (transform.Find(mapObjName))
        {
            DestroyImmediate(transform.Find(mapObjName).gameObject);
        }

        Transform mapHolder = new GameObject(mapObjName).transform;
        mapHolder.parent = this.transform;

        Transform groundHolder = new GameObject("GroundObjs").transform;
        groundHolder.parent = mapHolder.transform;

        Transform enemiesHolder = new GameObject("Enemies").transform;
        enemiesHolder.parent = mapHolder.transform;

        Transform blocksHolder = new GameObject("Blocks").transform;
        blocksHolder.parent = mapHolder.transform;

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
                Transform newTile = (Transform)Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));
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

            while (randomCoord.CompareTo(playerCoord)
                  || randomCoord.CompareTo(exitCoord)
                  || BoardManager.Distance(randomCoord, playerCoord) < 2
                  || !MapIsFullyAccessible(obstacleMap, currentObstacleCount))
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
                    currentObstacle = trapPrefab[Random.Range(0,trapPrefab.Count)];
                    obstaclesPlaced++;
                }
            }

            Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y, false);
            Transform newObstacle = Instantiate(currentObstacle, obstaclePosition, Quaternion.identity) as Transform;
            newObstacle.gameObject.GetComponent<SpecialTile>().SetPosition(randomCoord);
            if (placeObstacle)
            {
                obstacleCoords.Add(newObstacle.gameObject);
                
            }
            else
            {
                wallCoords.Add(randomCoord);
            }
            newObstacle.parent = blocksHolder;
        }
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

            Vector3 enemyPosition = CoordToPosition(randomCoord.x, randomCoord.y, false);
            Transform newEnemy = Instantiate(enemyPrefab[Random.Range(0,enemyPrefab.Count)], enemyPosition, Quaternion.identity) as Transform;
            enemyCoords.Add(newEnemy.gameObject);
            newEnemy.gameObject.GetComponent<Enemy>().position = (randomCoord);
            newEnemy.parent = enemiesHolder;
        }
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
        return new Vector3(-mapSize.x / 2 + 0.5f + x, yPos, -mapSize.y / 2 + 0.5f + y);
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
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