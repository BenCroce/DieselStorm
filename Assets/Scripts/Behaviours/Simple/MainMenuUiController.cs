using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUiController : MonoBehaviour
{    
    public GameObject m_ActiveMenu;
    public GameObject m_GamePlay;
    public List<CameraAnchors> m_CameraAnchors;    
    public int m_CameraIndex;
    public bool m_IsGamePlay;
    public MainMenuUiController instance;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeMenu(GameObject menu)
    {
        m_ActiveMenu.SetActive(false);
        m_ActiveMenu = menu;
        m_ActiveMenu.SetActive(true);
    }

    public float m_SceneTransitionTimer;
    public float m_Timer;
    void Update()
    {
        if (!m_IsGamePlay)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= m_SceneTransitionTimer)
            {
                m_Timer = 0;
                var camera = Camera.main;
                int nextLookAt = Random.Range(0, m_CameraAnchors.Count);
                if (m_CameraAnchors[nextLookAt].m_CameraPosition != null ||
                    m_CameraAnchors[nextLookAt].m_CameraLookAt != null)
                {
                    camera.transform.position = m_CameraAnchors[nextLookAt].m_CameraPosition.position;
                    camera.transform.LookAt(m_CameraAnchors[nextLookAt].m_CameraLookAt);
                }                

            }
        }
    }

    public void EnableGamePlayUI(Object[] args)
    {        
        m_IsGamePlay = true;
        ChangeMenu(m_GamePlay);
        m_GamePlay.SetActive(false);
    }

    public void DisableGamePlayUI(Object[] args)
    {        
        m_IsGamePlay = false;
        m_GamePlay.SetActive(false);
        Destroy(this.gameObject);
    }

    public void ToggleGamePlayUI()
    {
        m_GamePlay.SetActive(!m_GamePlay.activeSelf);
        Cursor.lockState = (m_GamePlay.activeSelf) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = m_GamePlay.activeSelf;
    }
}

[System.Serializable]
public class CameraAnchors
{
    public Transform m_CameraPosition;
    public Transform m_CameraLookAt;
}