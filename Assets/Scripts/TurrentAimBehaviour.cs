using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentAimBehaviour : MonoBehaviour {

    public Transform m_aimGuide;
    public Transform m_verticalAxis;
    public Transform m_horizontalAxis;

	// Use this for initialization
	void Start ()
    {
        m_aimGuide = Camera.main.transform;
        if (!m_aimGuide)
            Debug.Log("There is no aim guide... Did you forget to set it?");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_aimGuide)
            return;

        verticalAim();

        if(m_horizontalAxis != null)
            horizontalAim();
	}

    void horizontalAim()
    {
        var aimDir = new Vector3(0, m_aimGuide.rotation.eulerAngles.y - m_horizontalAxis.rotation.eulerAngles.y, 0);

        m_horizontalAxis.Rotate(aimDir);
    }

    void verticalAim()
    {
        var aimDir = new Vector3((m_aimGuide.rotation.eulerAngles.x - m_verticalAxis.rotation.eulerAngles.x), 0, 0);

        m_verticalAxis.Rotate(aimDir);
    }
}
