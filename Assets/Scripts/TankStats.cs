using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class TankStats : MonoBehaviour, IDamageable
{
    public HealthScriptable m_HealthStat;
    public ArmorScriptable m_ArmorStat;
    [SerializeField]
    public GameEventArgs m_TankStatsChanged;

    public GameEventArgs m_TankDestroyed;

    void Awake()
    {
        HealthScriptable newHP = ScriptableObject.CreateInstance<HealthScriptable>();
        newHP.CreateInstance(m_HealthStat);
        m_HealthStat = newHP;
        ArmorScriptable newArmor = ScriptableObject.CreateInstance<ArmorScriptable>();
        newArmor.CreateInstance(m_ArmorStat);
        m_ArmorStat = newArmor;
    }

    public void TakeDamage(ModifierScriptable modifier)
    {        
        m_TankStatsChanged.Raise(this);
        m_HealthStat.Apply(modifier);
    }

    public void DamageArmor(ModifierScriptable modifier)
    {
        m_TankStatsChanged.Raise(this);  
        if(m_ArmorStat.ArmorRemaining())
            m_ArmorStat.Apply(modifier);
        else
            TakeDamage(modifier);      
    }

    public void OnTakeDamage(UnityEngine.Object[] args)
    {
        if(args[3] != this.gameObject)
            return;
        var sender = args[0] as GameObject;
        var healthMod = args[1] as ModifierScriptable;
        var armorMod = args[2] as ModifierScriptable;
        if (sender == null)
            return;        
        DamageArmor(armorMod);
        TakeDamage(healthMod);
    }

    public void DestroyObject(UnityEngine.Object[] args)
    {
        if (args[0] == m_HealthStat)
        {
            m_TankDestroyed.Raise(this);
            Destroy(this.gameObject);
        }
    }
}
