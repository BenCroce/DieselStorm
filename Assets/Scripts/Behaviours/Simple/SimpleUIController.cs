using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SimpleUIController : NetworkBehaviour
{
    public GameObject m_MathEndPanel;
    public GameObject m_TankSelection;
    public Text m_MatchEndText;
    public string m_Textdata;
    public Color m_TextColor;
    public SimplePlayerBehaviour m_RespawningPlayer;    

    [ClientRpc]
    public void RpcDisplayMatchEndUI(bool status)
    {
        m_MathEndPanel.SetActive(status);
    }

    public void OnPlayerDied(Object[] args)
    {
        m_TankSelection.SetActive(true);
    }

    public void TankSelected(GameObject tankPrefab)
    {
        m_RespawningPlayer.m_TankObjectPrefab = tankPrefab;
    }
}
