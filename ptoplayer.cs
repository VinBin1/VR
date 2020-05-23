using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ptoplayer : MonoBehaviour {
     GameObject theTarget;
    

    
	// Use this for initialization
	void Start () {
        theTarget = GameObject.Find("player(Clone)");
            
            this.transform.parent = theTarget.transform;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
