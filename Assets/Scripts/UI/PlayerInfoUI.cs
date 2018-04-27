using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour {

    public Text m_healthText;
    public Text m_speedText;

    public TankStats m_player;
    public Rigidbody m_playerBody;

    float xzspeed;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        UIUpdate();
        xzspeed = new Vector3(m_playerBody.velocity.x, 0, m_playerBody.velocity.z).magnitude;
	}

    void UIUpdate()
    {
        m_healthText.text = m_player.rt_Health.m_Value.ToString();
        m_speedText.text = ((int)xzspeed).ToString();
    }
}
