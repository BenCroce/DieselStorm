using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MatchLoadStateScriptable : StateScriptable
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
