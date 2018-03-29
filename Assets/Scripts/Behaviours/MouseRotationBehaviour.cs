﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotationBehaviour : MonoBehaviour
{
    public MouseRotationScriptable m_MouseRotationScriptable;
    public Transform m_RotatedObject;    
    public Quaternion newRotation;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;        
        if (m_RotatedObject == null)
            m_RotatedObject = this.transform;        
    }

    //void Update()
    //{
    //    Rotate(Input.GetAxis("Turret Pitch"), Input.GetAxis("Turret Yaw"));
    //}

    public void Rotate(float y_axis, float x_axis)
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.LeftShift))
            Cursor.lockState = CursorLockMode.None;
        if (Input.GetKeyDown(KeyCode.Tab))
            Cursor.lockState = CursorLockMode.Locked;
#endif
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

        if (m_MouseRotationScriptable.m_ClampHorizontal)
            newRotation.y = Mathf.Clamp(newRotation.y, m_MouseRotationScriptable.m_MaxLeftRadian,
                m_MouseRotationScriptable.m_MaxRightRadian);

        newRotation.z = m_RotatedObject.transform.localRotation.z;      

        if (m_RotatedObject.transform.localRotation != newRotation)
        {
            m_RotatedObject.transform.localRotation = Quaternion.Lerp(
                m_RotatedObject.transform.localRotation, newRotation, 
                Time.deltaTime * m_MouseRotationScriptable.m_RotationSpeed);
        }
    }
}