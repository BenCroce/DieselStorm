using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(channel = Channels.DefaultReliable, sendInterval = 0.05f)]
public class TankShoot : NetworkBehaviour {

    public GameObject m_shell;
    public Transform m_turretPos;
    public NetworkTank player;

    //These will be replaced by Tank/Shell Stats
    public float m_shootForce = 200;
    public float m_shootCooldown = 1;

    bool canshoot = true;

    public void Fire(UnityEngine.Object[]args)
    {
        if(args[1] == this.gameObject)
            CmdShoot();
    }

    [Command]
    public void CmdShoot()
    {
        if (canshoot)
        {
            GameObject shot = Instantiate(m_shell, m_turretPos.position + (GetComponent<Rigidbody>().velocity.normalized * 0.1f), m_turretPos.rotation, this.transform);
            shot.GetComponent<Rigidbody>().velocity = shot.transform.forward * m_shootForce + GetComponent<Rigidbody>().velocity;
            NetworkServer.SpawnWithClientAuthority(shot, player.gameObject);
            RpcShoot(shot, gameObject);
            canshoot = false;
            StartCoroutine(Cooldown());
        }
    }

    [ClientRpc]
    public void RpcShoot(GameObject shot, GameObject parent)
    {
        //if (shot != null)
        {
            shot.transform.parent = parent.transform;            
            shot.GetComponent<Rigidbody>().velocity = shot.transform.forward * m_shootForce + parent.GetComponent<Rigidbody>().velocity;
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(m_shootCooldown);
        canshoot = true;
    }
}