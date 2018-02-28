using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankMovement : NetworkBehaviour {

    public float moveForce = 20f;
    public float hoverForce = 50f;
    public float hoverHeight = 1f;

    public ParticleSystem trail;

    Rigidbody self;
    private float MouseAimX;
    Ray ray, rayT, rayL, rayR, rayB;

    //Networking player movement
    [SyncVar]
    public float hinput;
    [SyncVar]
    public float vinput;
    [SyncVar]
    public float jinput;
    [SyncVar]
    private Vector3 mcam;

    void Start()
    {
        self = GetComponent<Rigidbody>();
        StartCoroutine(InputSync());
    }

    void Update()
    {
        MouseAimX += Input.GetAxis("Mouse X");
    }

    private void FixedUpdate()
    {
        //Player movement
        if (isLocalPlayer)
        {
            hinput = Input.GetAxis("Horizontal");
            vinput = Input.GetAxis("Vertical");
            jinput = Input.GetAxis("Jump");
            mcam = GameObject.FindGameObjectWithTag("MainCamera").transform.forward;
        }

        //Are we over the ground?
        ray = new Ray(transform.position, -transform.up);
        RaycastHit ground;

        //Will balance to ground slope up to 5 units above ground
        if (Physics.Raycast(ray, out ground, hoverHeight + 7))
        {
            HoverBalance();
            //Can move and steer up to 2 units above hover height
            if (jinput <= 0.99f && ground.distance <= hoverHeight + 2)
            {
                Move();
                Steer();
                trail.enableEmission = true;
                if (ground.distance <= hoverHeight)
                    Hover(ground);

            }
            else
                trail.enableEmission = false;
        }
        else
            Rebalance();
    }

    void Hover(RaycastHit hit)
    {
        float dist = (hoverHeight - hit.distance) / hoverHeight;
        Vector3 force = new Vector3(0, 1 - (self.velocity.y / 8) + ((Mathf.Abs(self.velocity.x)
            + Mathf.Abs(self.velocity.z)) / 25), 0) * dist * hoverForce * (-jinput + 1);

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
        if(Physics.Raycast(rayT, out balT, hoverHeight + 2) 
        && Physics.Raycast(rayL, out balL, hoverHeight + 2) 
        && Physics.Raycast(rayR, out balR, hoverHeight + 2))
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

    void Steer()
    {
        var steerdir = Quaternion.FromToRotation(transform.forward, mcam);
        self.AddRelativeTorque(new Vector3(0, (steerdir.y - (self.angularVelocity.y / 20)) * (-jinput + 1), 0) * 75, ForceMode.Impulse);
    }

    void Move()
    {
        //We need to convert input direction to world space
        Vector3 dir = new Vector3(hinput, 0, vinput).normalized;
        Vector3 relDir = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, transform.up) * dir;

        //If we have input, move
        if (dir != Vector3.zero)
        {
            self.AddRelativeForce(dir * moveForce * self.mass * (-jinput + 1), ForceMode.Force);
            //self.AddRelativeForce(dir * moveForce * Mathf.Max(0, Vector3.Dot(-self.velocity.normalized, relDir)) * self.mass / 2);
            self.AddForce(-self.velocity * 7.5f);
        }
        //If not, slow down
        else if (dir == Vector3.zero)
            self.AddForce(-self.velocity / 3 * self.mass);
    }

    void Rebalance()
    {
        var rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        self.AddTorque(new Vector3(rot.x, rot.y, rot.z) * 200);
    }

    [Command]
    void CmdInput(float h, float v, float j, Vector3 m)
    {
        RpcInput(h,v,j,m);
    }

    [ClientRpc]
    void RpcInput(float h, float v, float j, Vector3 m)
    {
        if(!isLocalPlayer)
        {
            hinput = h;
            vinput = v;
            jinput = j;
            mcam = m;
        }
    }

    IEnumerator InputSync()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (isLocalPlayer)
                CmdInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Jump"), GameObject.FindGameObjectWithTag("MainCamera").transform.forward);
        }
    }
}
