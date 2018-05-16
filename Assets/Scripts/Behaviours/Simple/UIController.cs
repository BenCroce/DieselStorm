using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject m_ActiveWindow;
    public GameObject m_MainMenu;
    public GameObject m_CreditsMenu;
    public GameObject m_ServerMenu;
    public GameObject m_GameMenu;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void TransitionMenu(GameObject menu)
    {
        menu.SetActive(true);
        m_ActiveWindow.SetActive(false);
        m_ActiveWindow = menu;
    }

    void Update()
    {

    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
