using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(menuName = "TeamScriptable")]
public class TeamSriptable : ScriptableObject
{
    public List<PlayerBehaviour> m_Players;
    public GameEventArgs m_OnAllPlayerLeftTeam;
    public int m_MaxPlayers;
    public Color m_Color;

    public GameObject m_HeavyTankPrefab;
    public GameObject m_LightTankPrefab;

    public TeamSriptable CreateInstance()
    {
        var tmp = Instantiate(this);
        tmp.m_OnAllPlayerLeftTeam = this.m_OnAllPlayerLeftTeam;
        tmp.m_HeavyTankPrefab = this.m_HeavyTankPrefab;
        tmp.m_LightTankPrefab = this.m_LightTankPrefab;    
        return tmp;
    }
    void OnEnable()
    {
        m_Players = new List<PlayerBehaviour>();
    }

    public Color Color
    {
        get { return m_Color; }
    }
    
    public void AddPlayer(PlayerBehaviour player)
    {
        if (!m_Players.Contains(player) && m_Players.Count < m_MaxPlayers && player != null)
        {
            m_Players.Add(player);
            player.m_TeamColor = m_Color;
        }
    }

    public void RemovePlayer(PlayerBehaviour player)
    {
        if (m_Players.Contains(player))
        {
            m_Players.Remove(player);            
        }
        if (m_Players.Count == 0)
            m_OnAllPlayerLeftTeam.Raise(this);
    }
}
