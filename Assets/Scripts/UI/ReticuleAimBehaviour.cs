using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ReticuleAimBehaviour : MonoBehaviour {

    public RectTransform m_reticule;
    public float m_smoothSpeed;
    public Transform m_aim;
    public float m_worldOffset = 30;

    bool m_searching = true;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(FindLoop());
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_aim == null && !m_searching)
        {
            StartCoroutine(FindLoop());
            m_searching = true;
            return;
        }

        if (m_aim != null)
        {
            Vector3 newpos = Camera.main.WorldToScreenPoint(m_aim.TransformPoint(Vector3.forward * m_worldOffset));
            m_reticule.position = Vector3.Lerp(m_reticule.position, newpos, m_smoothSpeed);

        }
	}

    void FindLocalPlayer()
    {
        foreach(SimplePlayerBehaviour i in FindObjectsOfType<SimplePlayerBehaviour>())
        {
            if (i.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
                if (i.m_rtTankObject != null)
                {
                    if(i.m_rtTankObject.GetComponent<TankShoot>())
                        m_aim = i.m_rtTankObject.GetComponent<TankShoot>().m_turretPos;
                    else if(i.m_rtTankObject.GetComponent<HitscanShootBehaviour>())
                        m_aim = i.m_rtTankObject.GetComponent<HitscanShootBehaviour>().m_muzzle;
                    m_searching = false;
                }
        }
    }

    IEnumerator FindLoop()
    {
        while (m_aim == null && m_searching)
        {
            yield return new WaitForSeconds(0.1f);
            FindLocalPlayer();
        }
    }
}
