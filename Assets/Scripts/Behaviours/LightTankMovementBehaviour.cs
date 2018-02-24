using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LightTankMovementBehaviour : MonoBehaviour
{
    public LightTankScriptable m_LightTankScriptable;
    public Rigidbody m_Rigidbody;
    public float m_HooverHeight;
    public List<Vector3> m_ThrusterOffset;
    public List<Vector3> m_ThrusterLocations;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            m_ThrusterLocations[i] = transform.position + m_ThrusterOffset[i];
            Ray ray = new Ray(m_ThrusterLocations[i], -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, m_HooverHeight))
            {
                float d2f = (m_HooverHeight - hit.distance) / m_HooverHeight;
                Vector3 appliedHooverForce = Vector3.up * d2f *
                                             m_LightTankScriptable.m_HooverForce;
                m_Rigidbody.AddForceAtPosition(appliedHooverForce, m_ThrusterLocations[i],
                    ForceMode.Acceleration);
                Quaternion newRotation = new Quaternion(hit.transform.rotation.x,
                    transform.rotation.y, hit.transform.rotation.z, transform.rotation.w);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation,
                    Time.deltaTime);
            }
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var thruster in m_ThrusterLocations)
        {
            Gizmos.DrawWireSphere(thruster, .5f);
        }
    }

}
