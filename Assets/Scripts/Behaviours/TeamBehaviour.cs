using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamBehaviour : NetworkBehaviour
{
    public TeamSriptable m_TeamScriptable;
    public Transform m_SpawnLocation;
    public GameEventArgs m_PlayerRespawn;
        
    [ContextMenu("Force Spawn")]
    [Command]
    public void CmdSpawnTank()
    {
        GameObject tank = Instantiate(m_TeamScriptable.m_HeavyTankPrefab);
        m_PlayerRespawn.Raise(this, m_TeamScriptable.m_Players[0], m_SpawnLocation, tank);
        NetworkServer.Spawn(tank);

        //var player = args[0] as PlayerBehaviour;
        //if (m_TeamScriptable.m_Players.Contains(player))
        //{
        //    m_PlayerRespawn.Raise(this, player, m_SpawnLocation, 
        //        m_TeamScriptable.m_HeavyTankPrefab);
        //}
    }    
}
