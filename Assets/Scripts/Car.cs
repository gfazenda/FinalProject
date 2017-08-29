using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    private NeuralNetwork net;
    private Rigidbody rBody;
    private Material[] mats;
    // Use this for initialization
    void Start () {
        rBody = this.GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        rBody.velocity = (5f * transform.forward);
        rBody.rotation = Quaternion.Euler(new Vector3( 0, 0, 0));
    }
}
