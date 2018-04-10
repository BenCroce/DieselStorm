using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetPlayersBehaviour : MonoBehaviour
{
    
    public GameEventArgs m_OnPlayerDisconnected;
    public TeamBehaviour m_RedTeam;
    public TeamBehaviour m_BlueTeam;
    public GameObject m_PlayerPrefab;

    public List<LobbyPlayer> m_LobbyPlayers = new List<LobbyPlayer>();
    public List<PlayerBehaviour> m_PlayerBehaviours = new List<PlayerBehaviour>();
    private int m_playerCount = 0;
    
    public void OnPlayerRemoved(Object[] args)
    {
        var player = args[0] as LobbyPlayer;
        m_LobbyPlayers.Remove(player);
        
    }

    public void OnPlayerAdded(Object[] args)
    {
        var player = args[0] as LobbyPlayer;
        m_LobbyPlayers.Add(player);
    }


    public void AutoBalance()
    {
        for (int i = 0; i < m_PlayerBehaviours.Count; i += 2)
        {
            if(!m_RedTeam.m_Players.Contains(m_PlayerBehaviours[i]))
                m_RedTeam.AddPlayer(m_PlayerBehaviours[i]);
            if(i + 1 >= m_PlayerBehaviours.Count)
                break;
            if (!m_BlueTeam.m_Players.Contains(m_PlayerBehaviours[i+1]))
                m_BlueTeam.AddPlayer(m_PlayerBehaviours[i+1]);
        }
    }

}
