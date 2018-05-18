using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUiController : MonoBehaviour
{    
    public GameObject m_ActiveMenu;
    public GameObject m_GamePlay;
    public List<CameraAnchors> m_CameraAnchors;    
    public int m_CameraIndex;
    public bool m_IsGamePlay;

    public Slider m_Horizontal;
    public Slider m_Vertical;
    public InputField m_HorizontalInput;
    public InputField m_VerticalInput;

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        float a = PlayerPrefs.GetFloat("VerticalSensitivity", 0.5f);
        m_HorizontalInput.text = PlayerPrefs.GetFloat("HorizontalSensitivity", 200).ToString();
        m_VerticalInput.text = a.ToString();
        m_Horizontal.value = float.Parse(m_HorizontalInput.text);
        m_Vertical.value = a;
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
    }

    public void ToggleGamePlayUI()
    {
        m_GamePlay.SetActive(!m_GamePlay.activeSelf);
        Cursor.lockState = (m_GamePlay.activeSelf) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = m_GamePlay.activeSelf;
    }

    public void UpdateHorizontalSlider()
    {
        PlayerPrefs.SetFloat("HorizontalSensitivity", (float)m_Horizontal.value);
        PlayerPrefs.Save();
        m_HorizontalInput.text = m_Horizontal.value.ToString();
    }

    public void UpdateVerticalSlider()
    {
        PlayerPrefs.SetFloat("VerticalSensitivity", (float)m_Vertical.value);
        PlayerPrefs.Save();
        m_VerticalInput.text = m_Vertical.value.ToString();
    }

    public void UpdateHorizontalInput()
    {
        PlayerPrefs.SetFloat("HorizontalSensitivity", float.Parse(m_HorizontalInput.text));
        PlayerPrefs.Save();
        m_Horizontal.value = PlayerPrefs.GetFloat("HorizontalSensitivity");
    }

    public void UpdateVerticalInput()
    {
        PlayerPrefs.SetFloat("VerticalSensitivity", float.Parse(m_VerticalInput.text));
        PlayerPrefs.Save();
        m_Vertical.value = PlayerPrefs.GetFloat("VerticalSensitivity");
    }
}

[System.Serializable]
public class CameraAnchors
{
    public Transform m_CameraPosition;
    public Transform m_CameraLookAt;
}