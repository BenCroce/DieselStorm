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

        if (behaviour == this)
        {
            m_TeamColor = sender.m_TeamScriptable.m_Color;
            m_SceneObject = args[3] as GameObject;            
            m_OnPlayerConnected.Raise(this.gameObject, m_SceneObject);
            m_SceneObject.transform.position = new Vector3(2450, 580, 1690) + 
                new Vector3(Random.Range(0,25),0, Random.Range(0,25));
            if(!m_SceneObject.GetComponent<NetworkIdentity>().hasAuthority)            
                StartCoroutine(RPCCall());
        }
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

        var renders = m_SceneObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        m_SceneObjectMaterial.color = m_TeamColor;
        foreach (var renderer in renders)
        {
            if (renderer.material.shader != m_SceneObjectMaterial.shader)
                break;
            renderer.material = new Material(Shader.Find("Shader Forge/Tank_Shader"));
            renderer.material.CopyPropertiesFromMaterial(m_SceneObjectMaterial);
            renderer.material.SetColor("_ColorPicker", m_TeamColor);
        }

        m_SceneObject.GetComponent<NetworkTankInputController>().m_vcam.SetActive(true);
    }

    IEnumerator RPCCall()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
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
