using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStats : MonoBehaviour
{
    [SerializeField]
    private List<StatScriptable> m_stats;

    [SerializeField]
    private List<ModifierScriptable> m_Mods;

    void OnTriggerEnter(Collider other)
    {
        foreach (var mod in m_Mods)
        {
            if (other.GetComponent<TankStats>() != null)
            {
                if (other.GetComponent<TankStats>().HasStat(mod))
                {
                    mod.NegativeMod();
                }
            }
        }
    }
}
