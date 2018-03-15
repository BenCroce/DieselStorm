﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankShoot : MonoBehaviour {

    public GameObject m_shell;
    public Transform m_turretPos;

    //These will be replaced by Tank/Shell Stats
    public float m_shootForce = 200;
    public float m_shootCooldown = 1;
    

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
    }

    public GameObject Shoot()
    {
        GameObject shot = Instantiate(m_shell, m_turretPos.position, m_turretPos.rotation);
        shot.GetComponent<Rigidbody>().velocity = shot.transform.forward * m_shootForce + GetComponent<Rigidbody>().velocity;
        return shot;
    }
}
