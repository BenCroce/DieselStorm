using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileStats))]
public class ExplosionBehaviour : MonoBehaviour {

    public ShellBehaviour m_shell;

    ProjectileStats m_stats;

    [SerializeField]
    ModifierScriptable rt_damage;

	// Use this for initialization
	void Start ()
    {
        m_stats = GetComponent<ProjectileStats>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent.gameObject == other.gameObject)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, m_shell.m_explosionRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            TankStats targetTankStats;

            if (colliders[i].GetComponent<TankStats>())
                targetTankStats = colliders[i].GetComponent<TankStats>();
            else
                continue;

            if (colliders[i] != other)
            {
                rt_damage = Instantiate(m_stats.m_HealthModifier) as ModifierScriptable;
                float distmult = ((m_shell.m_explosionRadius - Vector3.Magnitude(transform.position - colliders[i].transform.position)) / m_shell.m_explosionRadius);
                rt_damage.m_Value = (int)Mathf.Round(rt_damage.m_Value * distmult);
                m_stats.m_ProjectileHit.Raise(this.gameObject, rt_damage, colliders[i].gameObject);
            }
        }
    }
}
