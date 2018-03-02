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

    public void NegativeMod()
    {
        m_Stat.Remove(this);
    }

    public void PositiveMod()
    {
        m_Stat.Apply(this);
    }
}
