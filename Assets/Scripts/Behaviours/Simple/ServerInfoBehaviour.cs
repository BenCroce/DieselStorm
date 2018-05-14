using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public class ServerInfoBehaviour : MonoBehaviour
{    
    public Text m_ServerName;
    public Text m_NumPlayers;
    public MatchInfoSnapshot m_MatchInfo;

    public void JoinMatch()
    {
        NetworkManager.singleton.matchMaker.JoinMatch(m_MatchInfo.networkId, "", "", "", 0, 0, OnMatchJoined);
    }

    public void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        NetworkManager.singleton.OnMatchJoined(success, extendedInfo, matchInfo);
    }
}
