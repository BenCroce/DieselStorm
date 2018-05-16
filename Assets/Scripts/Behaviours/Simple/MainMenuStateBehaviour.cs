using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStateBehaviour : MonoBehaviour
{
    public GameEventArgs m_OnMainMenuLoaded;
    public StateScriptable m_MainMenuState;

    void Awake()
    {
        m_OnMainMenuLoaded.Raise(m_MainMenuState);
    }
}
