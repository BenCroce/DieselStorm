using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUIBehaviour : NetworkBehaviour
{
    public GameObject m_TankSelection;    

    void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void RpcPlayerDied()
    {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;            
            m_TankSelection.SetActive(true);        
    }
    
    public void RpcTankSelect()
    {                        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_TankSelection.SetActive(false);
    }    
}
 