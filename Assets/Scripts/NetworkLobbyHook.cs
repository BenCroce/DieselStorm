using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    public GameEventArgs m_OnPlayerConnected;    

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager,
        GameObject lobbyPlayer, GameObject gamePlayer)
    {
        m_OnPlayerConnected.Raise(this, lobbyPlayer, gamePlayer);
    }
}
