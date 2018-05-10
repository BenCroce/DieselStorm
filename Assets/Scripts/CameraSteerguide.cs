using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSteerguide : MonoBehaviour {

    public Transform guide;

	// Update is called once per frame
	void FixedUpdate()
    {
        if (!Input.GetKey(KeyCode.C))
            transform.rotation = guide.rotation;
    }
}
