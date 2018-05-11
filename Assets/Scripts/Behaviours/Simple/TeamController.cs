using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamController : NetworkBehaviour
{
    public List<SimpleTeamBehaviour> m_Teams;
    public int m_JoinIndex;

    public void OnPlayerJoined(NetworkIdentity connectionIdentity)
    {
        if(!isServer)
            return;
        foreach (var player in FindObjectsOfType<SimplePlayerBehaviour>())
        {
            if (player.GetComponent<NetworkIdentity>() == connectionIdentity)
            {
                m_Teams[m_JoinIndex].Add(player);
                m_JoinIndex = (m_JoinIndex < m_Teams.Count - 1) ? m_JoinIndex + 1 : 0;
                return; 
            }
        }
    }

    public void OnPlayerLeave(NetworkIdentity connectionIdentity)
    {
        if (!isServer)
            return;
        foreach (var team in m_Teams)
        {
            foreach (var player in FindObjectsOfType<SimplePlayerBehaviour>())
            {
                if (player.GetComponent<NetworkIdentity>() == connectionIdentity)
                {
                    team.Remove(player);
                    m_JoinIndex = m_Teams.IndexOf(team);
                }
            }
        }
    }
}
