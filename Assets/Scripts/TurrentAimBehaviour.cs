using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurrentAimBehaviour : NetworkBehaviour {

    public Transform m_aimGuide;
    public Transform m_verticalAxis;
    public Transform m_horizontalAxis;
    public Vector3 m_aimRotation;

    public float m_pitchMinimum;

	// Use this for initialization
	void Start ()
    {   
        if(hasAuthority)     
            m_aimGuide = Camera.main.transform;
        if (!m_aimGuide)
            Debug.Log("There is no aim guide... Did you forget to set it?");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_aimGuide != null)
            m_aimRotation = m_aimGuide.rotation.eulerAngles;

        verticalAim();

        //Check if there's a horizontal axis in the first place
        if(m_horizontalAxis != null)
            horizontalAim();
	}

    void horizontalAim()
    {
        //Rotate based on aim guide orientation
        var aimDir = new Vector3(0, m_aimRotation.y - m_horizontalAxis.rotation.eulerAngles.y, 0);

        m_horizontalAxis.Rotate(aimDir);
    }

    void verticalAim()
    {
        //Initial rotation based on aim guide orientation
        var aimDir = new Vector3((m_aimRotation.x - m_verticalAxis.rotation.eulerAngles.x), 0, 0);

        m_verticalAxis.Rotate(aimDir);

        //Make this angle shit less annoying to deal with
        float vangle = m_verticalAxis.rotation.eulerAngles.x;
        float tangle = transform.rotation.eulerAngles.x;
        if (m_verticalAxis.rotation.eulerAngles.x > 180)
            vangle -= 360;
        if (transform.rotation.eulerAngles.x > 180)
            tangle -= 360;

        //Limit pitch minimum so the barrel doesn't clip through the tank body
        if (tangle - vangle < m_pitchMinimum)
           m_verticalAxis.Rotate(Vector3.right, (tangle - vangle) - m_pitchMinimum);
    }
}
