using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSound : MonoBehaviour {
    
    public AudioSource engine;
    public AudioSource jet;

    Rigidbody self;
    LightTankMovement move;

	// Use this for initialization
	void Start ()
    {
        self = GetComponentInParent<Rigidbody>();
        move = GetComponentInParent<LightTankMovement>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        engine.pitch = 0.7f + self.velocity.magnitude / 50 + (-move.jinput + 1) * 0.2f;
        engine.volume = 0.8f - move.jinput * 0.6f;
        jet.pitch = (0.8f + self.velocity.magnitude / 25) * (-move.jinput + 1);

    }
}
