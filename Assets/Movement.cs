using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    [SerializeField] private CharacterController Controller = null;
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject feet = null;
    [SerializeField] private LayerMask GroundMask = new LayerMask();
    [SerializeField] private float jumpHeight = 2f;
    float checkRadius = 0.4f;
    float gravity_ = -9.81f;
    Vector3 velocity;
    [SerializeField] private Animator FPSAnimator = null, TPAnimator = null;

    void Update()
    {
        if (!isLocalPlayer) return;

        bool isGrounded = Physics.CheckSphere(feet.transform.position, 0.4f, GroundMask);

        if (isGrounded && velocity.y <= 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (x != 0 || z != 0)
        {
            FPSAnimator.SetBool("Walking", true);
            TPAnimator.SetBool("Walking", true);
        }
        else
        {
            FPSAnimator.SetBool("Walking", false);
            TPAnimator.SetBool("Walking", false);
        }
        Vector3 move_ = transform.right * x + transform.forward * z;
        Controller.Move(move_ * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {
            if (!isGrounded) return;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity_);
        }
        velocity.y += gravity_ * Time.deltaTime;
        Controller.Move(velocity * Time.deltaTime);
    }
}
