using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    [SerializeField] private CharacterController Controller = null;
    [SerializeField] private float speed = 5f;
    void Update()
    {
        if (!isLocalPlayer) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move_ = transform.right * x + transform.forward * z;
        Controller.Move(move_ * speed * Time.deltaTime);
    }
}
