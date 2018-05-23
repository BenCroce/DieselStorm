using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(channel = 0, sendInterval = 0.05f)]
public class HitscanShootBehaviour : NetworkBehaviour {

    public NetworkTankInputController m_control;
    public float m_shotCooldown;
    public int m_shotRange;
    public float m_spread;
    public Transform m_muzzle;
    public GameEventArgs m_hitEvent;
    public ModifierScriptable m_shotStats;
    public MagazineBehaviour m_magazine;
    public LineRenderer m_shootLine;
    public AudioSource m_shotSound;
    public Light m_MuzzleFlash;
    public NetworkTank player;
    public float m_renderTime = 0.1f;

    Ray m_shootRay;
    RaycastHit m_shootHit;
    bool canshoot = true;

	// Use this for initialization
	void Start ()
    {
        m_shootLine.enabled = false;
        m_MuzzleFlash.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if(m_control.m_FireButton.IsButtonDown() && canshoot && player.isLocalPlayer)
        {
            Vector3 shootVec = Vector3.Normalize(m_muzzle.forward + new Vector3(Random.Range(-m_spread, m_spread), Random.Range(-m_spread, m_spread), Random.Range(-m_spread, m_spread)));
            CmdShoot(shootVec);
            canshoot = false;

            m_shootRay = new Ray(m_muzzle.position, shootVec);

            m_shootLine.enabled = true;
            m_MuzzleFlash.gameObject.SetActive(true);
            m_shotSound.Play();

            m_shootLine.SetPosition(0, m_muzzle.position);

            if (Physics.Raycast(m_shootRay, out m_shootHit, m_shotRange))
            {
                m_shootLine.SetPosition(1, m_shootHit.point);
            }
            else
                m_shootLine.SetPosition(1, m_muzzle.position + shootVec * m_shotRange);

            StartCoroutine(Cooldown());
        }

        if(m_shootLine.enabled)
        {
            StartCoroutine(StopFX());
        }
	}

    [Command]
    void CmdShoot(Vector3 shootVec)
    {
        RpcShoot(shootVec);
    }

    [ClientRpc]
    void RpcShoot(Vector3 shootVec)
    {
        if (!player.isLocalPlayer)
        {
            m_shootRay = new Ray(m_muzzle.position, shootVec);

            m_shootLine.enabled = true;
            m_MuzzleFlash.gameObject.SetActive(true);
            m_shotSound.Play();

            m_shootLine.SetPosition(0, m_muzzle.position);

            if (Physics.Raycast(m_shootRay, out m_shootHit, m_shotRange))
            {
                if (m_shootHit.collider.GetComponent<TankStats>() && m_shootHit.collider.gameObject != gameObject)
                    m_hitEvent.Raise(gameObject, m_shotStats, m_shootHit.collider.gameObject);

                m_shootLine.SetPosition(1, m_shootHit.point);
            }
            else
                m_shootLine.SetPosition(1, m_muzzle.position + shootVec * m_shotRange);
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSecondsRealtime(m_shotCooldown);
        canshoot = true;
    }

    IEnumerator StopFX()
    {
        yield return new WaitForSecondsRealtime(m_renderTime);
        m_shootLine.enabled = false;
        m_MuzzleFlash.gameObject.SetActive(false);
    }
}
