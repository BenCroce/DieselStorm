using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayStateBehaviour : MonoBehaviour
{
    public GameEventArgs OnGameSceneLoaded;    
    public StateScriptable m_GamePlayState;
	void Start ()
    {
        OnGameSceneLoaded.Raise(m_GamePlayState);
	}

    void Update()
    {
        m_GamePlayState.Update();
    }
}
