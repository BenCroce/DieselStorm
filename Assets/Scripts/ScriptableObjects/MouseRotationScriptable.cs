using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MouseRotationScriptable", menuName = "Controls/MouseRotation")]
public class MouseRotationScriptable : ScriptableObject
{
    public bool m_ClampVertical;
    public bool m_InvertVerticalAxis;
    [Range(-1, 1)]
    public float m_MaxUpRadian;
    [Range(-1, 1)]
    public float m_MaxDownRadian;

    public bool m_InvertHorizontalAxis;
    public bool m_ClampHorizontal;
    [Range(-1, 1)]
    public float m_MaxRightRadian;
    [Range(-1, 1)]
    public float m_MaxLeftRadian;
    public float m_MouseHorizontalSensitivity;
    public float m_MouseVerticalSensitivity;

    public float m_RotationSpeed;
}
