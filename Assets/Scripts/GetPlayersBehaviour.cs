using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetPlayersBehaviour : NetworkBehaviour
{
    public GameEventArgs m_OnPlayerConnected;
    public TeamBehaviour m_RedTeam;
    public TeamBehaviour m_BlueTeam;
    public GameObject m_PlayerPrefab;

    public List<LobbyPlayer> m_LobbyPlayers;
    public List<PlayerBehaviour> m_PlayerBehaviours;
    private int m_playerCount = 0;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        m_LobbyPlayers = new List<LobbyPlayer>();

    }    

    void Update()
    {
        m_LobbyPlayers = FindObjectsOfType<LobbyPlayer>().ToList();
        for (int i = 0; i < m_PlayerBehaviours.Count; i++)
        {
            for (int j = 0; j < m_LobbyPlayers.Count; j++)
            {
                if (m_PlayerBehaviours[i].LobbyID == m_LobbyPlayers[j].GetInstanceID())
                    break;
                if (j == m_LobbyPlayers.Count &&
                    m_PlayerBehaviours[i].LobbyID != m_LobbyPlayers[j].GetInstanceID())
                {
                    Destroy(m_PlayerBehaviours[i].gameObject);
                    m_PlayerBehaviours.RemoveAt(i);
                }
            }
        }

        if (m_LobbyPlayers.Count == 0 || 
            m_LobbyPlayers.Count != FindObjectsOfType<LobbyPlayer>().Length)
        {                     
            m_PlayerBehaviours = new List<PlayerBehaviour>();

            foreach (var player in FindObjectsOfType<LobbyPlayer>())
            {
                if(m_LobbyPlayers.Contains(player))
                    continue;
                m_LobbyPlayers.Add(player);
                var playerObject = Instantiate(m_PlayerPrefab);
                //playerObject.name = player.playerName;
                playerObject.GetComponent<PlayerBehaviour>().LobbyID = player.GetInstanceID();
                playerObject.GetComponent<PlayerBehaviour>().m_ScreenName = player.playerName;
                m_PlayerBehaviours.Add(playerObject.GetComponent<PlayerBehaviour>());

            }
            AutoBalance();
            m_playerCount = m_LobbyPlayers.Count;
        }
    }

    void AutoBalance()
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
