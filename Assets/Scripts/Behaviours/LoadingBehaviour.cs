using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;

public class LoadingBehaviour : MonoBehaviour
{
    public float m_PercentComplete;
    public GameEventArgs m_OnLoadPercentChange;
    public GameEventArgs m_LoadComplete;

    public void StartLoad(Object[] args)
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {        
        var numConnected = FindObjectOfType<LobbyManager>().numPlayers;
        var playerLoaded = FindObjectsOfType<NetworkTankInputController>().Length;
        while (playerLoaded < numConnected)
        {
            yield return new WaitForSeconds(0.2f);
            if (FindObjectsOfType<NetworkTankInputController>().Length != 0)
                m_PercentComplete = (playerLoaded / FindObjectsOfType<NetworkTankInputController>().Length) * 100;
            playerLoaded = FindObjectsOfType<NetworkTankInputController>().Length;   
            m_OnLoadPercentChange.Raise(this);         
        }
        yield return new WaitForSeconds(2.0f);
        m_LoadComplete.Raise(this);        
    }
}
