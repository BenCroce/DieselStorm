using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class SimpleScoreBehaviour : NetworkBehaviour
{
    public TeamController m_TeamController;
    public List<SimpleTeamBehaviour> m_TeamsDefeated;
    [SyncVar]public int m_TeamRemaining;
    [SyncVar]public bool m_OnlyOneTeamRemaining;

    void Awake()
    {
        if (m_TeamController == null)
        {
            Debug.LogWarning("Team controller is null");
        }
        m_TeamRemaining = m_TeamController.m_Teams.Count;
    }

    void Update()
    {        
        foreach (var team in m_TeamController.m_Teams)
        {
            if (team.m_CurrentTeamLives <= 0 || team.m_Players.Count == 0)
                TryAddDefeatedTeam(team);
            if (team.m_PlayerOnTeam && 
                m_TeamsDefeated.Contains(team) &&
                team.m_CurrentTeamLives > 0)
            {
                m_TeamsDefeated.Remove(team);
                m_TeamRemaining++;
            }
        }
        m_OnlyOneTeamRemaining = m_TeamRemaining == 1;
    }

    void TryAddDefeatedTeam(SimpleTeamBehaviour team)
    {
        if(m_TeamsDefeated.Contains(team))
            return;
        m_TeamsDefeated.Add(team);
        m_TeamRemaining--;
    }    
}
