using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStats : MonoBehaviour
{
    public GameEventArgs m_ProjectileHit;
    public ModifierScriptable m_HealthModifier;
    public GameObject parent;

    private void Start()
    {
        parent = transform.parent.gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        //Args list
        //1. this object that invokes the event
        //2. this objects health mod        
        //4. object collided with
        if (transform.parent.gameObject != other.gameObject)
        {
            m_ProjectileHit.Raise(this.gameObject, m_HealthModifier, other.gameObject);
        }
    }
}
