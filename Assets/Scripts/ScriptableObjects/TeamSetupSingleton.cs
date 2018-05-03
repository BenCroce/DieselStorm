﻿using System.Collections;
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
    public List<TeamSriptable> m_Teams;
    public List<Color> m_TeamColors;
    public TeamSriptable m_DefaultTeamScriptable;
    public int m_TeamStartingTickets;
    public int m_MaxTeams;

    void OnEnable()
    {
        m_Players = new List<PlayerBehaviour>();
        m_Teams = new List<TeamSriptable>();
        m_TeamColors = new List<Color>();
        if (m_TeamStartingTickets <= 0)
            m_TeamStartingTickets = 1;
    }


    public void AddPlayer(PlayerBehaviour player)
    {
        if (!m_Players.Contains(player))
            m_Players.Add(player);
    }

    public bool AddTeam(TeamSriptable team)
    {
        if (m_Teams.Count < m_MaxTeams && !m_Teams.Contains(team))
        {            
            m_Teams.Add(team);
            team.m_TicketsRemaining = m_TeamStartingTickets;
            var teamColor = Random.ColorHSV();
            while (m_TeamColors.Contains(teamColor))
            {
                teamColor = Random.ColorHSV();
            }
            team.m_Color = teamColor;
            m_TeamColors.Add(teamColor);
            return true;
        }
        return false;
    }

    public void ClearTeams()
    {
        m_Teams = new List<TeamSriptable>();
        m_TeamColors = new List<Color>();
        m_Players = new List<PlayerBehaviour>();
    }

    public void BalanceTeams()
    {
        int teamCount = 0;
        for(int i = 0; i < m_Players.Count; i++)
        {
            m_Teams[teamCount].AddPlayer(m_Players[i]);
            teamCount++;
            if (teamCount >= m_Teams.Count)
                teamCount = 0;
        }
    }
}