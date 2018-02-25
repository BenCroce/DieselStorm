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
            m_Rigidbody.AddForce(transform.forward * 0.1f);
        }

    }

    void OnDrawGizmos()
    {
    }

}
