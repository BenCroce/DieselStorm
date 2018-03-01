using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LightTankMovement : MonoBehaviour {
    
    public LightTankMoveScriptable forces;

    public ParticleSystem trail;

    Rigidbody self;

    private float MouseAimX;

    //Hover and balance rays
    Ray ray, rayT, rayL, rayR;

    //These variables are here mainly for networking stuff
    public float hinput;
    public float vinput;
    public float jinput;
    public Vector3 mcam;

    public int updatesPerSecond = 10;

    void Start()
    {
        self = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MouseAimX += Input.GetAxis("Mouse X");
    }

    private void FixedUpdate()
    {
        //Player movement
        #region Testing
        if (!GetComponent<NetworkTank>())
        {
            hinput = Input.GetAxis("Horizontal");
            vinput = Input.GetAxis("Vertical");
            jinput = Input.GetAxis("Jump");
            mcam = GameObject.FindGameObjectWithTag("MainCamera").transform.forward;
        }
        #endregion //Delete later


        //Are we over the ground?
        ray = new Ray(transform.position, -transform.up);
        RaycastHit ground;

        //Will balance to ground slope up to 5 units above ground
        if (Physics.Raycast(ray, out ground, forces.hoverHeight + 5))
        {
            HoverBalance();
            //Can move and steer up to 2 units above hover height
            if (jinput <= 0.99f && ground.distance <= forces.hoverHeight + 2)
            {
                Move();
                Steer();
                trail.enableEmission = true;
                if (ground.distance <= forces.hoverHeight)
                    Hover(ground);

            }
            else
                //Turn the trail off if we aren't on the ground
                trail.enableEmission = false;
        }
        else
            //go back upright if we're higher than 5 units
            Rebalance();
    }

    void Hover(RaycastHit hit)
    {
        float dist = (forces.hoverHeight - hit.distance) / forces.hoverHeight;
        Vector3 force = new Vector3(0, 1 - (self.velocity.y / 8) + ((Mathf.Abs(self.velocity.x)
            + Mathf.Abs(self.velocity.z)) / 25), 0) * dist * forces.hoverForce * (-jinput + 1);

        self.AddRelativeForce(force, ForceMode.Acceleration);
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
        if(Physics.Raycast(rayT, out balT, forces.hoverHeight + 2) 
        && Physics.Raycast(rayL, out balL, forces.hoverHeight + 2) 
        && Physics.Raycast(rayR, out balR, forces.hoverHeight + 2))
        {
            //Get the average normal between the three raycast hits
            Vector3 norm = Vector3.Normalize(balT.normal + balL.normal + balR.normal);

            //Balance the tank to match the ground slope orientation
            var bal = Quaternion.FromToRotation(transform.up, norm);
            self.AddTorque(new Vector3(bal.x - 
                (self.angularVelocity.x / 20), bal.y - 
                (self.angularVelocity.y / 20), bal.z - 
                (self.angularVelocity.z / 20)) * 60, 
                ForceMode.Acceleration);
        }

    }

    //Turn to match the camera's orientation
    void Steer()
    {
        var steerdir = Quaternion.FromToRotation(transform.forward, mcam);
        self.AddRelativeTorque(new Vector3(0, (steerdir.y - (self.angularVelocity.y / 20))
            * (-jinput + 1), 0) * 75, ForceMode.Impulse);
    }

    void Move()
    {
        //We need to convert input direction to world space
        Vector3 dir = new Vector3(hinput, 0, vinput).normalized;
        Vector3 relDir = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, transform.up) * dir;

        //If we have input, move
        if (dir != Vector3.zero)
        {
            self.AddRelativeForce(dir * forces.moveForce * self.mass * (-jinput + 1), ForceMode.Force);
            //self.AddRelativeForce(dir * moveForce * Mathf.Max(0, Vector3.Dot(-self.velocity.normalized, relDir)) * self.mass / 2);
            self.AddForce(-self.velocity * 7.5f);
        }
        //If not, slow down
        else if (dir == Vector3.zero)
            self.AddForce(-self.velocity / 3 * self.mass);
    }

    //Stay upright
    void Rebalance()
    {
        var rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        self.AddTorque(new Vector3(rot.x, rot.y, rot.z) * 200);
    }
}
