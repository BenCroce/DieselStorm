using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "HealthStat", menuName = "HealthScriptable")]
public class HealthScriptable : StatScriptable
{

    [SerializeField]
    public GameEventArgs m_HealthStatChanged;

    public override void Apply(ModifierScriptable mod)
    {
        base.Apply(mod);
        m_HealthStatChanged.Raise(this);
    }
}