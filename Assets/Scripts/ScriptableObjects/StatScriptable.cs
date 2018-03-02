using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "StatScriptable")]
public class StatScriptable : ScriptableObject
{
    [SerializeField]
    private float m_BaseValue;

    [SerializeField]
    private string m_Name;

    public float m_Value;

    void OnEnable()
    {
        m_Value = m_BaseValue;
        m_Name = name;
    }

    public void Apply(ModifierScriptable mod)
    {
        if (mod.m_Type == ModifierScriptable.ModType.ADD)
            m_Value += mod.m_Value;
        if (mod.m_Type == ModifierScriptable.ModType.MULT)
            m_Value += m_BaseValue * mod.m_Value / 10;
    }

    public void Remove(ModifierScriptable mod)
    {
        if (mod.m_Type == ModifierScriptable.ModType.ADD)
            m_Value -= mod.m_Value;
        if (mod.m_Type == ModifierScriptable.ModType.MULT)
            m_Value -= m_BaseValue * mod.m_Value / 10;
    }
}

