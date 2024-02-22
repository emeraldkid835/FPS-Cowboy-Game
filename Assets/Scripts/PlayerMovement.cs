using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
#pragma warning disable 649

    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 11f;
    [SerializeField] float sprintSpeed = 17f;
    [SerializeField] float slopeForce = 5f;
    [SerializeField] float slopeRayLength = 0.5f;
    [SerializeField] float wallRunForce = 5f; // Added wall run force
    [SerializeField] float wallRunHeightOffset = 2f; // Adjust this value
    [SerializeField] float capsuleHeight = 2f; // Adjust this value
    [SerializeField] float capsuleRadius = 0.5f; // Adjust this value

    Vector2 horizontalInput;

    [SerializeField] float jumpHeight = 3.5f;
    [SerializeField] float gravity = -30f; // -9.81
    [SerializeField] float groundedRadius = 0.1f; // Added this to change the radius to detect if grounded while in the editor
    [SerializeField] float jumpGroundedRadius = 0.4f;
    Vector3 verticalVelocity = Vector3.zero;

    [SerializeField] LayerMask groundMask;

    [SerializeField] bool isGrounded; // Serialized for debug
    bool canJump;
    bool jump;
    [SerializeField] int jumpAmount = 2;
    [SerializeField] int currentJump = 0; // Serialized for debug
    bool sprint;


    private void Update()
    {
        HandleMovementInput();
    }

    void HandleMovementInput()
    {
        canJump = Physics.Raycast(transform.position, Vector3.down, jumpGroundedRadius, groundMask);
        if (canJump)
        {
            currentJump = 0;
        }
        else
        {
            if (currentJump == 0)
            {
                currentJump += 1;
            }
        }

        // Check grounding with capsule cast
        isGrounded = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * capsuleHeight, capsuleRadius, Vector3.down, groundedRadius, groundMask);
        if (isGrounded)
        {
            if (verticalVelocity.y < 0)
            {
                verticalVelocity.y = 0;
            }
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }

        float currentSpeed = sprint ? sprintSpeed : speed;
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * currentSpeed;

        // Check for slope
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, slopeRayLength, groundMask))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            if (slopeAngle > controller.slopeLimit)
            {
                // Apply slope force to slide down
                verticalVelocity += Vector3.down * slopeForce * Time.deltaTime;

                // Add wall run force only while sprinting
                if (sprint && slopeAngle < 90f)
                {
                    // Calculate the direction of the wall run force
                    Vector3 wallRunForceDirection = Vector3.Cross(hit.normal, Vector3.up).normalized;

                    // Apply wall run force
                    controller.Move(wallRunForceDirection * wallRunForce * Time.deltaTime);

                    // Adjust vertical position to lose height while running across the wall
                    Vector3 wallRunHeightPosition = transform.position;
                    wallRunHeightPosition.y = Mathf.Lerp(wallRunHeightPosition.y, hit.point.y - wallRunHeightOffset, 0.1f);
                    transform.position = wallRunHeightPosition;
                }
            }
        }

        controller.Move(horizontalVelocity * Time.deltaTime);

        if (jump)
        {
            if (currentJump < jumpAmount)
            {
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
                currentJump += 1;
            }

            jump = false;
        }

        
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    public void ReceiveInput (Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }

    public void OnSprintPressed()
    {
        if (!sprint && !jump)
        {
            sprint = true;
        }
        else
        {
            sprint = false;
        }
        
    }

    public void OnJumpPressed()
    {
        jump = true;
    }
}
