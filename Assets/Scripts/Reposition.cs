using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour {

	//This is a temporary solution. Do not let this get to the final game.
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.y <= 420)
            transform.position = new Vector3(transform.position.x, 525, transform.position.z);
	}
}
