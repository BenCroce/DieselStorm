using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prototype.NetworkLobby;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;

public class TeamSetupBehaviour : NetworkBehaviour
{
    public TeamSetupSingleton m_TeamSetupSingleton;
    public TeamSriptable m_TeamConfig;
    public GameObject m_TeamObject;
    
    private void Start()
    {
        if (!isServer)
            return;
        CreateTeams();
        StartCoroutine(GetPlayers());
    }

    public IEnumerator GetPlayers()
    {
        while(true)
        {
            yield return new WaitForSeconds(2.0f);
            foreach (var player in FindObjectsOfType<PlayerBehaviour>())
            {
                m_TeamSetupSingleton.AddPlayer(player);
            }
            m_TeamSetupSingleton.BalanceTeams();
            break;
        }
    }

    void CreateTeams()
    {
        for (int i = 0; i < m_TeamSetupSingleton.m_MaxTeams; i++)
        {
            var newTeam = m_TeamConfig.CreateInstance();
            if (m_TeamSetupSingleton.AddTeam(newTeam))
            {
                var teamObj = Instantiate(m_TeamObject);
                NetworkServer.Spawn(teamObj);
                teamObj.GetComponent<TeamBehaviour>().m_TeamScriptable = newTeam;
                teamObj.name = newTeam.name;                
            }
        }
    }
}
