using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour {


    public Transform seeker, target;
    //Grid grid;
    int count = 0;
    void Awake()
    {
       // grid = GetComponent<Grid>();
    }

    void Update()
    {
        //FindPath(seeker.position, target.position);
    }

    public void FindPath(Coord startNode, Coord targetNode)
    {
        List<Coord> openSet = new List<Coord>();
        HashSet<Coord> closedSet = new HashSet<Coord>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            count++;
         
            //if (count > 350)
            //{
            //    Debug.Log(closedSet.Count);
            //    RetracePath(startNode, targetNode);
            //    return;

            //}
            Coord node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Coord neighbour in BoardManager.Instance.GetNeighbours(node,1))
            {
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Coord startNode, Coord endNode)
    {
        List<Coord> path = new List<Coord>();
        Coord currentNode = endNode;
        int a = 0;
        while (currentNode != startNode)
        {
            Debug.Log("aaaa " + a);
            path.Add(currentNode);
            currentNode = currentNode.parent;
            a++;
        }
        path.Reverse();
        for (int i = 1; i < path.Count; i++)
        {
            Debug.DrawLine(BoardManager.Instance.CoordToPosition(path[i - 1]), BoardManager.Instance.CoordToPosition(path[i]));
        }
        //grid.path = path;

    }

    int GetDistance(Coord nodeA, Coord nodeB)
    {
        int dstX = Mathf.Abs(nodeA.x - nodeB.x);
        int dstY = Mathf.Abs(nodeA.y - nodeB.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}

