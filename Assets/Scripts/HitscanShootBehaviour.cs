using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HitscanShootBehaviour : NetworkBehaviour {

    public NetworkTankInputController m_control;
    public float m_shotCooldown;
    public int m_shotRange;
    public Transform m_muzzle;
    public GameEventArgs m_hitEvent;
    public ModifierScriptable m_shotStats;
    public MagazineBehaviour m_magazine;
    public LineRenderer m_shootLine;
    public float m_renderTime = 0.1f;

    Ray m_shootRay;
    RaycastHit m_shootHit;
    bool canshoot = true;

	// Use this for initialization
	void Start ()
    {
        m_shootLine.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(m_control.m_FireButton.IsButtonDown() && canshoot)
        {
            CmdShoot();
            canshoot = false;
            StartCoroutine(Cooldown());
            StartCoroutine(StopLineRender());
        }
	}

    [Command]
    void CmdShoot()
    {
        if (canshoot)
        {
            RpcShoot();
        }
    }

    [ClientRpc]
    void RpcShoot()
    {
        m_shootRay = new Ray(m_muzzle.position, m_muzzle.forward);

        m_shootLine.enabled = true;
        m_shootLine.SetPosition(0, m_muzzle.position);

        if (Physics.Raycast(m_shootRay, out m_shootHit, m_shotRange))
        {
            if (m_shootHit.collider.GetComponent<TankStats>() && m_shootHit.collider.gameObject != gameObject)
                m_hitEvent.Raise(gameObject, m_shotStats, m_shootHit.collider.gameObject);

            m_shootLine.SetPosition(1, m_shootHit.point);
        }
        else
            m_shootLine.SetPosition(1, m_muzzle.position + m_muzzle.forward * m_shotRange);

        StartCoroutine(StopLineRender());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(m_shotCooldown);
        canshoot = true;
    }

    IEnumerator StopLineRender()
    {
        yield return new WaitForSeconds(m_renderTime);
        m_shootLine.enabled = false;
    }
}
