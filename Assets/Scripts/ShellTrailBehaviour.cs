using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellTrailBehaviour : MonoBehaviour {

    bool m_stopped = false;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(m_stopped == false && transform.parent == null)
        {
            StartCoroutine(DestroyTrail());
            m_stopped = true;
        }
	}

    public IEnumerator DestroyTrail()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().startLifetime);
        Destroy(gameObject);
    }
}
