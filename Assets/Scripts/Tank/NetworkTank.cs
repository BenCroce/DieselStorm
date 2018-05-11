﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[NetworkSettings(channel = Channels.DefaultUnreliable, sendInterval = 0.05f)]
public class NetworkTank : NetworkBehaviour {

    public SimplePlayerBehaviour m_player;
    public NetworkIdentity local;
    public NetworkTankInputController input;
    public TankMovementBehaviour m_movement;
    public TurrentAimBehaviour m_aim;
    public Transform m_steerAimGuide;
    Rigidbody m_body;

    bool getting = true;

    public int updatesPerSecond = 60;

	//Use this for initialization
	void Start()
    {
        local = GetComponent<NetworkIdentity>();

        StartCoroutine(GetTank());

        //If this is the local player, set that steeraimguide        
        m_steerAimGuide = Camera.main.transform;
        StartCoroutine(InputSync());
	}

    private void FixedUpdate()
    {
        if (!m_player)
            return;
        if (m_movement && m_body && m_aim && input)
        {
            m_movement.m_steerForward = m_steerAimGuide.forward;
            m_aim.m_aimRotation = m_steerAimGuide.rotation.eulerAngles;
        }
        else if (!getting && m_movement == null)
        {
            getting = true;
            StartCoroutine(GetTank());
        }
        else if (m_movement && m_body && m_aim && input)
            m_movement.Move(input.Hinput, input.Vinput, input.Jinput);
    }

    //Send out input information to other players
    [Command]
    void CmdPlayer(float h, float v, float j, Vector3 m, Vector3 p, Vector3 b, Vector3 a)
    {
        if (!m_player)
            return;
        RpcPlayer(h, v, j, m, p, b, a);
    }

    //Update input variables from other players
    [ClientRpc]
    void RpcPlayer(float h, float v, float j, Vector3 m, Vector3 p, Vector3 b, Vector3 a)
    {
        if (!m_player)
            return;
        if (!isLocalPlayer && m_movement && m_body && m_aim && input)
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
            if (isLocalPlayer && m_movement && m_body && m_aim && input)
            {
                CmdPlayer(input.Hinput, input.Vinput,
                    input.Jinput, m_steerAimGuide.forward, m_body.position, m_body.velocity, m_steerAimGuide.rotation.eulerAngles);
            }
        }
    }

    IEnumerator GetTank()
    {
        while (m_player.m_rtTankObject == null && m_body == null)
        {
            yield return new WaitForSeconds(0.1f);
            Debug.Log("Searching for scene object...");
        }
        //Get all of this stuff from the tank
        m_body = m_player.m_rtTankObject.GetComponent<Rigidbody>();
        m_aim = m_player.m_rtTankObject.GetComponent<TurrentAimBehaviour>();
        m_movement = m_player.m_rtTankObject.GetComponent<TankMovementBehaviour>();
        input = m_player.m_rtTankObject.GetComponent<NetworkTankInputController>();
        m_player.m_rtTankObject.GetComponent<TankShoot>().player = this;
        getting = false;
    }
}
 