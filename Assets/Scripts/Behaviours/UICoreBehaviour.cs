using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICoreBehaviour : MonoBehaviour
{
    public GameObject m_LoadScreen;

    public GameObject m_GameplayUI;

    public GameObject m_VictoryScreen;


    public void ToggleLoadScreenOn(Object[] args)
    {
        m_LoadScreen.SetActive(true);
    }

    public void ToggleLoadScreenOff(Object[] args)
    {
        m_LoadScreen.SetActive(false);
    }


    public void ToggleGamePlayScreenOn(Object[] args)
    {
        m_GameplayUI.SetActive(true);
    }

    public void ToggleGamePlayScreenOff(Object[] args)
    {
        m_GameplayUI.SetActive(false);
    }

    public void ToggleVictoryScreenOn(Object[] args)
    {
        m_VictoryScreen.SetActive(true);
    }

    public void ToggleVictoryScreenOff(Object[] args)
    {
        m_VictoryScreen.SetActive(false);
    }
}
