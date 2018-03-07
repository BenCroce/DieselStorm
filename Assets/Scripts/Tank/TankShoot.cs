using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankShoot : MonoBehaviour {

    public GameObject m_shell;
    public Transform m_turretPos;

    public float m_shootForce = 20;
    public float m_shootCooldown = 2;

	// Use this for initialization
	void Start ()
    {
        //StartCoroutine(Shoot());
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    /*IEnumerator Shoot()
    {
        while (true)
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObject shot = Instantiate(m_shell, m_turretPos.position, m_turretPos.rotation);
                shot.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, m_shootForce), ForceMode.Impulse);
                shot.GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity, ForceMode.Impulse);
                yield return new WaitForSeconds(m_shootCooldown);
            }
            else
                yield return new WaitForFixedUpdate();
        
    }*/
}
