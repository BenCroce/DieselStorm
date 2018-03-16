using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ArmorStat", menuName = "ArmorScriptable")]
public class ArmorScriptable : StatScriptable
{
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_Mitigation;

    public float Mitigation { get { return m_Mitigation; } }
}
