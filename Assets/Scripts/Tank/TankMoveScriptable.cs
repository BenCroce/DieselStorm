using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankMoveScriptable", menuName = "Movement/Tank")]
public class TankMoveScriptable : ScriptableObject
{
    public float m_forwardForce;
    public float m_strafeForce;
    public float m_turnSpeed;
    public float m_heightLimit;
}
