using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;


public class GameStateController : MonoBehaviour
{
    public List<StateScriptable> m_States;
    public StateScriptable m_CurrentState;    

    void Awake()
    {
        if(m_CurrentState == null)
            Debug.LogError("No initial state set");
        m_CurrentState.OnEnter();

        SceneManager.sceneLoaded += SceneLoaded;
    }

    void TransitionToState(Object[] args)
    {
        var sender = args[0] as StateScriptable;
        var toState = args[1] as StateScriptable;
        if(m_CurrentState != sender)
            return;        
        m_CurrentState = sender.TryTransition(toState) ? toState : m_CurrentState;
        if (m_CurrentState == toState)
        {
            sender.OnExit();
            m_CurrentState.OnEnter();            
        }
    }    
    
    public void SceneLoaded(Scene cur, LoadSceneMode mode)
    {
        if (cur.name == "1.Game-Play")
            TransitionToState(new Object[] { m_CurrentState, m_States[3] });
        if (cur.name == "0.lobby")
            TransitionToState(new Object[] { m_CurrentState, m_States[1] });
    }

    [ContextMenu("ToGame")]
    public void TransitionToGame()
    {
        TransitionToState(new Object[] { m_CurrentState, m_States[0] });
    }

    [ContextMenu("ToEnd")]
    public void TransitionToEnd()
    {
        TransitionToState(new Object[] { m_CurrentState, m_States[2] });
    }

    [ContextMenu("ToLobby")]
    public void TransitionToLobby()
    {
        TransitionToState(new Object[] { m_CurrentState, m_States[1] });
    }
}
