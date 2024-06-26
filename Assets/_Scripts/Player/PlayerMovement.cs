using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private float playerWalkSpeed = 5.0f, playerRunSpeed = 8;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;


    private Vector3 playerVelocity;

    [Header("Grounded check parameters:")]
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float rayDistance = 1;
    [field: SerializeField]
    public bool IsGrounded { get; private set; }



    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private Vector3 GetMovementDirection(Vector3 movementInput)
    {
        return transform.right * movementInput.normalized.x + transform.forward * movementInput.normalized.z;
    }

    public void Walk(Vector3 movementInput, bool runningInput)
    {
        Vector3 movementDirection = GetMovementDirection(movementInput);
        //If running input is true, use playerRunSpeed, if false, use playerWalkSpeed
        float speed = runningInput ? playerRunSpeed : playerWalkSpeed;
        controller.Move(movementDirection * Time.deltaTime * speed);
    }

    public void HandleGravity(bool isJumping)
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        if (isJumping && IsGrounded)
            AddJumpForce();
        ApplyGravityForce();
        controller.Move(playerVelocity * Time.deltaTime);
    }
    private void AddJumpForce()
    {
        playerVelocity.y = jumpHeight;
    }

    private void ApplyGravityForce()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        playerVelocity.y = Mathf.Clamp(playerVelocity.y, gravityValue, 10);
    }

    private void Update()
    {
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance, groundMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * rayDistance);
    }
}
