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
using System.Linq;

[CreateAssetMenu(menuName = "TeamSetupSingleton")]
public class TeamSetupSingleton : ScriptableObject
{
    [System.Serializable]
    public class LobbyPlayerEntry
    {
        public LobbyPlayer Player;
        public string Name;
        public int ID;
    }

    [System.Serializable]
    public class PlayerBehaviourEntry
    {
        public PlayerBehaviour Player;
        public string Name;
        public int ID;
    }

    private static TeamSetupSingleton _instance;

    public static TeamSetupSingleton Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.FindObjectsOfTypeAll<TeamSetupSingleton>().FirstOrDefault();
            return _instance;
        }
    }
    public TeamSriptable m_RedTeam;
    public TeamSriptable m_BlueTeam;
    public GameObject m_PlayerPrefab;
    public List<LobbyPlayerEntry> m_lobbyPlayerEntries = new List<LobbyPlayerEntry>();
    public List<PlayerBehaviourEntry> m_PlayerBehaviourEntries = new List<PlayerBehaviourEntry>();
    public List<LobbyPlayer> m_LobbyPlayers = new List<LobbyPlayer>();
    public List<PlayerBehaviour> m_PlayerBehaviours = new List<PlayerBehaviour>();
    private int m_playerCount = 0;

    void OnEnable()
    {
        m_PlayerBehaviourEntries = new List<PlayerBehaviourEntry>();
        m_PlayerBehaviours = new List<PlayerBehaviour>();
        m_LobbyPlayers = new List<LobbyPlayer>();
        m_lobbyPlayerEntries = new List<LobbyPlayerEntry>();
    }

    public void ResetLists()
    {
        m_lobbyPlayerEntries = new List<LobbyPlayerEntry>();
        m_PlayerBehaviours = new List<PlayerBehaviour>();
        m_PlayerBehaviourEntries = new List<PlayerBehaviourEntry>();
        m_LobbyPlayers = new List<LobbyPlayer>();
    }

    public void OnPlayerRemoved(Object[] args)
    {
        var player = args[0] as LobbyPlayer;
        m_LobbyPlayers.Remove(player);
        m_lobbyPlayerEntries = new List<LobbyPlayerEntry>();
        m_LobbyPlayers.ForEach(x => m_lobbyPlayerEntries.Add
            (
                new LobbyPlayerEntry() {Name = x.name, Player = x, ID = x.GetInstanceID()}
            )
        );

        for (int i = 0; i < m_PlayerBehaviours.Count; i++)
        {
            if(m_LobbyPlayers.Count != 0)
                for (int j = 0; j < m_LobbyPlayers.Count; j++)
                {
                    if (m_PlayerBehaviours[i].LobbyID == m_lobbyPlayerEntries[j].ID)
                        break;
                    if (j == m_LobbyPlayers.Count - 1 &&
                        m_PlayerBehaviours[i].LobbyID != m_lobbyPlayerEntries[j].ID)
                    {
                        m_BlueTeam.RemovePlayer(m_PlayerBehaviours[i]);
                        m_RedTeam.RemovePlayer(m_PlayerBehaviours[i]);
                        if (m_PlayerBehaviours[i] != null)
                            Destroy(m_PlayerBehaviours[i].gameObject);
                        m_PlayerBehaviours.Remove(m_PlayerBehaviours[i]);
                        i--;
                    }
                }
            else
            {
                m_BlueTeam.RemovePlayer(m_PlayerBehaviours[i]);
                m_RedTeam.RemovePlayer(m_PlayerBehaviours[i]);
                if (m_PlayerBehaviours[i] != null)
                    Destroy(m_PlayerBehaviours[i].gameObject);
                m_PlayerBehaviours.Remove(m_PlayerBehaviours[i]);
                i--;
            }
        }        

        m_PlayerBehaviourEntries = new List<PlayerBehaviourEntry>();
        m_PlayerBehaviours.ForEach(x => m_PlayerBehaviourEntries.Add
            (
                new PlayerBehaviourEntry() { Name = x.m_ScreenName, Player = x}
            )
        );
        AutoBalance();
    }

    public void OnPlayerAdded(Object[] args)
    {
        var player = args[0] as LobbyPlayer;
        m_LobbyPlayers.Add(player);
        m_lobbyPlayerEntries = new List<LobbyPlayerEntry>();
        m_LobbyPlayers.ForEach(x => m_lobbyPlayerEntries.Add
            (new LobbyPlayerEntry() {Name = x.playerName, Player = x, ID = x.GetInstanceID()}));
        AutoBalance();
    }    

    public void AutoBalance()
    {
        //Makes sure for every lobby player there is a playerBehaviour
        foreach (var lobbyPlayer in m_LobbyPlayers)
        {
            bool playerExists = false;
            foreach (var playerBehaviour in m_PlayerBehaviours)
            {
                if (playerBehaviour.LobbyID == lobbyPlayer.GetInstanceID())
                {
                    playerExists = true;
                    break;
                }
            }
            if (!playerExists)
            {
                var newPlayer = Instantiate(m_PlayerPrefab);
                var behaviour = newPlayer.GetComponent<PlayerBehaviour>();
                behaviour.LobbyID = lobbyPlayer.GetInstanceID();
                behaviour.m_ScreenName = lobbyPlayer.playerName;                
                m_PlayerBehaviours.Add(behaviour);
            }
        }
        m_PlayerBehaviourEntries = new List<PlayerBehaviourEntry>();
        m_PlayerBehaviours.ForEach(x => m_PlayerBehaviourEntries.Add
            (
                new PlayerBehaviourEntry() { Name = x.m_ScreenName, Player = x, ID = x.LobbyID }
            )            
        );
        for (int i = 0; i < m_PlayerBehaviours.Count; i++)
        {
            m_BlueTeam.AddPlayer(m_PlayerBehaviours[i]);
            m_LobbyPlayers[i].playerColor = Color.blue;
            m_PlayerBehaviours[i].m_TeamColor = Color.blue;
            i++;
            if (i >= m_PlayerBehaviours.Count)
                break;
            m_RedTeam.AddPlayer(m_PlayerBehaviours[i]);
            m_LobbyPlayers[i].playerColor = Color.red;
            m_PlayerBehaviours[i].m_TeamColor = Color.red;
        }

    }
}
