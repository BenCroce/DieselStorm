using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotationBehaviour : MonoBehaviour
{
    public float m_MaxUpAngle;
    public float m_MaxDownAngle;
    public float m_MaxRightAngle;
    public float m_MaxLeftAngle;
    [Range(0,2)]
    public float m_MouseHorizontalSensitivity;
    [Range(0, 2)]
    public float m_MouseVerticalSensitivity;

    private Vector3 m_MouseRef;

    void Start()
    {
        m_MouseRef = Input.mousePosition;
    }

    void FixedUpdate()
    {
        Vector3 mouseOffset = Input.mousePosition - m_MouseRef;        

        transform.Rotate(new Vector3(0,mouseOffset.x * m_MouseHorizontalSensitivity, 0));

        m_MouseRef = Input.mousePosition;        
    }
}