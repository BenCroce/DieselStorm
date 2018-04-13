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

    [Command]
    void CmdAssignClientAuthority(NetworkIdentity id)
    {
        var newid = m_SceneObject.GetComponent<NetworkIdentity>();
        var connection = newid.connectionToClient;
        id.AssignClientAuthority(connection);
    }

    public void ReSpawn(Object[] args)
    {
        var sender = args[0] as TeamBehaviour;
        var behaviour = args[1] as PlayerBehaviour;
        var location = args[2] as Transform;
        m_SceneObject = args[3] as GameObject;

        if (behaviour == this)
        {
            m_SceneObject.SetActive(true);
            CmdAssignClientAuthority(m_SceneObject.GetComponent<NetworkIdentity>());
            m_SceneObject.transform.position = location.position;
            m_SceneObject.transform.SetParent( this.transform);
           
        }
    }    
}

