using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class GamePlayScriptable : StateScriptable
{
    public GameEventArgs OnPauseToggle;
    public override void OnEnter()
    {
        m_OnStateEntered.Raise(this);                
    }

    public override void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            OnPauseToggle.Raise();
    }

    public override void OnExit()
    {
        m_OnStateExit.Raise(this);        
    }
}
