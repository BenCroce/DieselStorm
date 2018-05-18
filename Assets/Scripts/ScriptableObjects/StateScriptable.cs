using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateScriptable : ScriptableObject, IState
{
    [SerializeField]
    protected GameEventArgs m_OnStateEntered;
    [SerializeField]
    protected GameEventArgs m_OnStateExit;

    public virtual void OnEnter()
    {
        m_OnStateEntered.Raise(this);
    }

    public virtual void Update()
    {
        
    }

    public virtual void OnExit()
    {
        m_OnStateExit.Raise(this);
    }
}
