using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


public class GameStateController : MonoBehaviour
{
    public StateScriptable m_CurrentState;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void TransitionToState(Object[] args)
    {
        var sender = args[0] as StateScriptable;
        var toState = args[1] as StateScriptable;        
        m_CurrentState = sender.TryTransition(toState) ? toState : m_CurrentState;
        if (m_CurrentState == toState)
        {
            sender.OnExit();
            m_CurrentState.OnEnter();            
        }
    }
}
