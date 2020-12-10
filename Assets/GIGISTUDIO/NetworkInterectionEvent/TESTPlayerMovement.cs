using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTPlayerMovement : NetworkBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float turnSensitivity = 5f;
    public float maxTurnSpeed = 150f;

    [Header("Diagnostics")]
    public float horizontal;
    public float vertical;
    public float turn;
    public float jumpSpeed;
    public bool isGrounded = true;
    public bool isFalling;
    public Vector3 velocity;

    public Transform moveTarget;
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        // Q and E cancel each other out, reducing the turn to zero
        if (Input.GetKey(KeyCode.W))
            moveTarget.Translate(Vector3.forward * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
            moveTarget.Translate(Vector3.left * Time.deltaTime);

        if (Input.GetKey(KeyCode.D))
            moveTarget.Translate(Vector3.right * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            moveTarget.Translate(Vector3.back * Time.deltaTime);

        if (Input.GetKey(KeyCode.Q))
            moveTarget.Translate(Vector3.up * Time.deltaTime);

        if (Input.GetKey(KeyCode.E))
            moveTarget.Translate(Vector3.down * Time.deltaTime);
        if (Input.GetKey(KeyCode.Z))
            moveTarget.Rotate(Vector3.up * (Time.deltaTime * -30));
        if (Input.GetKey(KeyCode.X))
            moveTarget.Rotate(Vector3.up * (Time.deltaTime * 30));
    }

   
}
