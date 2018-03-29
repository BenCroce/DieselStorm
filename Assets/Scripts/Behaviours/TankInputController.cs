using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankInputController : MonoBehaviour
{

    private float m_hinput;
    private float m_jinput;
    private float m_vinput;
    public MovementBehaviour movement;

    public float Hinput
    {
        get
        {
            return m_hinput;
        }

        set
        {
            m_hinput = value;
        }
    }

    public float Jinput
    {
        get
        {
            return m_jinput;
        }

        set
        {
            m_jinput = value;
        }
    }

    public float Vinput
    {
        get
        {
            return m_vinput;
        }

        set
        {
            m_vinput = value;
        }
    }

    // Use this for initialization

    // Update is called once per frame
    void Update ()
    {
        m_hinput = Input.GetAxis("Horizontal");
        Vinput = Input.GetAxis("Vertical");
        m_jinput = Input.GetAxis("Jump");
        movement.Move(m_hinput, Vinput, m_jinput);
    }
}
