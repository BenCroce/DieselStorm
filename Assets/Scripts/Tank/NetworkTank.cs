using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(TankMovementBehaviour))]
[RequireComponent(typeof(NetworkTankInputController))]
[NetworkSettings(channel = Channels.DefaultReliable, sendInterval = 0.05f)]
public class NetworkTank : NetworkBehaviour {

    public NetworkTankInputController input;
    public TankMovementBehaviour m_movement;
    public TurrentAimBehaviour m_aim;
    public Transform m_steerAimGuide;
    public PlayerBehaviour m_player;
    Rigidbody m_body;

    public int updatesPerSecond = 60;

	// Use this for initialization
	void Start ()
    {
        m_player = GetComponent<PlayerBehaviour>();
        m_body = GetComponent<Rigidbody>();
        if (m_player.isLocalPlayer)
            m_steerAimGuide = Camera.main.transform;
        StartCoroutine(InputSync());
	}

    private void FixedUpdate()
    {
        if (m_player.isLocalPlayer)
        {
            m_movement.m_steerForward = m_steerAimGuide.forward;
            m_aim.m_aimRotation = m_steerAimGuide.rotation.eulerAngles;
        }
    }

    //Send out input information to other players
    [Command]
    void CmdPlayer(float h, float v, float j, Vector3 m, Vector3 p, Vector3 b, Vector3 a)
    {
        RpcPlayer(h, v, j, m, p, b, a);
    }

    //Update input variables from other players
    [ClientRpc]
    void RpcPlayer(float h, float v, float j, Vector3 m, Vector3 p, Vector3 b, Vector3 a)
    {
        if (!m_player.isLocalPlayer)
        {
            input.Hinput = h;
            input.Vinput = v;
            input.Jinput = j;
            m_movement.m_steerForward = m;
            m_body.position = p;
            m_body.velocity = b;
            m_aim.m_aimRotation = a;
        }
    }

    //X times per second, send input, positional, and velocity information from the local player to the rest of the players
    IEnumerator InputSync()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / updatesPerSecond);
            if (m_player.isLocalPlayer)
            {
                CmdPlayer(input.Hinput, input.Vinput,
                    input.Jinput, m_steerAimGuide.forward, m_body.position, m_body.velocity, m_steerAimGuide.rotation.eulerAngles);
            }
        }
    }
}
 