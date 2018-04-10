using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMax : MonoBehaviour
{
    private static MiniMax _instance;

    public static MiniMax Instance { get { return _instance; } }
    //public struct Node
    //{
    //    public List<Node> children;
    //    public struct 
    //    public float result;
    //}
    List<MMNode> actions = new List<MMNode>();

    int atkValue = 100, moveCloser = 10, takeDMG = -100, closerToTarget = 1, move = 2;
    int mapHeight = 0, mapWidth = 0, treeLevel = 3;
    Coord playerPosition, exitPosition;
    float distToPlayer, distToExit, atkRange;

    BoardManager.tileType[,] currentBoard;
    // Use this for initialization
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this);

    }

    public void CheckNextAction(GameObject enemy)
    {
        MMNode node = new MMNode();
        Vector2 mapInfo = BoardManager.Instance.BoardHeightWidth();
        mapWidth = (int)mapInfo.x;
        mapHeight = (int)mapInfo.y;

        CalculateEnemyMoves(node, enemy);

        MMNode result = DoMinimax(node, 3, true);
        Debug.Log("performing:");
        Debug.Log(result.value + " "+  result.action + " " + result.targetPosition.DebugInfo());

        if (result.action == MMNode.ActionType.Move)
            enemy.GetComponent<Enemy>().PerformMove(result.targetPosition);
        else
            enemy.GetComponent<Enemy>().PerformAttack();
    }

    void CalculateEnemyMoves(MMNode node, GameObject enemy)
    {
        currentBoard = BoardManager.Instance.GetGameBoard();
        node.resultingBoard = (BoardManager.tileType[,])currentBoard.Clone();
        Enemy e = enemy.GetComponent<Enemy>();
        Coord c = e.GetPosition();
        
        atkRange = e.atkRange;

        playerPosition = BoardManager.Instance._playerScript.position;
        distToPlayer = BoardManager.Distance(c, playerPosition);
        distToExit = BoardManager.Distance(c, BoardManager.Instance.GetExitCoord());
        exitPosition = BoardManager.Instance.GetExitCoord();

        if (distToPlayer <= atkRange)
        {
            // MMNode child = new MMNode(MMNode.ActionType.Atk, atkValue, c);
            // node.children.Add(child);
            AddNode(node, MMNode.ActionType.Atk, atkValue, BoardManager.Instance._playerScript.position);
           // return;
        }

        CreateEnemyMoveNodes(node, c, 3);

        //for (int i = 0; i < length; i++)
        //{
        //    CreateEnemyMoveNodes(node, c, 1);
        //    node = DoMinimax(node, 1, true);
            
        //}

    }

    private void CreateEnemyMoveNodes(MMNode node, Coord position, int level)
    {
        if (level <= 0)
            return;

        Coord newPosition = new Coord();
        BoardManager.tileType[,] newBoard = node.resultingBoard;
        BoardManager.tileType nextTile;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 || j == 0)
                {
                   
                    newPosition.x = position.x + i;
                    newPosition.y = position.y + j;
                    nextTile = GetType(node.resultingBoard, newPosition);
                    if(nextTile == BoardManager.tileType.ground)
                    {
                        newBoard = (BoardManager.tileType[,])node.resultingBoard.Clone();
                        int value = move
                                    + (distToPlayer > BoardManager.Distance(newPosition, playerPosition) ? moveCloser : 0)
                                    + (distToExit > BoardManager.Distance(newPosition, exitPosition) ? closerToTarget : 0)
                                    ;
                        newBoard[position.x, position.y] = BoardManager.tileType.ground;
                        newBoard[newPosition.x, newPosition.y] = BoardManager.tileType.enemy;
                        MMNode child = AddNode(node, MMNode.ActionType.Move, value, new Coord(newPosition), newBoard);
                        CreatePlayerMoveNodes(child, level - 1);
                    }else if (nextTile == BoardManager.tileType.player)
                    {
                        MMNode child = AddNode(node, MMNode.ActionType.Atk, atkValue, new Coord(newPosition));
                        CreatePlayerMoveNodes(child, level - 1);
                    }
                }

            }
        }

    }

    private BoardManager.tileType GetType(BoardManager.tileType[,] board, Coord newPosition)
    {
        if((newPosition.x >= 0 && newPosition.x < mapWidth && newPosition.y >= 0 && newPosition.y < mapHeight))
             return board[newPosition.x, newPosition.y];
        else
        {
             return BoardManager.tileType.outOfLimits;
        }
    }

    private void CreatePlayerMoveNodes(MMNode parent, int level)
    {
        if (level <= 0)
            return;
        Coord newPosition = new Coord();
        BoardManager.tileType[,] newBoard = parent.resultingBoard;
        float playerDistToExit = BoardManager.Distance(playerPosition, exitPosition);
        Coord position = FindPlayer(parent.resultingBoard);
        BoardManager.tileType nextTile;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 || j == 0)
                {
                    newPosition.x = playerPosition.x + i;
                    newPosition.y = playerPosition.y + j;
                    nextTile = GetType(parent.resultingBoard, newPosition);
                    if (nextTile == BoardManager.tileType.ground)
                    {
                        newBoard = (BoardManager.tileType[,])parent.resultingBoard.Clone();
                        int value = -1 * ( move
                                    + (playerDistToExit > BoardManager.Distance(newPosition, exitPosition) ? moveCloser : 0) 
                                    );

                        newBoard[position.x, position.y] = BoardManager.tileType.ground;
                        newBoard[newPosition.x, newPosition.y] = BoardManager.tileType.player;
                        MMNode child = AddNode(parent, MMNode.ActionType.Move, value, new Coord(newPosition), newBoard);
                        CreateEnemyMoveNodes(child, parent.targetPosition, level-1);
                    }
                    else if (nextTile == BoardManager.tileType.enemy)
                    {
                        MMNode child = AddNode(parent, MMNode.ActionType.Atk, -atkValue, new Coord(newPosition));
                        CreateEnemyMoveNodes(child, parent.targetPosition, level - 1);
                    }
                }

            }
        }
       
    }

    Coord FindPlayer(BoardManager.tileType[,] board)
    {
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                if(board[j,i] == BoardManager.tileType.player)
                {
                    return new Coord(j, i); 
                }
            }
        }
        return null;
    }

    MMNode AddNode(MMNode parent, MMNode.ActionType at, int value , Coord coordinate, BoardManager.tileType[,] board = null)
    {
        MMNode child = new MMNode(at, value, coordinate);
        child.parent = parent;
        child.resultingBoard = board == null ? parent.resultingBoard : board;
        parent.children.Add(child);
        return child;
    }

    //void AddNode(MMNode parent, MMNode child, bool doNextTreeLevel = false, bool playerMove = true)
    //{
    //    parent.children.Add(child);
    //    if (doNextTreeLevel)
    //    {
    //        if (playerMove)
    //            CreatePlayerMoveNodes(child);
    //        else
    //            CreateEnemyMoveNodes(child, child.parent.targetPosition);
    //    }
    //}

    MMNode DoMinimax(MMNode node, int depth, bool maxPlayer)
    {
        if (depth == 0 || node.children == null)
        {
            return node.parent.parent;
        }

        MMNode resultingNode = null, currNode = null;
        float bestValue = 0, currValue;
        if (maxPlayer)
        {
            bestValue = -9999999999;
            for (int i = 0; i < node.children.Count; i++)
            {
                currNode = this.DoMinimax(node.children[i], depth - 1, false);
                if (currNode == null) continue;
                currValue = currNode.value;
                if (currValue > bestValue)
                {
                    bestValue = currValue;
                    resultingNode = currNode;
                }
                //bestValue = currValue > bestValue ? currValue : bestValue;// Mathf.Max(currValue, bestValue);
            }
            return resultingNode;
        }
        else
        {
            bestValue = 9999999999;
            for (int i = 0; i < node.children.Count; i++)
            {

                currNode = this.DoMinimax(node.children[i], depth - 1, true);
                if (currNode == null) continue;
                currValue = currNode.value;
                if (currValue < bestValue)
                {
                    bestValue = currValue;
                    resultingNode = currNode;
                }
                return resultingNode;
            }
        }
        return resultingNode;
    }

    void PrintTree(MMNode node)
    {
        for (int i = 0; i < node.children.Count; i++)
        {
            Debug.Log(node.children[i].value);
        }
    }

}

    

    public class MMNode
    {
        public enum ActionType { Move, Atk };
        public ActionType action;
        public Coord targetPosition;
        public float value;
        public List<MMNode> children = new List<MMNode>();
        public BoardManager.tileType[,] resultingBoard;
        public MMNode parent;
        public MMNode()
        {

        }

        public MMNode(ActionType at, float _value, Coord pos)
        {
            action = at;
            value = _value;
            targetPosition = pos;
        }


    }
