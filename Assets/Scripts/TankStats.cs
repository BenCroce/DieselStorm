using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankStats : MonoBehaviour
{
    [SerializeField]
    private List<StatScriptable> m_Stats;

    public bool HasStat(ModifierScriptable mod)
    {
        foreach (var stat in m_Stats)
        {
            if (stat == mod.m_Stat)
                return true;
        }
        return false;
    }
}
