using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightTankScriptable", menuName = "Tanks/LightTank")]
public class LightTankScriptable : ScriptableObject
{
    public float m_HooverForce;
    public float m_StrafeForce;
    public float m_ForwardMovementSpeed;
    public float m_TurnSpeed;
}
