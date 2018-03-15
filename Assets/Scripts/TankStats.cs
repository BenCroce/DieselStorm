using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class TankStats : MonoBehaviour, IDamageable
{
    public HealthScriptable m_HealthStat;
    public ArmorScriptable m_ArmorStat;

    public StatScriptable rt_Health;
    public StatScriptable rt_Armor;

    [SerializeField]
    public GameEventArgs m_TankStatsChanged;
    [SerializeField]
    public GameEventArgs m_TankDestroyed;

    void Awake()
    {
        rt_Health = m_HealthStat.CreateInstance() as HealthScriptable;
        rt_Armor = m_ArmorStat.CreateInstance() as ArmorScriptable;
    }

    public void TakeDamage(ModifierScriptable modifier)
    {
        rt_Health.Apply(modifier);
        m_TankStatsChanged.Raise(this);       
    }
    
    public void DamageArmor(ModifierScriptable modifier)
    {

        if (rt_Armor.m_Value > 0)
            rt_Armor.Apply(modifier);
        else
            TakeDamage(modifier);
        m_TankStatsChanged.Raise(this);
    }

    public void OnTakeDamage(UnityEngine.Object[] args)
    {        
        var sender = args[0] as GameObject;
        var healthMod = args[1] as ModifierScriptable;
        var armorMod = args[2] as ModifierScriptable;
        var collidedwith = args[3] as GameObject;
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
