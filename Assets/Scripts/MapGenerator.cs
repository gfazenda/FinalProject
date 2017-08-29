using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public Transform tilePrefab, obstaclePrefab, player, exitPrefab;
    public Vector2 mapSize;

    [Range(0,1)]
    public float outlinePercent;

    public string mapObjName = "Map";

    List<Coord> allTileCoords, obstacleCoords, enemyCoords;
    Queue<Coord> shuffledTileCoords;
    public int seed = 10, obstacleCount = 10;
    public Vector2 playerPosition;
    public Coord exitCoord;
    //public void Start()
    //{
    //    GenerateMap();
    //}

    public GameObject GetPlayer()
    {
        return player.gameObject;
    }

    public void GenerateMap()
    {
        allTileCoords = new List<Coord>();
        obstacleCoords = new List<Coord>();
        Coord playerCoord = new Coord();
        exitCoord = new Coord((int)Random.Range(0,mapSize.x),(int)(mapSize.y-1));
        playerCoord.x = (int)playerPosition.x;
        playerCoord.x = (int)playerPosition.y;
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }
        //seed = System.DateTime.UtcNow.Millisecond;
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed));

        if (transform.Find(mapObjName))
        {
            DestroyImmediate(transform.Find(mapObjName).gameObject);
        }

        Transform mapHolder = new GameObject(mapObjName).transform;
        mapHolder.parent = this.transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = (Transform) Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));
                newTile.localScale = Vector3.one * (1 - outlinePercent);
                newTile.parent = mapHolder.transform;
            }
        }


        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            while (randomCoord == playerCoord || randomCoord == exitCoord)
            {
                randomCoord = GetRandomCoord();
            }
            obstacleCoords.Add(randomCoord);
            Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y, false);
            Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity) as Transform;
            newObstacle.parent = mapHolder;
        }

        Vector3 exitPosition = CoordToPosition(exitCoord.x, exitCoord.y, false);
        Transform newExit = Instantiate(exitPrefab, exitPosition, Quaternion.identity) as Transform;
        newExit.parent = mapHolder;

        player.GetComponent<Player>().SetPosition(playerCoord);
        player.transform.position = CoordToPosition(4, 0, false);

    }

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


    public List<Coord> GetObstacles()
    {
        return obstacleCoords;
    }

    //public struct Coord
    //{
    //    public int x;
    //    public int y;
    //    public Coord(int _x, int _y)
    //    {
    //        x = _x;
    //        y = _y;
    //    }
    //}
}
