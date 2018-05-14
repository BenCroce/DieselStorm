using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;


public class ExtendNetworkMangerUI : MonoBehaviour
{
    public ExtendNetworkManager m_NetworkManager;

    public Button m_SearchForGames;
    public Button m_CreateNewGame;    
    public InputField m_CreateGameName;
    public ScrollRect m_ServerList;

    public GameObject m_ServerInfoPanel;    

    void Awake()
    {
        m_NetworkManager.StartMatchMaker();
        GetServers();
    }
    public void GetServers()
    {
        NetworkManager.singleton.matchMaker.ListMatches(0,10,"",true, 0,0, PopulateServerList);
    }

    public void PopulateServerList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {        
        foreach (var match in matches)
        {
            var contents = m_ServerList.GetComponent<ScrollRect>().content;
            var newServer = Instantiate(m_ServerInfoPanel, contents, false);
            var info = newServer.GetComponent<ServerInfoBehaviour>();
            info.m_MatchInfo = match;
            info.m_NumPlayers.text = match.currentSize + "/" + match.maxSize;
            info.m_ServerName.text = match.name;
        }
    }
    public void CreateGame()
    {
        NetworkManager.singleton.matchMaker.CreateMatch(m_CreateGameName.text,3,true, 
            "", "", "", 0, 0, OnMatchCreate);
    }

    public void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        NetworkManager.singleton.OnMatchCreate(success, extendedInfo, matchInfo);
    }
}
