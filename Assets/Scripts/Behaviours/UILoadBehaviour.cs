using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadBehaviour : MonoBehaviour
{
    public UnityEngine.UI.Slider m_LoadingBar;

    void UpdateBar(Object[] args)
    {
        if (args[0].GetType() == typeof(LoadingBehaviour))
        {
            m_LoadingBar.value = (args[0] as LoadingBehaviour).m_PercentComplete;
        }
    }
}
