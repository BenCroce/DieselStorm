using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamBehaviour : MonoBehaviour
{
    public TeamSriptable m_TeamScriptable;
    public Transform m_SpawnLocation;
    public GameEventArgs m_PlayerRespawn;
    
    public void SpawnTank(Object[] args)
    {
        var player = args[0] as PlayerBehaviour;
        if (m_TeamScriptable.m_Players.Contains(player))
        {
            m_PlayerRespawn.Raise(this, player, m_SpawnLocation);
        }
    }    
}
