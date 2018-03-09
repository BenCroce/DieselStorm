using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "HealthStat", menuName = "HealthScriptable")]
public class HealthScriptable : StatScriptable
{

    [SerializeField]
    public GameEventArgs m_HealthStatChanged;

    [SerializeField]
    public GameEventArgs m_HealthHitZero;

    public void CreateInstance(HealthScriptable healthScriptable)
    {
        base.CreateInstance(healthScriptable);
        m_HealthStatChanged = healthScriptable.m_HealthStatChanged;
        m_HealthHitZero = healthScriptable.m_HealthHitZero;
    }

    public override void Apply(ModifierScriptable mod)
    {        
        base.Apply(mod);
        m_HealthStatChanged.Raise(this, mod);
        if(m_Value <= 0)
            m_HealthHitZero.Raise(this);
    }
}