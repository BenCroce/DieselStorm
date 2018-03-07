using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(LightTankMovement))]
[RequireComponent(typeof(TankShoot))]
public class NetworkTank : NetworkBehaviour {

    LightTankMovement m_movement;
    TankShoot m_shoot;

    public int updatesPerSecond = 60;

	// Use this for initialization
	void Start ()
    {
        m_movement = GetComponent<LightTankMovement>();
        m_shoot = GetComponent<TankShoot>();
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
    void CmdInput(float h, float v, float j, Vector3 m)
    {
        RpcInput(h, v, j, m);
    }

    //Update input variables from other players
    [ClientRpc]
    void RpcInput(float h, float v, float j, Vector3 m)
    {
        if (!isLocalPlayer)
        {
            m_movement.m_hinput = h;
            m_movement.m_vinput = v;
            m_movement.m_jinput = j;
            m_movement.m_steerinput = m;
        }
    }

    [Command]
    void CmdShoot()
    {
        GameObject shot = Instantiate(m_shoot.m_shell, m_shoot.m_turretPos.position, m_shoot.m_turretPos.rotation);
        //shot.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, m_shoot.m_shootForce), ForceMode.Impulse);
        //shot.GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity, ForceMode.Impulse);
        shot.GetComponent<Rigidbody>().velocity = shot.transform.forward * 50 + GetComponent<Rigidbody>().velocity;
        NetworkServer.Spawn(shot);
    }

    //X times per second, send input information from the local player to the rest of the players
    IEnumerator InputSync()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / updatesPerSecond);
            if (isLocalPlayer)
                CmdInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),
                    Input.GetAxis("Jump"), m_movement.m_steerGuide.forward);
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
 