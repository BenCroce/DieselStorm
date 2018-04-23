using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NetworkHoverBehaviour : MonoBehaviour {

    public HoverScriptable m_hoverValues;
    public NetworkTankInputController m_input;

    //Hover and balance rays
    Ray m_ray, m_rayT, m_rayL, m_rayR;

    //Balance ray positions
    Vector3 m_rayTPos, m_rayLPos, m_rayRPos;

    Rigidbody m_self;

    // Use this for initialization
    void Start()
    {
        //Define rigidbody
        m_self = GetComponent<Rigidbody>();

        //Set ray positions
        m_rayTPos = Vector3.forward * m_hoverValues.m_hoverBalanceOffset;
        m_rayLPos = Vector3.Normalize(Vector3.left + Vector3.back) * m_hoverValues.m_hoverBalanceOffset;
        m_rayRPos = Vector3.Normalize(Vector3.right + Vector3.back) * m_hoverValues.m_hoverBalanceOffset;

        if (!m_input)
            Debug.LogWarning("There is no controller set! Hover on/off switching is disabled.");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Are we over the ground?
        m_ray = new Ray(transform.position, -transform.up);
        RaycastHit ground;

        //Will balance to ground slope up to 5 units above ground
        if (m_input.Jinput <= 0.99f && Physics.Raycast(m_ray, out ground, m_hoverValues.m_hoverHeight + 5))
        {
            HoverBalance();
            //Can move and steer up to 2 units above hover height
            if (ground.distance <= m_hoverValues.m_hoverHeight + 2)
            {
                if (ground.distance <= m_hoverValues.m_hoverHeight)
                {
                    Hover(ground, 0);

                    if (m_input != null)
                        Hover(ground, m_input.Jinput);
                }
            }
        }
        else
            //Go back upright if we're higher than 5 units
            Rebalance();
    }

    void HoverBalance()
    {
        m_rayT = new Ray(transform.position + m_rayTPos, Vector3.down);
        m_rayL = new Ray(transform.position + m_rayLPos, Vector3.down);
        m_rayR = new Ray(transform.position + m_rayRPos, Vector3.down);

        RaycastHit balT;
        RaycastHit balL;
        RaycastHit balR;

        //If all raycasts are hitting
        if (Physics.Raycast(m_rayT, out balT, m_hoverValues.m_hoverHeight + 2)
        && Physics.Raycast(m_rayL, out balL, m_hoverValues.m_hoverHeight + 2)
        && Physics.Raycast(m_rayR, out balR, m_hoverValues.m_hoverHeight + 2))
        {
            //Get the average normal between the three raycast hits
            Vector3 norm = Vector3.Normalize(balT.normal + balL.normal + balR.normal);

            //Balance the tank to match the ground slope orientation
            var bal = Quaternion.FromToRotation(transform.up, norm);
            m_self.AddTorque(new Vector3(bal.x -
                (m_self.angularVelocity.x / 20), bal.y -
                (m_self.angularVelocity.y / 20), bal.z -
                (m_self.angularVelocity.z / 20)) * 60,
                ForceMode.Acceleration);
        }

    }

    void Hover(RaycastHit hit, float m_jinput)
    {
        float dist = (m_hoverValues.m_hoverHeight - hit.distance) / m_hoverValues.m_hoverHeight;
        Vector3 force = new Vector3(0, 1 - (m_self.velocity.y / 8) + ((Mathf.Abs(m_self.velocity.x)
            + Mathf.Abs(m_self.velocity.z)) / 25), 0) * dist * m_hoverValues.m_hoverForce * (-m_jinput + 1);

        m_self.AddRelativeForce(force, ForceMode.Acceleration);
    }

    //Stay upright
    void Rebalance()
    {
        Quaternion rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        m_self.AddTorque(new Vector3(rot.x, rot.y, rot.z) * 200);
    }
}
