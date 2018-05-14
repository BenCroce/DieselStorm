using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject m_ActiveWindow;
    public GameObject m_MainMenu;
    public GameObject m_CreditsMenu;
    public GameObject m_ServerMenu;

    public void TransitionMenu(GameObject menu)
    {
        menu.SetActive(true);
        m_ActiveWindow.SetActive(false);
        m_ActiveWindow = menu;
    }    
}
