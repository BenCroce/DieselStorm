using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExtendNetworkManager : NetworkManager
{
    public GameEventArgs m_OnClientConnected;
    public GameEventArgs m_OnClientDisconnected;
    public TeamController m_TeamController;
    public Color m_ClientTeamColor;
    public int m_connectionId;
    public Dictionary<string,NetworkIdentity> m_Connections = new Dictionary<string, NetworkIdentity>();

    public int m_PlayerConnected = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<NetworkManagerHUD>().showGUI = !GetComponent<NetworkManagerHUD>().showGUI;
            Cursor.visible = GetComponent<NetworkManagerHUD>().showGUI;
            if (Cursor.visible)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.None;
        }
        if (m_TeamController != null)
        {
            foreach (var team in m_TeamController.m_Teams)
            {
                foreach (var player in team.m_Players)
                {
                    if (player.GetComponent<NetworkIdentity>().connectionToClient.connectionId == m_connectionId)
                    {
                        m_ClientTeamColor = player.m_TeamColor;
                    }
                }
            }
        }
    }

    public override void OnClientConnect(NetworkConnection connection)
    {
        m_connectionId = connection.connectionId;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GetComponent<NetworkManagerHUD>().showGUI = false;
    }

    //Detect when a client connects to the Server
    public override void OnServerConnect(NetworkConnection connection)
    {
        base.OnServerConnect(connection);
        if (m_TeamController == null)
            StartCoroutine(SearchForController());
        StartCoroutine(ClientConnect(connection));
        m_connectionId = connection.connectionId;
        GetComponent<NetworkManagerHUD>().showGUI = false;
    }

    public override void OnClientDisconnect(NetworkConnection connection)
    {
        GetComponent<NetworkManagerHUD>().showGUI = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        base.OnClientDisconnect(connection);
    }

    //Detect when a client connects to the Server
    public override void OnServerDisconnect(NetworkConnection connection)
    {        
        foreach (var con in m_Connections)
        {
            if (con.Value.connectionToClient.connectionId == connection.connectionId)
            {                
                m_TeamController.OnPlayerLeave(con.Value);
                m_PlayerConnected--;                
                m_Connections.Remove(con.Key);
            }
        }
        GetComponent<NetworkManagerHUD>().showGUI = true;
        base.OnServerDisconnect(connection);
    }

    IEnumerator SearchForController()
    {
        yield return new WaitForSeconds(1.0f);
        m_TeamController = FindObjectOfType<TeamController>();
    }

    IEnumerator ClientConnect(NetworkConnection connection)
    {
        yield return new WaitForSeconds(1.0f);
        var netIDs = FindObjectsOfType<NetworkIdentity>();
        foreach (var id in netIDs)
        {
            if (id.connectionToClient == null)
                break;
            if (id.connectionToClient.connectionId == connection.connectionId)
            {
                m_TeamController.OnPlayerJoined(id);
                m_PlayerConnected++;
                m_Connections.Add("Connection::" + m_PlayerConnected, id);
            }
        }
        StopCoroutine(ClientConnect(connection));
    }
}
