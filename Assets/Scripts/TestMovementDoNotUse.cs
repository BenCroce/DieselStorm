using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovementDoNotUse : MonoBehaviour
{
    public string Horizontal = "Horizontal";
    public string Vertical = "Vertical";
    public string Jump;
    public float Speed = 10;
    public float _currentSpeed;
    public float gravity = 20.0F;

    private Vector3 moveDirection;
    private CharacterController controller;
    public Vector3 targetDir;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            var h = Input.GetAxis(Horizontal);
            var v = Input.GetAxis(Vertical);
            var forward = Camera.main.transform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;
            ///such copy paste but it works
            var right = new Vector3(forward.z, 0, -forward.x);
            targetDir = h * right + v * forward;
            if (targetDir.magnitude > 0)
                transform.rotation = Quaternion.LookRotation(targetDir);

            moveDirection = targetDir;
        }

        controller.SimpleMove(moveDirection * Speed);
    }
}