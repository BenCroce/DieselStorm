using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    
    public float offset;
    public float smoothSpeed = 0.3f;
    public float MouseSens = 1.2f;

    Transform target;
    float MouseAimX;
    float MouseAimY;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GameObject.FindGameObjectWithTag("Player") && target == null)
            target = GameObject.FindGameObjectsWithTag("Player")[GameObject.FindGameObjectsWithTag("Player").Length-1].transform;

        MouseAimX += Input.GetAxis("Mouse X");
        MouseAimY += Input.GetAxis("Mouse Y");
	}

    void FixedUpdate()
    {
        if (target == null)
            return;

        float rot = MouseAimX * Mathf.PI / 180;
        float rot2 = MouseAimY * Mathf.PI / 180;

        //Rotate the camera around the player
        Vector3 pos = target.position + new Vector3(offset * -Mathf.Sin(rot * MouseSens), 3, -offset * Mathf.Cos(rot * MouseSens));
        Vector3 smoothMove = Vector3.Lerp(transform.position, pos, smoothSpeed);
        //Quaternion smoothRotate = Quaternion.Lerp(transform.rotation, target.rotation, smoothSpeed);

        transform.position = smoothMove;
        
        transform.LookAt(target.position + new Vector3(0,4,0), Vector3.up);
    }
}
