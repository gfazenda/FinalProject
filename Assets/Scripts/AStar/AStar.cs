using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour {

    public bool drawPath = false;

    [HideInInspector]
    public Transform seeker, target;

    List<Coord> path = new List<Coord>();
    //Grid grid;
    int count = 0;
    void Awake()
    {
       // grid = GetComponent<Grid>();
    }

    void Update()
    {
       if(drawPath && path.Count > 0)
        {
            for (int i = 1; i < path.Count; i++)
            {
                Debug.DrawLine(BoardManager.Instance.CoordToPosition(path[i - 1]), BoardManager.Instance.CoordToPosition(path[i]));
            }
            
        }
    }

    public List<Coord> FindPath(Coord startPos, Coord targetPos)
    {
        path = new List<Coord>();
        Node startNode = BoardManager.Instance.GetNode(startPos);
        Node targetNode = BoardManager.Instance.GetNode(targetPos);

        Heap<Node> openSet = new Heap<Node>(BoardManager.Instance.BoardSize());
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);


        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);
            Debug.Log(currentNode.posX + " " + currentNode.posY);
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return path;
            }
            foreach (Node neighbour in BoardManager.Instance.GetNodeNeighbours(currentNode, 1))
            {

                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return path;
    }
   
    void RetracePath(Node startNode, Node endNode)
    {
        path.Clear();
        Node currentNode = endNode;
        int a = 0;
        while (currentNode != startNode)
        {
            path.Add(new Coord(currentNode.posX,currentNode.posY));
            currentNode = currentNode.parent;
            a++;
        }
        path.Reverse();
        //grid.path = path;

    }

   

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.posX - nodeB.posX);
        int dstY = Mathf.Abs(nodeA.posY - nodeB.posY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}

