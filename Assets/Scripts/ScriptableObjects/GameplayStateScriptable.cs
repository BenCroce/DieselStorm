using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameplayStateScriptable : StateScriptable
{
    public override void OnEnter()
    {
        m_OnStateEntered.Raise(this);
    }

    public override void OnExit()
    {
        m_OnStateExit.Raise(this);
    }
}
