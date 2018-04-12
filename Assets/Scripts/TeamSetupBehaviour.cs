using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class TeamSetupBehaviour : MonoBehaviour
{
    public TeamSetupSingleton m_TeamSetupSingleton;
    public LobbyPlayerList m_LobbyList;
    public int m_CurrentPlayers;
        
    void Update()
    {
        if (m_LobbyList == null)
            m_LobbyList = Resources.FindObjectsOfTypeAll<LobbyPlayerList>()[1];

        if (m_CurrentPlayers != m_LobbyList.Players.Count)
        {
            m_CurrentPlayers = m_LobbyList.Players.Count;
            for (int i = 0; i < m_TeamSetupSingleton.m_LobbyPlayers.Count; i++)
            {
                if (!m_LobbyList.Players.Contains(m_TeamSetupSingleton.m_LobbyPlayers[i]))
                {
                    m_TeamSetupSingleton.OnPlayerRemoved(new Object[] { m_TeamSetupSingleton.m_LobbyPlayers[i] });
                }                
            }
            for (int i = 0; i < m_CurrentPlayers; i++)
            {
                if (!m_TeamSetupSingleton.m_LobbyPlayers.Contains(m_LobbyList.Players[i]))
                {
                    m_TeamSetupSingleton.OnPlayerAdded(new Object[] {m_LobbyList.Players[i]});
                }
            }
        }

        if (m_CurrentPlayers == 0)
        {
            foreach (var player in FindObjectsOfType<PlayerBehaviour>())
            {
                Destroy(player);
            }
            m_TeamSetupSingleton.ResetLists();
        }


        m_CurrentPlayers = m_LobbyList.Players.Count;
    }
}
