using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightTankMovement", menuName = "Tank Force Scriptable")]
public class LightTankMoveScriptable : ScriptableObject
{
    public float moveForce;
    public float hoverForce;
    public float hoverHeight;
}
