using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Modifier", menuName = "ModifierScriptable")]
public class ModifierScriptable : ScriptableObject
{
    public enum  ModType
    {
        ADD,
        MULT        
    }

    public StatScriptable m_Stat;
    public ModType m_Type;
    public int m_Value;

    void OnEnable()
    {
        if(m_Stat == null)
            Debug.LogError("No value for m_Stat set on ModiferScriptable:" + name);
    }

    public virtual void RemoveMod()
    {
        m_Stat.Remove(this);
    }

    public virtual void ApplyMod()
    {
        m_Stat.Apply(this);
    }
}
