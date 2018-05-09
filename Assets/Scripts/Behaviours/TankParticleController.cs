using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankStats))]
public class TankParticleController : MonoBehaviour {

    public int m_smokeAmount = 5;

    public ParticleSystem m_SmokeParticles;
    public ParticleSystem m_HeatDistortion;
    public ParticleSystem m_Explosion;
    public TankStats m_tankStats;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_SmokeParticles.emissionRate = Mathf.Abs((m_tankStats.rt_Health.m_Value - m_tankStats.m_HealthStat.m_Value) / 100 * m_smokeAmount);
	}

    private void OnDestroy()
    {
        //m_Explosion.transform.parent = null;
        //m_Explosion.Play();
    }
}
