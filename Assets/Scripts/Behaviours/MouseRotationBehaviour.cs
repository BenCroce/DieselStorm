using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotationBehaviour : MonoBehaviour
{
    public bool m_ClampVertical;
    [Range(-1,1)]
    public float m_MaxUpRadian;
    [Range(-1, 1)]
    public float m_MaxDownRadian;

    public bool m_ClampHorizontal;
    [Range(-1, 1)]
    public float m_MaxRightRadian;
    [Range(-1, 1)]
    public float m_MaxLeftRadian;
    public float m_MouseHorizontalSensitivity;
    public float m_MouseVerticalSensitivity;

    void Start()
    {        
    }

    void FixedUpdate()
    {
        Vector3 mousePos = new Vector3(Input.GetAxisRaw("Mouse Y") * m_MouseVerticalSensitivity,
            Input.GetAxisRaw("Mouse X") * m_MouseHorizontalSensitivity, 0);       

        Quaternion newRotation = transform.rotation * Quaternion.Euler(mousePos);
        if(m_ClampVertical)
            newRotation.x = Mathf.Clamp(newRotation.x, m_MaxDownRadian, m_MaxUpRadian);
        if (m_ClampHorizontal)
            newRotation.y = Mathf.Clamp(newRotation.y, m_MaxLeftRadian, m_MaxRightRadian);


        transform.rotation = newRotation;
    }
}