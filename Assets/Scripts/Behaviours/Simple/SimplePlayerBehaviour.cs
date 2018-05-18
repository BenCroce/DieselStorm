using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(channel = Channels.DefaultReliable, sendInterval = 0.05f)]
public class SimplePlayerBehaviour : NetworkBehaviour
{
    public GameObject m_TankObjectPrefab;
    [SyncVar]public GameObject m_rtTankObject;
    [SyncVar]public Color m_TeamColor;
    public Material m_TankMaterial;    
    [SyncVar] public float m_RespawnDelay;
    public GameEventArgs m_OnSceneTankDestroyed;
    public SimpleTeamBehaviour m_Team;
    public PlayerUIBehaviour m_PlayerUI;

    [ClientRpc]
    public void RpcSetTeamColor(GameObject tank, Color col)
    {        
        var renders = tank.GetComponentsInChildren<SkinnedMeshRenderer>();        
        foreach (var renderer in renders)
        {
            var oldMaterial = renderer.material;
            m_TankMaterial = new Material(Shader.Find("Shader Forge/Tank_Shader"));
            m_TankMaterial.CopyPropertiesFromMaterial(oldMaterial);
            m_TankMaterial.SetColor("_ColorPicker", col);
            renderer.material = m_TankMaterial;
        }
    }

    [Command]
    public void CmdSetTeamColor(Color col)
    {
        m_TeamColor = col;
    }

    public void SpawnNewTank(Object[] args)
    {        
        if(args[0] as SimplePlayerBehaviour != this)
            return;
        m_Team = args[1] as SimpleTeamBehaviour;      
        m_PlayerUI.PlayerDied();          
    }

    [ClientRpc]
    public void RpcSelectNewTank(GameObject prefab)
    {
        m_TankObjectPrefab = prefab;
        StartCoroutine(RespawnDelay());
    }

    [ClientRpc]
    void RpcSpawnTank()
    {
        CmdSpawnNewTank();        
    }

    [Command]
    public void CmdSpawnNewTank()
    {
        var connection = GetComponent<NetworkIdentity>().connectionToClient;
        m_rtTankObject.GetComponent<NetworkIdentity>().AssignClientAuthority(connection);
    }

    IEnumerator RespawnDelay()
    {        
        yield return new WaitForSeconds(m_RespawnDelay);
        var spawnPosition = FindObjectsOfType<NetworkStartPosition>();
        int numSpawns = spawnPosition.Length;
        int spawnIndex = Random.Range(0, numSpawns);
        var newTank = Instantiate(m_TankObjectPrefab,
            spawnPosition[spawnIndex].transform.position,
            Quaternion.identity);
        GetComponent<NetworkTank>().m_player = this;
        NetworkServer.Spawn(newTank);
        m_rtTankObject = newTank;
        RpcSetTeamColor(m_rtTankObject, m_TeamColor);
        RpcSpawnTank();
    }

    public void SceneObjectDestroyed(Object[] args)
    {
        var sender = args[0] as TankStats;        
        if (sender.gameObject == m_rtTankObject)
            m_Team.PlayerDied(new Object[] {this});
    }    

    [Command]
    public void CmdSetTankPrefab(GameObject tank)
    {
        m_TankObjectPrefab = tank;        
    }
}
