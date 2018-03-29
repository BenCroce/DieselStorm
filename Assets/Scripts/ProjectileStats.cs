using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStats : MonoBehaviour
{
    public GameEventArgs m_ProjectileHit;
    public ModifierScriptable m_HealthModifier;

    void OnTriggerEnter(Collider other)
    {        
        //Args list
        //1. this object that invokes the event
        //2. this objects helth mod
        //3. this object armor mod
        //4. object collided with
        m_ProjectileHit.Raise(this.gameObject, m_HealthModifier, other.gameObject);
        Destroy(this.gameObject);
    }
}
