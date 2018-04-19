using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetFocus : MonoBehaviour {

    public Cinemachine.CinemachineFreeLook m_cam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setFocus(UnityEngine.Object[] args)
    {
        var sender = args[0] as PlayerBehaviour;
        var obj = args[1] as GameObject;
        if(sender == GetComponentInParent<PlayerBehaviour>())
        {
            m_cam.m_Follow = obj.transform;
            m_cam.m_LookAt = obj.transform;
        }        
    }
}
