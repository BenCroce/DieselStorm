using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContext
{
    void Transition(IState state);
}

public interface IState
{
    void OnEnter();
    void OnExit();
}

public class ClientStateMachine : MonoBehaviour
{
    public IState m_CurrentState;
}
