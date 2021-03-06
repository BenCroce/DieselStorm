﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerUIController : NetworkBehaviour
{
    public GameObject m_MathEndPanel;
    public GameObject m_TankSelection;
    public Text m_MatchEndText;
    public string m_Textdata;
    public Color m_TextColor;    

    [ClientRpc]
    public void RpcDisplayMatchEndUI(bool status)
    {
        m_MathEndPanel.SetActive(status);
    }
}
