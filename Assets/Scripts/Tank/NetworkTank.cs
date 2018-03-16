using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(LightTankMovement))]
[RequireComponent(typeof(TankShoot))]
public class NetworkTank : NetworkBehaviour {

    LightTankMovement m_movement;
    TankShoot m_shoot;
    Rigidbody m_body;

    public int updatesPerSecond = 60;

	// Use this for initialization
	void Start ()
    {
        m_movement = GetComponent<LightTankMovement>();
        m_shoot = GetComponent<TankShoot>();
        m_body = GetComponent<Rigidbody>();
        StartCoroutine(InputSync());
        if (isLocalPlayer)
            StartCoroutine(Shoot());
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		if (isLocalPlayer)
        {
            m_movement.m_hinput = Input.GetAxis("Horizontal");
            m_movement.m_vinput = Input.GetAxis("Vertical");
            m_movement.m_jinput = Input.GetAxis("Jump");
            m_movement.m_steerinput = m_movement.m_steerGuide.forward;
        }
	}

    //Send out input information to other players
    [Command]
    void CmdPlayer(float h, float v, float j, Vector3 m, Vector3 p, Vector3 b)
    {
        RpcPlayer(h, v, j, m, p, b);
    }

    //Update input variables from other players
    [ClientRpc]
    void RpcPlayer(float h, float v, float j, Vector3 m, Vector3 p, Vector3 b)
    {
        if (!isLocalPlayer)
        {
            m_movement.m_hinput = h;
            m_movement.m_vinput = v;
            m_movement.m_jinput = j;
            m_movement.m_steerinput = m;
            m_body.position = p;
            m_body.velocity = b;
        }
    }

    [Command]
    void CmdShoot()
    {
        GameObject shot = m_shoot.Shoot();
        NetworkServer.Spawn(shot);
    }

    //X times per second, send input information from the local player to the rest of the players
    IEnumerator InputSync()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / updatesPerSecond);
            if (isLocalPlayer)
            {
                CmdPlayer(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),
                    Input.GetAxis("Jump"), m_movement.m_steerGuide.forward, m_body.position, m_body.velocity);
            }
        }
    }

    IEnumerator Shoot()
    {
        while (true)
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                CmdShoot();
                yield return new WaitForSeconds(m_shoot.m_shootCooldown);
            }
            else
                yield return new WaitForFixedUpdate();

    }
}
 