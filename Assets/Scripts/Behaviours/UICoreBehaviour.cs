using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICoreBehaviour : MonoBehaviour
{
    public GameObject m_LoadScreen;
    [SerializeField]
    private bool m_LoadScreenState = true;
    public GameObject m_GameplayUI;
    [SerializeField]
    private bool m_GameplayUIState = false;
    public GameObject m_VictoryScreen;
    [SerializeField]
    private bool m_VictoryScreenState = false;

    public void ToggleLoadScreen(Object[] args)
    {
        m_LoadScreenState = !m_LoadScreenState;
        m_LoadScreen.SetActive(m_LoadScreenState);
    }

    public void ToggleGamePlayScreen(Object[] args)
    {
        m_GameplayUIState = !m_GameplayUIState;
        m_GameplayUI.SetActive(m_GameplayUIState);
    }

    public void ToggleVictoryScreen(Object[] args)
    {
        m_VictoryScreenState = !m_VictoryScreenState;
        m_VictoryScreen.SetActive(m_VictoryScreenState);
    }
}
