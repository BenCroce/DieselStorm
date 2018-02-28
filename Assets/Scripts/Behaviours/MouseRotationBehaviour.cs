using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotationBehaviour : MonoBehaviour
{
    public MouseRotationScriptable m_MouseRotationScriptable;

    void FixedUpdate()
    {
        float horizontal = (m_MouseRotationScriptable.m_InvertVerticalAxis)
            ? -Input.GetAxisRaw("Mouse Y") * m_MouseRotationScriptable.m_MouseVerticalSensitivity
            : Input.GetAxisRaw("Mouse Y") * m_MouseRotationScriptable.m_MouseVerticalSensitivity;

        float vertical = (m_MouseRotationScriptable.m_InvertHorizontalAxis)
            ? - Input.GetAxisRaw("Mouse X") * m_MouseRotationScriptable.m_MouseHorizontalSensitivity
            : Input.GetAxisRaw("Mouse X") * m_MouseRotationScriptable.m_MouseHorizontalSensitivity;


        Vector3 mousePos = new Vector3(horizontal, vertical, 0);  

        Quaternion newRotation = transform.localRotation * Quaternion.Euler(mousePos);
        if(m_MouseRotationScriptable.m_ClampVertical)
            newRotation.x = Mathf.Clamp(newRotation.x, m_MouseRotationScriptable.m_MaxDownRadian,
                m_MouseRotationScriptable.m_MaxUpRadian);
        if (m_MouseRotationScriptable.m_ClampHorizontal)
            newRotation.y = Mathf.Clamp(newRotation.y, m_MouseRotationScriptable.m_MaxLeftRadian,
                m_MouseRotationScriptable.m_MaxRightRadian);

        newRotation.z = transform.localRotation.z;

        transform.localRotation = newRotation;
    }
}