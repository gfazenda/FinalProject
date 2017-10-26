using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayTest : MonoBehaviour {
    public GameObject hand, target;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        //ppos.y = 0.5f;
        Vector3 d = target.transform.position - hand.transform.position;
        if (Physics.Raycast(hand.transform.position, d.normalized, out hit,5))
        {
            print("Found an object: " + hit.collider.tag);
        }
        Debug.DrawLine(hand.transform.position, hit.point);
    }
}
