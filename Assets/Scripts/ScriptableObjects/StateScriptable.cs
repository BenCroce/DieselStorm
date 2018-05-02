using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateScriptable : ScriptableObject
{
    public List<StateScriptable> Transitions;

    public GameEventArgs m_OnStateEntered;
    public GameEventArgs m_OnStateExit;

    public bool TryTransition(StateScriptable state)
    {
        return Transitions.Contains(state);
    }

    public abstract void OnEnter();

    public abstract void OnExit();
}
