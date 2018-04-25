using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class TeamSetupBehaviour : NetworkBehaviour
{
    public TeamSetupSingleton m_TeamSetupSingleton;        
    
    private void Start()
    {
        if (!isServer)
            return;
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
}
