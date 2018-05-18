using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour {

    public float speed = 10f;


    void FixedUpdate()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
