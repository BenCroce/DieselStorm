using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamBehaviour : NetworkBehaviour
{
    public TeamSriptable m_TeamScriptable;
    [SyncVar]public Transform m_SpawnLocation;
    public GameEventArgs m_PlayerRespawn;
    public GameEventArgs m_OnTicketsRemaingChanged;
    public GameEventArgs m_OnTicketsDepleted;
    [SyncVar] public Color m_TeamColor;
    [SyncVar] public int m_TicketsRemaining;
    [SyncVar] public int m_MaxPlayers;
    [SyncVar] public GameObject m_HeavyTankPrefab;
    [SyncVar] public GameObject m_LightTankPrefab;


    void Start()
    {
        StartCoroutine(InitalSpawn());
        m_TeamColor = m_TeamScriptable.Color;
    }

    public void Setup(string newName)
    {        
        name = newName;
        m_TeamScriptable.name = newName;
        RpcSetup();
    }

    [ClientRpc]
    public void RpcSetup()
    {
        m_TeamColor = m_TeamScriptable.m_Color;
        m_TicketsRemaining = m_TeamScriptable.m_TicketsRemaining;
        m_MaxPlayers = m_TeamScriptable.m_MaxPlayers;
        m_HeavyTankPrefab = m_TeamScriptable.m_HeavyTankPrefab;
        m_LightTankPrefab = m_TeamScriptable.m_LightTankPrefab;
    }

    public void SpawnTank(Object[] args)
    {
        var senderGameObject = args[0] as PlayerBehaviour;
        if (m_TeamScriptable.m_Players.Contains(senderGameObject))
        {
            m_RespawningPlayer = senderGameObject;
            CmdSpawnTank();            
        }
    }

    public PlayerBehaviour m_RespawningPlayer;

    [ContextMenu("Force Spawn")]
    [Command]
    public void CmdSpawnTank()
    {        
        GameObject tank = Instantiate(m_TeamScriptable.m_HeavyTankPrefab);
        NetworkServer.Spawn(tank);
        m_PlayerRespawn.Raise(this, m_RespawningPlayer, m_SpawnLocation, tank);
        m_TeamScriptable.m_TicketsRemaining--;
        m_OnTicketsRemaingChanged.Raise(m_TeamScriptable);
        if(m_TeamScriptable.m_TicketsRemaining <= 0)
            m_OnTicketsDepleted.Raise(m_TeamScriptable);
        m_RespawningPlayer = null;
    }

    IEnumerator InitalSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            foreach (var player in m_TeamScriptable.m_Players)
            {
                GameObject tank = Instantiate(m_TeamScriptable.m_HeavyTankPrefab);
                var renders = tank.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var renderer in renders)
                {
                    var oldMaterial = renderer.material;
                    renderer.material = new Material(Shader.Find("Shader Forge/Tank_Shader"));
                    renderer.material.CopyPropertiesFromMaterial(oldMaterial);
                    renderer.material.SetColor("_ColorPicker", m_TeamScriptable.m_Color);
                }
                NetworkServer.Spawn(tank);
                m_PlayerRespawn.Raise(this, player, m_SpawnLocation, tank);                
            }
            break;
        }
    }

    public void SetSpawnPoint(Object[] args)
    {
        var sender = args[0] as SpawnPointController;
        if (sender != null)
        {
            m_SpawnLocation = args[1] as Transform;
        }
    }
}
