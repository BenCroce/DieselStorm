using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSound : MonoBehaviour {
    
    public AudioSource engine;
    public AudioSource jet;

    Rigidbody self;
    NetworkTankInputController move;

	// Use this for initialization
	void Start ()
    {
        self = GetComponentInParent<Rigidbody>();
        move = GetComponentInParent<NetworkTankInputController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        engine.pitch = 0.6f + self.velocity.magnitude / 25 + (-move.Jinput + 1) * 0.2f;
        engine.volume = 0.6f - move.Jinput * 0.6f;
        jet.pitch = (0.6f + self.velocity.magnitude / 50) * (-move.Jinput + 1);
    }
}
