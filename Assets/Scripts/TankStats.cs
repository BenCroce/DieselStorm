using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class TankStats : MonoBehaviour, IDamageable
{
    public HealthScriptable m_HealthStat;
    [SerializeField]
    public GameEventArgs m_TankStatsChanged;

    void Awake()
    {
        HealthScriptable newHP = ScriptableObject.CreateInstance<HealthScriptable>();
        newHP.CreateInstance(m_HealthStat);        
    }

    public void TakeDamage(ModifierScriptable modifier)
    {
        m_TankStatsChanged.Raise(this);
        m_HealthStat.Apply(modifier);
    }

    public void OnTakeDamage(UnityEngine.Object[] args)
    {
        var sender = args[0] as GameObject;
        var mod = args[1] as ModifierScriptable;
        if (sender == null)
            return;
        TakeDamage(mod);
    }

    void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
