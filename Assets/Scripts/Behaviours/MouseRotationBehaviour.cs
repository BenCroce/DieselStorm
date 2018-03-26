using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotationBehaviour : MonoBehaviour
{
    public MouseRotationScriptable m_MouseRotationScriptable;
    public Transform m_RotatedObject;
    public Quaternion m_InitialRotation;
    public Quaternion newRotation;

    void Awake()
    {
        if (m_RotatedObject == null)
            m_RotatedObject = this.transform;
        m_InitialRotation = m_RotatedObject.localRotation;
    }

    void Update()
    {
        Rotate(Input.GetAxis("Turret Pitch"), Input.GetAxis("Turret Yaw"));
    }

    public void Rotate(float y_axis, float x_axis)
    {
        float horizontal = (m_MouseRotationScriptable.m_InvertVerticalAxis)
            ? -y_axis * m_MouseRotationScriptable.m_MouseVerticalSensitivity
            : y_axis * m_MouseRotationScriptable.m_MouseVerticalSensitivity;

        float vertical = (m_MouseRotationScriptable.m_InvertHorizontalAxis)
            ? -x_axis * m_MouseRotationScriptable.m_MouseHorizontalSensitivity
            : x_axis * m_MouseRotationScriptable.m_MouseHorizontalSensitivity;


        Vector3 mousePos = new Vector3(horizontal, vertical, 0);  

        newRotation = m_RotatedObject.transform.localRotation * Quaternion.Euler(mousePos);
        if (m_MouseRotationScriptable.m_ClampVertical)
            newRotation.x = Mathf.Clamp(newRotation.x, m_MouseRotationScriptable.m_MaxDownRadian,
                m_MouseRotationScriptable.m_MaxUpRadian);
        else
            newRotation.x = m_InitialRotation.x;
        if (m_MouseRotationScriptable.m_ClampHorizontal)
            newRotation.y = Mathf.Clamp(newRotation.y, m_MouseRotationScriptable.m_MaxLeftRadian,
                m_MouseRotationScriptable.m_MaxRightRadian);
        else
            newRotation.y = m_InitialRotation.y;

        newRotation.z = m_RotatedObject.transform.localRotation.z;
        newRotation *= m_InitialRotation;        

        if (m_RotatedObject.transform.localRotation != newRotation)
        {
            m_RotatedObject.transform.localRotation = Quaternion.Lerp(
                m_RotatedObject.transform.localRotation, newRotation, 
                Time.deltaTime * m_MouseRotationScriptable.m_RotationSpeed);
        }
    }
}