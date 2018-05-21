using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUIBehaviour : NetworkBehaviour
{
    public GameObject m_TankSelection;
    public GameObject m_RespawningPlayer;

    void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }        
    
    public void PlayerDied()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;        
        m_TankSelection.SetActive(true);
    }
    
    public void TankSelect(GameObject prefab)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_TankSelection.SetActive(false);
        m_RespawningPlayer.GetComponent<SimplePlayerBehaviour>().m_TankObjectPrefab = prefab;
        m_RespawningPlayer.GetComponent<SimplePlayerBehaviour>().CmdSelectNewTank(); 
    }    
}
 