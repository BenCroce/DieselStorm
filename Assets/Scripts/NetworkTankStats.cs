using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(channel = 0, sendInterval = 0.05f)]
public class NetworkTankStats : NetworkBehaviour
{
    public TankStats m_stats;
    NetworkTank player;

	// Use this for initialization
	void Start ()
    {


    }

    private void Update()
    {
        if (player == null)
        {
            if (GetComponent<TankShoot>())
                player = GetComponent<TankShoot>().player;
            if (GetComponent<HitscanShootBehaviour>())
                player = GetComponent<HitscanShootBehaviour>().player;
        }
    }

    public void CallCmdStats()
    {
        if (player.isLocalPlayer)
        {
            float h = m_stats.rt_Health.m_Value;
            float a = m_stats.rt_Armor.m_Value;
            CmdStats(h, a);
        }
    }

    [Command]
    void CmdStats(float h, float a)
    {
        if (!player.isLocalPlayer)
        {
            RpcStats(h, a);
        }
    }

    [ClientRpc]
    void RpcStats(float h, float a)
    {
        m_stats.rt_Health.m_Value = h;
        m_stats.rt_Armor.m_Value = a;
    }


}
