using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;


public class ExtendNetworkMangerUI : MonoBehaviour
{
    public Button m_SearchForGames;
    public Button m_CreateNewGame;    
    public InputField m_CreateGameName;
    public ScrollRect m_ServerList;

    public GameObject m_ServerInfoPanel;

    public List<GameObject> m_ServerPanels;

    void Awake()
    {        
        GetServers();
    }
    public void GetServers()
    {
        if (NetworkManager.singleton.matchMaker == null)
            NetworkManager.singleton.StartMatchMaker();
        NetworkManager.singleton.matchMaker.ListMatches(0,10,"",true, 0,0, PopulateServerList);
    }

    public void PopulateServerList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        for (int i = 0; i < m_ServerPanels.Count; i++)
        {
            var panel = m_ServerPanels[i];
            m_ServerPanels.RemoveAt(i);
            Destroy(panel);
        }
             
        foreach (var match in matches)
        {
            var contents = m_ServerList.content;
            var newServer = Instantiate(m_ServerInfoPanel);
            newServer.transform.SetParent(contents, false);            
            var info = newServer.GetComponent<ServerInfoBehaviour>();
            info.PopulateData(match);
            m_ServerPanels.Add(newServer);
        }
    }
    public void CreateGame()
    {
        if(NetworkManager.singleton.matchMaker == null)
            NetworkManager.singleton.StartMatchMaker();
        NetworkManager.singleton.matchMaker.CreateMatch(m_CreateGameName.text,3,true, 
            "", "", "", 0, 0, NetworkManager.singleton.OnMatchCreate);
    }
}
