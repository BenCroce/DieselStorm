using UnityEngine.Networking;
using UnityEngine;
using System.Collections.Generic;
public class NetworkTankInputController : NetworkBehaviour
{
    private float m_hinput;
    private float m_jinput;
    private float m_vinput;
    public GameObject m_vcam;
    public MovementBehaviour m_movement;
    public MagazineBehaviour m_Mag;
    public InputAxisScriptable m_HorizontalAxis;
    public InputAxisScriptable m_VerticalAxis;
    public InputAxisScriptable m_JumpAxis;
    public InputAxisScriptable m_TurretPitchAxis;
    public InputAxisScriptable m_TurretYawAxis;
    public InputButtonScriptable m_FireButton;
    public InputButtonScriptable m_ReloadButton;
    public List<MouseRotationBehaviour> m_TurretRotations;    

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
    void Start()
    {
        if (!isLocalPlayer)
            m_vcam.SetActive(false);
        m_movement = GetComponent<TankMovementBehaviour>();
    }
    // Update is called once per frame
    void Update()
    {
        //if (!isLocalPlayer)
        //    return;
        if (!hasAuthority)
            return;
        m_hinput = m_HorizontalAxis.AxisValue();
        m_vinput = m_VerticalAxis.AxisValue();
        m_jinput = m_JumpAxis.AxisValue();
        if (m_FireButton.WasButtonPressed())
        {
            m_Mag.UseRound();
        }
        if (m_ReloadButton.WasButtonPressed())
        {
            m_Mag.ReloadMag();
        }

        m_TurretRotations.ForEach(item => item.Rotate(m_TurretPitchAxis.AxisValue(), 
            m_TurretYawAxis.AxisValue()));            
        m_movement.Move(m_hinput, Vinput, m_jinput);
    }
}
