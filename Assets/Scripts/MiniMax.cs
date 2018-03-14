using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMax : MonoBehaviour {

    public struct Node
    {
        public List<Node> children;
        public float result;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    float DoMinimax(Node node, int depth, bool maxPlayer)
    {
        if (depth == 0 || node.children == null)
        {
            return node.result;
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
