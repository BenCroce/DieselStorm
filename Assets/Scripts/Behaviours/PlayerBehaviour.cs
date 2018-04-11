using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerBehaviour : NetworkBehaviour
{
    [SyncVar] public string m_ScreenName;
    [SyncVar] public int m_Score;
    [SyncVar] public Color m_TeamColor;
    public int LobbyID;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetTeamColor(UnityEngine.Object[] obj)
    {
        var sender = obj[0] as TeamSriptable;
        var player = obj[1] as PlayerBehaviour;
       
        if (player == this)
        {                          
            m_TeamColor = (sender.Color == null) ?  Color.gray : sender.Color;
        }
    }
}

