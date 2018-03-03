﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LightTankMovement : NetworkBehaviour {
    
    public LightTankMoveScriptable m_forces;
    public Transform m_steerGuide;

    Rigidbody m_self;

    //Hover and balance rays
    Ray ray, rayT, rayL, rayR;

    //Balance ray positions
    Vector3 rayTPos, rayLPos, rayRPos;

    //Networked input
    [SyncVar]
    public float m_hinput;
    [SyncVar]
    public float m_vinput;
    [SyncVar]
    public float m_jinput;
    [SyncVar]
    public Vector3 m_steerinput;

    void Start()
    {
        m_self = GetComponent<Rigidbody>();
        m_steerGuide = Camera.main.transform;
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        //Player movement
        #region Testing
        if (!GetComponent<NetworkTank>())
        {
            m_hinput = Input.GetAxis("Horizontal");
            m_vinput = Input.GetAxis("Vertical");
            m_jinput = Input.GetAxis("Jump");
            m_steerinput = m_steerGuide.forward;
        }
        #endregion //Delete later

        MovementLoop();
    }

    void MovementLoop()
    {
        //Are we over the ground?
        ray = new Ray(transform.position, -transform.up);
        RaycastHit ground;

        //Will balance to ground slope up to 5 units above ground
        if (Physics.Raycast(ray, out ground, m_forces.hoverHeight + 5))
        {
            HoverBalance();
            //Can move and steer up to 2 units above hover height
            if (m_jinput <= 0.99f && ground.distance <= m_forces.hoverHeight + 2)
            {
                Move();
                Steer();
                if (ground.distance <= m_forces.hoverHeight)
                    Hover(ground);

            }
        }
        else
            //Go back upright if we're higher than 5 units
            Rebalance();
    }

    void Hover(RaycastHit hit)
    {
        float dist = (m_forces.hoverHeight - hit.distance) / m_forces.hoverHeight;
        Vector3 force = new Vector3(0, 1 - (m_self.velocity.y / 8) + ((Mathf.Abs(m_self.velocity.x)
            + Mathf.Abs(m_self.velocity.z)) / 25), 0) * dist * m_forces.hoverForce * (-m_jinput + 1);

        m_self.AddRelativeForce(force, ForceMode.Acceleration);
    }

    void HoverBalance()
    {
        rayT = new Ray(transform.position + new Vector3(0, 0, 2), Vector3.down);
        rayL = new Ray(transform.position - new Vector3(2, 0, 0), Vector3.down);
        rayR = new Ray(transform.position + new Vector3(2, 0, 0), Vector3.down);

        RaycastHit balT;
        RaycastHit balL;
        RaycastHit balR;

        //If all raycasts are hitting
        if(Physics.Raycast(rayT, out balT, m_forces.hoverHeight + 2) 
        && Physics.Raycast(rayL, out balL, m_forces.hoverHeight + 2) 
        && Physics.Raycast(rayR, out balR, m_forces.hoverHeight + 2))
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

    //Turn to match the camera's orientation
    void Steer()
    {
        var steerdir = Quaternion.FromToRotation(transform.forward, m_steerinput);
        m_self.AddRelativeTorque(new Vector3(0, (steerdir.y - (m_self.angularVelocity.y / 20))
            * (-m_jinput + 1), 0) * 75, ForceMode.Impulse);
    }

    void Move()
    {
        //We need to convert input direction to world space
        Vector3 dir = new Vector3(m_hinput, 0, m_vinput).normalized;
        Vector3 relDir = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, transform.up) * dir;

        //If we have input, move
        if (dir != Vector3.zero)
        {
            //Main driving force
            m_self.AddRelativeForce(dir * m_forces.moveForce * m_self.mass * (-m_jinput + 1), ForceMode.Force);
            //This next force assists with sharp turns and strafing 
            //but it may be *too* responsive for a hovercraft...
            //self.AddRelativeForce(dir * forces.moveForce * Mathf.Max(0, Vector3.Dot(-self.velocity
                //.normalized, relDir)) * self.mass / 3);
            m_self.AddForce(-m_self.velocity * 7.5f);
        }
        //If not, slow down
        else if (dir == Vector3.zero)
            m_self.AddForce(-m_self.velocity / 3 * m_self.mass);
    }

    //Stay upright
    void Rebalance()
    {
        var rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        m_self.AddTorque(new Vector3(rot.x, rot.y, rot.z) * 200);
    }
}
