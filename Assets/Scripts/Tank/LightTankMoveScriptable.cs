using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightTankMovement", menuName = "Tank Force Scriptable")]
public class LightTankMoveScriptable : ScriptableObject
{
    public float m_moveForce;
    public float m_hoverForce;
    public float m_hoverHeight;
    public float m_steerSpeed;
}
