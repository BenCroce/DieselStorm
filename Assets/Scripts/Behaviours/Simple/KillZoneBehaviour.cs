using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZoneBehaviour : MonoBehaviour
{
    public ModifierScriptable m_HealthModifier;
    public GameEventArgs m_Collision;

    void OnTriggerEnter(Collider other)
    {
        m_Collision.Raise(this.gameObject, m_HealthModifier, other.gameObject);
    }
}
