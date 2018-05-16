using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ClientStateMachine : MonoBehaviour, IContext
{
    public StateScriptable m_CurrentState;
    public List<StateScriptable> m_States = new List<StateScriptable>();
    private ClientStateMachine instance;
    void Awake()
    {
        if(instance)
        DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
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
            if(m_CurrentState.GetType() == typeof(GamePlayScriptable))
                Destroy(this.gameObject);
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
