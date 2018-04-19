using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HoverScriptable", menuName = "Movement/Hover")]
public class HoverScriptable : ScriptableObject
{
    public float m_hoverHeight;
    public float m_hoverForce;
    public float m_hoverBalanceOffset;
    public float m_balanceHeightLimit;
    public float m_balanceMultiplier;
    public float m_uprightMultiplier;
}
