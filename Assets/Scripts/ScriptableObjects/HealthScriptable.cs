using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "HealthStat", menuName = "HealthScriptable")]
public class HealthScriptable : StatScriptable
{

    [SerializeField]
    public GameEventArgs m_HealthStatChanged;

    public GameEventArgs m_HealthHitZero;

    public override void Apply(ModifierScriptable mod)
    {
        base.Apply(mod);
        m_HealthStatChanged.Raise(this);
        if(m_Value <= 0)
            m_HealthHitZero.Raise(this);
    }
}