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
    public List<PlayerBehaviour> m_Players;
    public TeamSriptable m_RedTeam;
    public TeamSriptable m_BlueTeam;

    public void AddPlayer(PlayerBehaviour player)
    {
        if (!m_Players.Contains(player))
            m_Players.Add(player);
    }    

    public void BalanceTeams()
    {
        for(int i = 0; i < m_Players.Count; i++)
        {
            m_BlueTeam.AddPlayer(m_Players[i]);
            i++;
            if (i >= m_Players.Count)
                break;
            m_RedTeam.AddPlayer(m_Players[i]);
        }
    }
}
