using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStats : MonoBehaviour
{
    public GameEventArgs m_ProjectileHit;
    public ModifierScriptable m_ArmorModifier;
    public ModifierScriptable m_HealthModifier;

    void OnTriggerEnter(Collider other)
    {
        m_ProjectileHit.Raise(this.gameObject, m_HealthModifier, m_ArmorModifier, other.gameObject);
        //Destroy(this.gameObject);
    }
}
