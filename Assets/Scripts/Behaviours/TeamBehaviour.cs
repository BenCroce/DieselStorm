using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamBehaviour : NetworkBehaviour
{
    public List<PlayerBehaviour> m_Players;
    public GameEventArgs m_OnPlayerAdded;
    [SerializeField]
    private Color m_Color;

    public NetworkLobbyManager m_LobbyManager;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public Color Color
    {
        get { return m_Color; }
    }

    [Server]
    public void AddPlayer(UnityEngine.Object[] args)
    {
        var go = args[2] as GameObject;
        go.AddComponent<PlayerBehaviour>();
        m_Players.Add(go.GetComponent<PlayerBehaviour>());
    }
}
