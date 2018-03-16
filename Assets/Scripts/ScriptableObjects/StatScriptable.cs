using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "StatScriptable")]
public class StatScriptable : ScriptableObject
{
    [SerializeField]
    protected GameEventArgs StatChanged;

    [SerializeField]
    protected GameEventArgs HitZero;

    [SerializeField]
    private float m_BaseValue;

    [SerializeField]
    private string m_StatName;

    public float m_Value;

    public string StatName
    {
        get { return m_StatName; }
    }

    public StatScriptable CreateInstance()
    {
        var tmp = Instantiate(this);
        tmp.m_BaseValue = m_BaseValue;
        tmp.m_StatName = m_StatName;
        tmp.m_Value = m_Value;
        return tmp;
    }

    void OnEnable()
    {
        m_Value = m_BaseValue;
        m_StatName = name;
        StatChanged = Resources.Load("Events/StatChanged") as GameEventArgs;
        HitZero = Resources.Load("Events/HitZero") as GameEventArgs;
    }

    public virtual void Apply(ModifierScriptable mod)
    {
        if (mod.m_Type == ModifierScriptable.ModType.ADD)
            m_Value += mod.m_Value;
        if (mod.m_Type == ModifierScriptable.ModType.MULT)
            m_Value += m_BaseValue * mod.m_Value / 10;
        StatChanged.Raise(this);
        if (m_Value <= 0)
            HitZero.Raise(this);
    }

    public void Remove(ModifierScriptable mod)
    {
        if (mod.m_Type == ModifierScriptable.ModType.ADD)
            m_Value -= mod.m_Value;
        if (mod.m_Type == ModifierScriptable.ModType.MULT)
            m_Value -= m_BaseValue * mod.m_Value / 10;
    }
}
