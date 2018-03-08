using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class TankStats : MonoBehaviour, IDamageable
{
    [SerializeField]
    private List<StatScriptable> m_Stats;

    [SerializeField]
    public GameEventArgs m_TankStatsChanged;

    void Awake()
    {
        List<StatScriptable> newStats = new List<StatScriptable>();
        foreach (var statRef in m_Stats)
        {
            StatScriptable stat = ScriptableObject.CreateInstance<StatScriptable>();
            stat.CreateInstance(statRef);
            newStats.Add(stat);
        }
        m_Stats = newStats;
    }

    public StatScriptable HasStat(ModifierScriptable mod)
    {
        foreach (var stat in m_Stats)
        {
            if (stat.Name == mod.m_Stat.Name)
                return stat;
        }
        return null;
    }

    public void TakeDamage(ModifierScriptable modifier)
    {
        m_TankStatsChanged.Raise(this);
    }
}
