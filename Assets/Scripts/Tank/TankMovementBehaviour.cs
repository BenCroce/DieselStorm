using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovementBehaviour : MovementBehaviour {

    public TankMoveScriptable m_forces;
    public Transform m_steerGuide;

    Rigidbody m_self;

    //Ground distance check
    Ray m_ray;

    void Start()
    {
        m_self = GetComponent<Rigidbody>();
        if (m_steerGuide == null)
            Debug.LogWarning("you must have a steerguide, this is usually the camera... did you forget to assign it?");
    }

    //Turn to match the camera's orientation
    void Steer(Transform m_steerinput, float m_jinput)
    {
        Quaternion steerdir = Quaternion.FromToRotation(transform.forward, m_steerinput.forward);
        m_self.AddRelativeTorque(new Vector3(0, (steerdir.y - (m_self.angularVelocity.y / Mathf.Max(8, m_forces.m_turnSpeed/10)))
            * (-m_jinput + 1), 0) * m_forces.m_turnSpeed, ForceMode.Impulse);
    }

    public override void Move(float m_hinput, float m_vinput, float m_jinput)
    {
        //Are we over the ground?
        m_ray = new Ray(transform.position, -transform.up);
        RaycastHit ground;

        //Can move and steer as long as under height limit
        if (Physics.Raycast(m_ray, out ground, m_forces.m_heightLimit) && m_jinput <= 0.99f)
        {
            //TANK MOVEMENT//
            var dir = new Vector3(m_hinput, 0, m_vinput).normalized;
            //Apply stats and modifiers to movement force
            var dirStat = new Vector3(dir.x * m_forces.m_strafeForce, 0, dir.z * m_forces.m_forwardForce);
            var relDir = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, transform.up) * dir;

            //If we have input, move
            if (dir != Vector3.zero)
            {
                //Main driving force
                m_self.AddRelativeForce(dirStat * m_self.mass * (-m_jinput + 1), ForceMode.Force);
                m_self.AddForce(-m_self.velocity * 7.5f);
            }
            //If not, slow down
            else if (dir == Vector3.zero)
                m_self.AddForce(-m_self.velocity / 3 * m_self.mass);


            //TANK TURNING//
            Steer(m_steerGuide, m_jinput);
        }
    }

}
