using UnityEngine;

public class LightTankMovement : MovementBehaviour
{
    
    public LightTankMoveScriptable m_forces;
    public Transform m_steerGuide;

    Rigidbody m_self;

    //Hover and balance rays
    Ray m_ray, m_rayT, m_rayL, m_rayR;

    //Balance ray positions
    public Vector3 m_rayTPos, m_rayLPos, m_rayRPos;
   
    void Start()
    {
        m_self = GetComponent<Rigidbody>();
        if (m_steerGuide == null)
            Debug.LogWarning("you must have a steerguide, this is usually the camera... did you forget to assign it?");
    }
 
    void MovementLoop(float m_hinput, float m_vinput,float m_jinput, Vector3 m_steerinput)
    {
        //Are we over the ground?
        m_ray = new Ray(transform.position, -transform.up);
        RaycastHit ground;

        //Will balance to ground slope up to 5 units above ground
        if (Physics.Raycast(m_ray, out ground, m_forces.m_hoverHeight + 5))
        {
            HoverBalance();
            //Can move and steer up to 2 units above hover height
            if (m_jinput <= 0.99f && ground.distance <= m_forces.m_hoverHeight + 2)
            {
                Move(m_hinput, m_vinput, m_jinput);
                Steer(m_steerinput, m_jinput);
                if (ground.distance <= m_forces.m_hoverHeight)
                    Hover(ground, m_jinput);

            }
        }
        else
            //Go back upright if we're higher than 5 units
            Rebalance();
    }

    void Hover(RaycastHit hit, float m_jinput)
    {
        float dist = (m_forces.m_hoverHeight - hit.distance) / m_forces.m_hoverHeight;
        Vector3 force = new Vector3(0, 1 - (m_self.velocity.y / 8) + ((Mathf.Abs(m_self.velocity.x)
            + Mathf.Abs(m_self.velocity.z)) / 25), 0) * dist * m_forces.m_hoverForce * (-m_jinput + 1);

        m_self.AddRelativeForce(force, ForceMode.Acceleration);
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
        if(Physics.Raycast(m_rayT, out balT, m_forces.m_hoverHeight + 2) 
        && Physics.Raycast(m_rayL, out balL, m_forces.m_hoverHeight + 2) 
        && Physics.Raycast(m_rayR, out balR, m_forces.m_hoverHeight + 2))
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
    void Steer(Vector3 m_steerinput, float m_jinput)
    {
        Quaternion steerdir = Quaternion.FromToRotation(transform.forward, m_steerinput);
        m_self.AddRelativeTorque(new Vector3(0, (steerdir.y - (m_self.angularVelocity.y / 25))
            * (-m_jinput + 1), 0) * m_forces.m_steerSpeed, ForceMode.Impulse);
    }

    public override void Move(float m_hinput, float m_vinput, float m_jinput)
    {
        //We need to convert input direction to world space
        var dir = new Vector3(m_hinput, 0, m_vinput).normalized;
        var relDir = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, transform.up) * dir;

        //If we have input, move
        if (dir != Vector3.zero)
        {
            //Main driving force
            m_self.AddRelativeForce(dir * m_forces.m_moveForce * m_self.mass * (-m_jinput + 1), ForceMode.Force);
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
        Quaternion rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        m_self.AddTorque(new Vector3(rot.x, rot.y, rot.z) * 200);
    }
}
