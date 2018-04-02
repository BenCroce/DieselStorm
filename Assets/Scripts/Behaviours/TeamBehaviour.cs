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

    public Color Color
    {
        get { return m_Color; }
    }

    public void AddPlayer(PlayerBehaviour player)
    {
        if(m_Players.Contains(player))
            return;
        m_Players.Add(player);
        m_OnPlayerAdded.Raise(this, player);        
    }
}
