using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SimpleTeamBehaviour : NetworkBehaviour
{
    public List<SimplePlayerBehaviour> m_Players;
    [SyncVar]public int m_MaxPlayers;
    [SyncVar]public Color m_TeamColor;
    [SyncVar]public int m_StartingTeamLives;
    [SyncVar]public int m_CurrentTeamLives;
    public GameEventArgs m_PlayerRespawn;
    [SyncVar]public bool m_PlayerOnTeam;

    void Awake()
    {
        m_CurrentTeamLives = m_StartingTeamLives;
    }

    public void Add(SimplePlayerBehaviour player)
    {
        if(!isServer)
            return;
        if(m_Players.Contains(player) || m_Players.Count >= m_MaxPlayers)
            return;
        m_Players.Add(player);
        m_PlayerOnTeam = true;   
        CmdSpawnPlayer(player.gameObject);           
    }

    public void PlayerDied(Object[] args)
    {
        var player = args[0] as SimplePlayerBehaviour;
        if (m_Players.Contains(player))
        {
            m_CurrentTeamLives--;
            if(m_CurrentTeamLives > 0)
                m_PlayerRespawn.Raise(player, this);
        }
    }

    [Command]
    public void CmdSpawnPlayer(GameObject player)
    {
        player.GetComponent<SimplePlayerBehaviour>().CmdSetTeamColor(m_TeamColor);
        player.GetComponent<SimplePlayerBehaviour>().SpawnNewTank(new Object[] 
        { player.GetComponent<SimplePlayerBehaviour>(), this});
    }

    public void Remove(SimplePlayerBehaviour player)
    {
        if(!isServer)
            return;
        if (m_Players.Contains(player))
        {
            m_Players.Remove(player);
        }
    }
}
