using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class NewPlayerList : LobbyPlayerList
{
    public GameEventArgs m_OnPlayerConnected;

    public new void OnEnable()
    {
        base.OnEnable();
    }

    public new void PlayerListModified()
    {
        base.PlayerListModified();
        m_OnPlayerConnected.Raise(_players[_players.Count - 1]);
    }

}
