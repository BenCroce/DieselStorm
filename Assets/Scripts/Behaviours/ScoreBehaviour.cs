﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreBehaviour : NetworkBehaviour
{
    public List<TeamSriptable> m_Teams;    
    public GameEventArgs m_OnTeamsSorted;
    public GameEventArgs m_OnWinCondition;

    void Start()
    {
        StartCoroutine(GetTeams());
    }

    public void SortTeams(Object []args)
    {
        if (args[0].GetType() == typeof(TeamSriptable))
        {
            var sortedList = m_Teams.OrderBy(order => order.m_TicketsRemaining).ToList();
            m_Teams = sortedList;
            m_OnTeamsSorted.Raise(this);
            Debug.Log("Sorting Teams");
        }
    }

    public void CheckForVictory(Object[] args)
    {
        if (args[0].GetType() == typeof(TeamSriptable))
        {
            Debug.Log("Checking For Victory");
            int teamsWithTickets = 0;
            foreach (var team in m_Teams)
            {
                if (team.m_TicketsRemaining > 0)
                {
                    teamsWithTickets++;
                    if (teamsWithTickets > 1)
                        return;
                }
            }
            if (teamsWithTickets == 1)
            {
                m_OnWinCondition.Raise(this);
                Debug.Log("Victory Met");
            }
        }
    }

    IEnumerator GetTeams()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            foreach (var teamBehaviour in FindObjectsOfType<TeamBehaviour>())
            {
                m_Teams.Add(teamBehaviour.m_TeamScriptable);
            }
            break;
        }
    }
}
