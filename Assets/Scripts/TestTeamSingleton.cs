using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "TeamSingleton")]
public class TestTeamSingleton : ScriptableObject
{
    [System.Serializable]
    public class LobbyPlayerEntry
    {
        public LobbyPlayer Player;
        public string Name;
    }
    public static TestTeamSingleton _instance;
    public GameEventArgs m_OnPlayerDisconnected;
    public TeamBehaviour m_RedTeam;
    public TeamBehaviour m_BlueTeam;
    public GameObject m_PlayerPrefab;
    public List<LobbyPlayerEntry> m_lobbyPlayerEntries = new List<LobbyPlayerEntry>();
    public List<LobbyPlayer> m_LobbyPlayers = new List<LobbyPlayer>();
    public List<PlayerBehaviour> m_PlayerBehaviours = new List<PlayerBehaviour>();
    private int m_playerCount = 0;

    void OnEnable()
    {
        if (_instance == null)
            _instance = this;
    }


    public void OnPlayerRemoved(Object[] args)
    {
        var player = args[0] as LobbyPlayer;
        m_LobbyPlayers.Remove(player);
        m_lobbyPlayerEntries = new List<LobbyPlayerEntry>();
        m_LobbyPlayers.ForEach(x => m_lobbyPlayerEntries.Add
            (
                new LobbyPlayerEntry() { Name = x.name, Player = x }
            )
        );

    }

    public void OnPlayerAdded(Object[] args)
    {
        var player = args[0] as LobbyPlayer;
        m_LobbyPlayers.Add(player);
        m_lobbyPlayerEntries = new List<LobbyPlayerEntry>();
        m_LobbyPlayers.ForEach(x => m_lobbyPlayerEntries.Add
        (
            new LobbyPlayerEntry() { Name = x.name, Player = x }
            )
        );

    }


    public void AutoBalance()
    {
        for (int i = 0; i < m_PlayerBehaviours.Count; i += 2)
        {
            if (!m_RedTeam.m_Players.Contains(m_PlayerBehaviours[i]))
                m_RedTeam.AddPlayer(m_PlayerBehaviours[i]);
            if (i + 1 >= m_PlayerBehaviours.Count)
                break;
            if (!m_BlueTeam.m_Players.Contains(m_PlayerBehaviours[i + 1]))
                m_BlueTeam.AddPlayer(m_PlayerBehaviours[i + 1]);
        }
    }
}
