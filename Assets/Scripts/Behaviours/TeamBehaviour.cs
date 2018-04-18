using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamBehaviour : NetworkBehaviour
{
    public TeamSriptable m_TeamScriptable;
    public Transform m_SpawnLocation;
    public GameEventArgs m_PlayerRespawn;

    void Start()
    {
        StartCoroutine(InitalSpawn());
    }

    public void SpawnTank(Object[] args)
    {
        var senderGameObject = args[0] as PlayerBehaviour;
        if (m_TeamScriptable.m_Players.Contains(senderGameObject))
        {
            m_RespawningPlayer = senderGameObject;
            CmdSpawnTank();            
        }
    }

    public PlayerBehaviour m_RespawningPlayer;

    [ContextMenu("Force Spawn")]
    [Command]
    public void CmdSpawnTank()
    {
        GameObject tank = Instantiate(m_TeamScriptable.m_HeavyTankPrefab);
        NetworkServer.Spawn(tank);
        m_PlayerRespawn.Raise(this, m_RespawningPlayer, m_SpawnLocation, tank);       
        m_RespawningPlayer = null;
    }

    IEnumerator InitalSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            foreach (var player in m_TeamScriptable.m_Players)
            {
                GameObject tank = Instantiate(m_TeamScriptable.m_HeavyTankPrefab);
                m_PlayerRespawn.Raise(this, player, m_SpawnLocation, tank);
                NetworkServer.Spawn(tank);
            }
            break;
        }
    }
}
