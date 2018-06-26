using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    float moveTimer = 0;
    float moveDuration = 0.4f;

	Vector3 targetPos;
	// Use this for initialization
	void Start () {
		
	}

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		
	}

	public void Shot(Vector3 origin, Vector3 target){
		this.transform.position = origin;
		targetPos = target;
				Debug.Log("will go");
				moveTimer=0;
		this.gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position != targetPos)
			Move();
		else{
			this.gameObject.SetActive(false);
		}
	}

	protected void Move()
    {
        moveTimer += Time.deltaTime;
        float step = moveTimer / moveDuration;
        //transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
		//Debug.Log("im going");
        transform.position = Vector3.Lerp(transform.position, targetPos, step);
    }
}
