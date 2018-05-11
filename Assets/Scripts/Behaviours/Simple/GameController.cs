using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour
{
    public SimpleScoreBehaviour m_ScoreKeeper;
    public List<SimpleTeamBehaviour> m_Teams;
    public SimpleUIController m_UIController;
    [SyncVar]public bool m_AtLeastTwoFilledTeams;

    [Tooltip("timer for players to join game before match termination")]
    [SyncVar]public float m_GameTimerOut;

    [SerializeField]
    [SyncVar]private float m_Timer;

    [SyncVar]public bool m_GameOver;


    void Awake()
    {
        if (m_GameTimerOut <= 0)
            m_GameTimerOut = 10;
    }

    void Update()
    {
        if (m_GameOver)
        {
            var tanks = FindObjectsOfType<NetworkTankInputController>();
            foreach (var tank in tanks)
            {
                tank.enabled = false;
                tank.GetComponent<TurrentAimBehaviour>().enabled = false;
            }

            //if (m_GameTimer >= m_GameOverTimer)
            //{
            //    foreach (var team in m_Teams)
            //    {
            //        foreach (var player in team.m_Players)
            //        {
            //            player.GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
            //        }
            //    }
            //    NetworkManager.Shutdown();
            //}
            return;            
        }
        int numFilledTeams = 0;
        foreach (var team in m_ScoreKeeper.m_TeamController.m_Teams)
        {
            if (team.m_PlayerOnTeam)
                numFilledTeams++;
        }        
        m_AtLeastTwoFilledTeams = numFilledTeams > 1;

        if (!m_AtLeastTwoFilledTeams)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= m_GameTimerOut)
            {
                m_GameOver = true;
                foreach (var team in m_Teams)
                {
                    foreach (var player in team.m_Players)
                    {
                        if (player.isLocalPlayer)
                        {
                            m_UIController.RpcDisplayMatchEndUI(true);
                        }
                    }
                }
            }
            return;
        }  
        RpcVictoryCheck();
    }

    [ClientRpc]
    void RpcVictoryCheck()
    {
        StartCoroutine(VictoryCheckDelay());
    }

    IEnumerator VictoryCheckDelay()
    {
        yield return new WaitForSeconds(2.5f);
        if (m_ScoreKeeper.m_TeamRemaining <= 1)
        {
            m_GameOver = true;
            foreach (var team in m_ScoreKeeper.m_TeamController.m_Teams)
            {
                m_UIController.RpcDisplayMatchEndUI(true);                
            }            
        }
    }
}
