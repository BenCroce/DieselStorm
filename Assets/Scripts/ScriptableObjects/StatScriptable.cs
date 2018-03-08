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

    public string Name
    {
        get { return m_Name; }
    }

    public void CreateInstance(float baseValue, string statName, float value)
    {
        m_BaseValue = baseValue;
        m_Name = statName;
        m_Value = value;
        name = m_Name;
    }

    public void CreateInstance(StatScriptable statScriptable)
    {
        m_BaseValue = statScriptable.m_BaseValue;
        m_Name = statScriptable.m_Name;
        m_Value = statScriptable.m_Value;        
    }

    void OnEnable()
    {
        m_Value = m_BaseValue;
        m_Name = name;
    }

    public virtual void Apply(ModifierScriptable mod)
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
