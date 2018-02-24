using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightTankScriptable", menuName = "Tanks/LightTank")]
public class LightTankScriptable : ScriptableObject
{
    public Vector3 m_ThrusterForce;
    public float m_MaxForwardMovementSpeed;
    public float m_MaxHorizontalMovementSpeed;
}
