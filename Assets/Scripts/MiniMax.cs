using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMax : MonoBehaviour {

    //public struct Node
    //{
    //    public List<Node> children;
    //    public struct 
    //    public float result;
    //}

    int atkValue = 100, moveCloser = 10, takeDMG = -100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
        
        for (int i = -1; i < length; i++)
        {

        }

    }

    float DoMinimax(MMNode node, int depth, bool maxPlayer)
    {
        if (depth == 0 || node.children == null)
        {
            return node.value;
        }
        float bestValue = 0, currValue;
        if (maxPlayer)
        {
            bestValue = -9999999999;
            for (int i = 0; i < node.children.Count; i++)
            {
                currValue = this.DoMinimax(node.children[i], depth - 1, false);
                bestValue = Mathf.Max(currValue, bestValue);
            }
            return bestValue;
        }
        else
        {
            bestValue = 9999999999;
            for (int i = 0; i < node.children.Count; i++)
            {
                currValue = this.DoMinimax(node.children[i], depth - 1, true);
                bestValue = Mathf.Min(currValue, bestValue);
            }
            return bestValue;
        }
    }
    


  
}

public class MMNode{
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
