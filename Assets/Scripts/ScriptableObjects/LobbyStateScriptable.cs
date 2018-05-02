using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LobbyStateScriptable : StateScriptable
{            
    public override void OnEnter()
    {
        m_OnStateEntered.Raise(this);
        ClearPlayers();
    }

    public override void OnExit()
    {
        m_OnStateExit.Raise(this);
    }

    void ClearPlayers()
    {
        var players = FindObjectsOfType<PlayerBehaviour>();
        for (int i = 0; i < players.Length; i++)
        {
            Destroy(players[i].gameObject);
        }
    }
}
