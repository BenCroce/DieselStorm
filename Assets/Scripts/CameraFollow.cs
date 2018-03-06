using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    
    public float m_offset;
    public float m_lerpSpeed = 0.2f;
    public float m_aimSensitivity = 1.2f;

    Transform target;
    float m_aimX;
    float m_aimY;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GameObject.FindGameObjectWithTag("Player") && target == null)
            target = GameObject.FindGameObjectsWithTag("Player")[GameObject.FindGameObjectsWithTag("Player").Length-1].transform;

        #region Testing
        m_aimX += Input.GetAxis("Mouse X");
        m_aimY += Input.GetAxis("Mouse Y");
        #endregion
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        float rot = m_aimX * Mathf.PI / 180;
        float rot2 = m_aimY * Mathf.PI / 180;

        //Rotate the camera around the player
        Vector3 pos = target.position + new Vector3(m_offset * -Mathf.Sin(rot * m_aimSensitivity), 3, -m_offset * Mathf.Cos(rot * m_aimSensitivity));
        Vector3 smoothMove = Vector3.Lerp(transform.position, pos, m_lerpSpeed);
        //Quaternion smoothRotate = Quaternion.Lerp(transform.rotation, target.rotation, smoothSpeed);

        transform.position = smoothMove;
        
        transform.LookAt(target.position + new Vector3(0,4,0), Vector3.up);
    }
}
