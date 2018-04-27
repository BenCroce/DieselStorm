using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerBehaviour : NetworkBehaviour
{
    [SyncVar] public string m_ScreenName;
    [SyncVar] public int m_Score;
    [SyncVar] public Color m_TeamColor;

    [SyncVar]
    public GameObject m_SceneObject;

    public int LobbyID;

    public GameEventArgs m_PlayerObjectDestroyed;
    public GameEventArgs m_OnPlayerConnected;
    public Material m_SceneObjectMaterial;    

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void ReSpawn(Object[] args)
    {
        var sender = args[0] as TeamBehaviour;
        var behaviour = args[1] as PlayerBehaviour;
        var location = args[2] as Transform;

        if (behaviour.gameObject == this.gameObject)
        {
            m_SceneObject = args[3] as GameObject;
            m_TeamColor = sender.m_TeamColor;
            m_OnPlayerConnected.Raise(this.gameObject, m_SceneObject);
            m_SceneObject.transform.position = new Vector3(2450, 580, 1690) + 
                new Vector3(Random.Range(0,25),0, Random.Range(0,25));
            if(!m_SceneObject.GetComponent<NetworkIdentity>().hasAuthority)            
                StartCoroutine(RPCCall());
        }
    }

    [ClientRpc]
    void RpcSetColor(Color c, GameObject obj)
    {
        m_SceneObject = obj;
        var renders = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var renderer in renders)
        {
            var oldMaterial = renderer.material;
            m_SceneObjectMaterial = new Material(Shader.Find("Shader Forge/Tank_Shader"));
            m_SceneObjectMaterial.CopyPropertiesFromMaterial(oldMaterial);            
            m_SceneObjectMaterial.SetColor("_ColorPicker", m_TeamColor);
            renderer.material = m_SceneObjectMaterial;
        }
    }

    [Command]
    void CmdSetMeshColors(Color c)
    {
        m_TeamColor = c;
    }

    [ClientRpc]
    public void RpcAssignClientAuthority(NetworkIdentity local, NetworkIdentity id)
    {
        if (local.isLocalPlayer)
        {
            CmdAssignAuthority(local, id);            
        }        
    }

    [Command]
    void CmdAssignAuthority(NetworkIdentity local, NetworkIdentity id)
    {
        var connection = local.connectionToClient;        
        id.AssignClientAuthority(connection);        
    }

    IEnumerator RPCCall()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            CmdSetMeshColors(m_TeamColor);
            RpcSetColor(m_TeamColor, m_SceneObject);
            RpcAssignClientAuthority(GetComponent<NetworkIdentity>(), 
                  m_SceneObject.GetComponent<NetworkIdentity>());
            break;
        }
    }

    public void SceneObjectDestroyed(Object[] args)
    {
        var senderGameObject = args[0] as TankStats;
        if(senderGameObject.gameObject == m_SceneObject)
            m_PlayerObjectDestroyed.Raise(this);
    }
}
