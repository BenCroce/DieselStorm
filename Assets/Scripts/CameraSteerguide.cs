using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSteerguide : MonoBehaviour {

	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.rotation = new Quaternion(0, Camera.main.transform.rotation.y, 0, 0);
    }
}
