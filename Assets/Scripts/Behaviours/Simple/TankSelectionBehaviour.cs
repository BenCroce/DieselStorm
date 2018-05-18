using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSelectionBehaviour : MonoBehaviour
{
    public GameObject m_TankSelection;
    public GameEventArgs m_OnTankSelected;

    public void PlayerDied(Object[] args)
    {
        var player = args[0] as SimplePlayerBehaviour;
        if (player == GetComponentInParent<SimplePlayerBehaviour>())
        {
            m_TankSelection.SetActive(true);
        }
    }

    public void TankSelect(GameObject prefab)
    {
        m_OnTankSelected.Raise(GetComponentInParent<SimplePlayerBehaviour>(), prefab);
    }
}
