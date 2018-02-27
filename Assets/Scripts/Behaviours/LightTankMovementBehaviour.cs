using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LightTankMovementBehaviour : MonoBehaviour
{
    public LightTankScriptable m_LightTankScriptable;
    public Rigidbody m_Rigidbody;
    public float m_HooverHeight;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, m_HooverHeight))
        {
            float d2f = (m_HooverHeight - hit.distance);
            Vector3 appliedHooverForce = Vector3.up * d2f *
                                         m_LightTankScriptable.m_HooverForce;
            m_Rigidbody.AddForceAtPosition(appliedHooverForce, transform.position,
                ForceMode.Acceleration);
            Quaternion newRotation = new Quaternion(hit.transform.rotation.x,
                transform.rotation.y, hit.transform.rotation.z, transform.rotation.w);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation,
                Time.deltaTime);

            #region Test Code                     
            if (Input.GetKey(KeyCode.W))
                m_Rigidbody.AddForce(transform.forward * 
                    m_LightTankScriptable.m_ForwardMovementSpeed);
            if (Input.GetKey(KeyCode.S))
                m_Rigidbody.AddForce(transform.forward * 
                    -m_LightTankScriptable.m_ForwardMovementSpeed);
            if(Input.GetKeyDown(KeyCode.A))
                StrafeLeft();
            if (Input.GetKeyDown(KeyCode.D))
                StrafeRight();
            if(Input.GetKey(KeyCode.E))
                RotateClockwise();
            if(Input.GetKey(KeyCode.Q))
                RotateCounterClockwise();
            #endregion
        }
    }

    [ContextMenu("Strafe Right")]
    void StrafeRight()
    {
        m_Rigidbody.AddForce(transform.right * m_LightTankScriptable.m_StrafeForce, 
            ForceMode.Impulse);
    }

    [ContextMenu("Strafe Left")]
    void StrafeLeft()
    {
        m_Rigidbody.AddForce(transform.right * -m_LightTankScriptable.m_StrafeForce,
            ForceMode.Impulse);
    }

    void RotateClockwise()
    {
        transform.Rotate(transform.up, m_LightTankScriptable.m_TurnSpeed * Time.deltaTime);
    }

    void RotateCounterClockwise()
    {
        transform.Rotate(transform.up, -m_LightTankScriptable.m_TurnSpeed * Time.deltaTime);
    }
}
