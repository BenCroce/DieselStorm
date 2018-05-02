using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(channel = Channels.DefaultReliable, sendInterval = 0.05f)]
public class TankShoot : NetworkBehaviour {

    public GameObject m_shell;
    public Transform m_turretPos;

    //These will be replaced by Tank/Shell Stats
    public float m_shootForce = 200;
    public float m_shootCooldown = 1;

    public void Fire(UnityEngine.Object[]args)
    {
        if(args[1] == this.gameObject)
            CmdShoot();
    }

    [Command(channel = Channels.DefaultReliable)]
    public void CmdShoot()
    {
        GameObject shot = Instantiate(m_shell, m_turretPos.position + (GetComponent<Rigidbody>().velocity.normalized * 0.1f), m_turretPos.rotation, this.transform);
        shot.GetComponent<Rigidbody>().velocity = shot.transform.forward * m_shootForce + GetComponent<Rigidbody>().velocity;

        NetworkServer.Spawn(shot);
    }
}