using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerBehaviour : NetworkBehaviour
{
    [SyncVar] public string m_ScreenName;
    [SyncVar] public int m_Score;
    [SyncVar] public Color m_TeamColor;

    public GameObject m_SceneObject;

    public int LobbyID;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void ReSpawn(Object[] args)
    {
        var sender = args[0] as TeamBehaviour;
        var obj = args[1] as PlayerBehaviour;
        var location = args[2] as Transform;

        if (obj == this)
        {
            m_SceneObject.transform.position = location.position;
            m_SceneObject.SetActive(true);
        }
    }    
}

