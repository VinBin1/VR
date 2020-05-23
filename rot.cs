using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rot : MonoBehaviour {
    private float speed=3.0f;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        // Slowly rotate the object arond its X axis at 1 degree/second.
        transform.Rotate(0,Time.deltaTime* speed, 0);

    }
}
