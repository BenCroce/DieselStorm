using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(LightTankMovement))]
public class NetworkTank : NetworkBehaviour {

    LightTankMovement self;

    public int updatesPerSecond = 60;

	// Use this for initialization
	void Start ()
    {
        self = GetComponent<LightTankMovement>();
        StartCoroutine(InputSync());
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		if (isLocalPlayer)
        {
            self.hinput = Input.GetAxis("Horizontal");
            self.vinput = Input.GetAxis("Vertical");
            self.jinput = Input.GetAxis("Jump");
            self.steerinput = self.steerGuide.forward;
        }
	}

    //Send out input information to other players
    [Command]
    void CmdInput(float h, float v, float j, Vector3 m)
    {
        RpcInput(h, v, j, m);
    }

    //Update input variables from other players
    [ClientRpc]
    void RpcInput(float h, float v, float j, Vector3 m)
    {
        if (!isLocalPlayer)
        {
            self.hinput = h;
            self.vinput = v;
            self.jinput = j;
            self.steerinput = m;
        }
    }

    //X times per second, send input information from the local player to the rest of the players
    IEnumerator InputSync()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / updatesPerSecond);
            if (isLocalPlayer)
                CmdInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),
                    Input.GetAxis("Jump"), self.steerGuide.forward);
        }
    }
}
 