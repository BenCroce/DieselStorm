using UnityEngine.Networking;
using UnityEngine;
public class NetworkTankInputController : NetworkBehaviour
{
    private float m_hinput;
    private float m_jinput;
    private float m_vinput;
    public GameObject m_vcam;
    public MovementBehaviour m_movement;
    public TankShoot m_shooting;

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
    }
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;
        m_hinput = Input.GetAxis("Horizontal");
        m_vinput = Input.GetAxis("Vertical");
        m_jinput = Input.GetAxis("Jump");
        if (Input.GetButtonDown("Fire1"))
            m_shooting.Shoot();
        m_movement.Move(m_hinput, Vinput, m_jinput);
    }
}
