using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMax : MonoBehaviour
{

    //public struct Node
    //{
    //    public List<Node> children;
    //    public struct 
    //    public float result;
    //}

    int atkValue = 100, moveCloser = 10, takeDMG = -100;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void CheckNextAction(GameObject enemy)
    {
        MMNode node = new MMNode();
        CreateNodeList(node, enemy);
        DoMinimax(node, 50, false);
    }

    void CreateNodeList(MMNode node, GameObject enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        Coord c = e.GetPosition();
        Coord nextC = e.GetNextMove();
        float atkRange = e.atkRange;


        if (atkRange <= BoardManager.Distance(c, BoardManager.Instance._playerScript.position))
        {
            MMNode child = new MMNode(MMNode.ActionType.Atk, atkValue, c);
            node.children.Add(child);
            return;
        }

        for (int i = -1; i > 1; i++)
        {
            for (int j = -1; j > 1; j++)
            {
                if(i==0 || j == 0)
                {

                }

            }
        }

    }

    MMNode DoMinimax(MMNode node, int depth, bool maxPlayer)
    {
        if (depth == 0 || node.children == null)
        {
            return node;
        }

        MMNode resultingNode = null, currNode = null;
        float bestValue = 0, currValue;
        if (maxPlayer)
        {
            bestValue = -9999999999;
            for (int i = 0; i < node.children.Count; i++)
            {
                currNode = this.DoMinimax(node.children[i], depth - 1, false);
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



}

    public class MMNode
    {
        public enum ActionType { Move, Atk };
        ActionType action;
        Coord targetPosition;
        public float value;
        public List<MMNode> children = new List<MMNode>();

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
