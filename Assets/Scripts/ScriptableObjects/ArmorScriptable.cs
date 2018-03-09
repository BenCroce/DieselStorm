using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ArmorStat", menuName = "ArmorScriptable")]
public class ArmorScriptable : StatScriptable
{
    [SerializeField]
    public GameEventArgs m_ArmorStatChanged;

    [SerializeField]
    public GameEventArgs m_ArmorHitZero;

    public void CreateInstance(ArmorScriptable armorScriptable)
    {
        base.CreateInstance(armorScriptable);
        m_ArmorHitZero = armorScriptable.m_ArmorHitZero;
        m_ArmorStatChanged = armorScriptable.m_ArmorStatChanged;
    }

    public override void Apply(ModifierScriptable mod)
    {
        base.Apply(mod);
        m_ArmorStatChanged.Raise(this, mod);        
        if (!ArmorRemaining())
            m_ArmorHitZero.Raise(this);
    }

    public bool ArmorRemaining()
    {
        return m_Value > 0;
    }
}
