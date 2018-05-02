using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBehaviour : MonoBehaviour {

    public float m_lifetime;

    public ParticleSystem m_trail;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(DestroySelf());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(m_lifetime);
        Destroy(gameObject);
        m_trail.transform.parent = null;
        m_trail.transform.localScale = new Vector3(1, 1, 1);
        m_trail.Stop();
        m_trail.GetComponent<ShellTrailBehaviour>().enabled = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (transform.parent.gameObject != collision.gameObject)
        {
            Destroy(gameObject);
            m_trail.transform.parent = null;
            m_trail.transform.localScale = new Vector3(1, 1, 1);
            m_trail.Stop();
            m_trail.GetComponent<ShellTrailBehaviour>().enabled = true;
        }
    }
}
