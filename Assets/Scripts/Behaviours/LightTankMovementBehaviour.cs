using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LightTankMovementBehaviour : MonoBehaviour
{
    public LightTankScriptable m_LightTankScriptable;
    public Rigidbody m_Rigidbody;
    public float m_MinToGround;
    public float d2f;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {   
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, transform.lossyScale, -transform.up, out hit))
        {
             d2f = Vector3.Distance(transform.position, hit.point);
        }        
        if (d2f < m_MinToGround)
            m_Rigidbody.AddForce(m_LightTankScriptable.m_ThrusterForce);
    }    
}
