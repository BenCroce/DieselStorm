using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBehaviour : MonoBehaviour
{

    public float m_lifetime;

    public ParticleSystem m_trail;
    public ParticleSystem m_explosion;
    public AudioSource m_explosionSound;
    public AudioSource m_shotSound;

    public int m_explosionRadius = 5;
    public int m_explosionForce = 10;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(DestroySelf());
        m_explosionSound.pitch += Random.Range(-0.2f, 0.2f);
        m_shotSound.pitch += Random.Range(-0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_explosionRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody;

            if (colliders[i].GetComponent<Rigidbody>())
                targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            else
                continue;

            targetRigidbody.AddExplosionForce(m_explosionForce, transform.position, m_explosionRadius);
        }

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
            Explode();
            Destroy(gameObject);
            m_trail.transform.parent = null;
            m_trail.transform.localScale = new Vector3(1, 1, 1);
            m_trail.Stop();
            m_trail.GetComponent<ShellTrailBehaviour>().enabled = true;
            m_explosion.transform.parent = null;
            m_explosion.transform.position += new Vector3(0, 1, 0);
            m_trail.transform.localScale = new Vector3(1, 1, 1);
            m_explosion.Play();
            m_explosion.GetComponent<ShellTrailBehaviour>().enabled = true;
            m_explosionSound.enabled = true;
            m_explosionSound.Play();
        }
    }
}
