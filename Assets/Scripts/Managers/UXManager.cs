using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UXManager : MonoBehaviour {
    private static UXManager _instance;

    public static UXManager instance { get { return _instance; } }

    private void Awake()
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

        // Use this for initialization
        void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
