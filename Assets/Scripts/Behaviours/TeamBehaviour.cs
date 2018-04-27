using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamBehaviour : NetworkBehaviour
{
    public TeamSriptable m_TeamScriptable;
    public Transform m_SpawnLocation;
    public GameEventArgs m_PlayerRespawn;
    [SyncVar] public Color m_TeamColor;

    void Start()
    {
        StartCoroutine(InitalSpawn());
        var listener = GetComponent<GameEventArgsListener>();
        listener.Response.AddListener(SpawnTank);
        m_TeamColor = m_TeamScriptable.Color;
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
}
