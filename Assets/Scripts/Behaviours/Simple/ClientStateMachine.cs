using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ClientStateMachine : MonoBehaviour, IContext
{
    public StateScriptable m_CurrentState;
    public List<StateScriptable> m_States = new List<StateScriptable>();    

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        m_CurrentState.OnEnter();
    }

    public void Transition(Object[] args)
    {
        var state = args[0] as StateScriptable;
        if (m_States.Contains(state))
        {
            m_CurrentState.OnExit();
            m_CurrentState = state;
            m_CurrentState.OnEnter();
        }
    }
}

public interface IContext
{
    void Transition(Object[] args);
}

public interface IState
{
    void OnEnter();
    void OnExit();
}
