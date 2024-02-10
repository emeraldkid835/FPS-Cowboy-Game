using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
#pragma warning disable 649

    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 11f;
    [SerializeField] float sprintSpeed = 17f;

    Vector2 horizontalInput;

    [SerializeField] float jumpHeight = 3.5f;
    [SerializeField] float gravity = -30f; // -9.81
    [SerializeField] float groundedRadius = 0.1f; // Added this to change the radius to detect if grounded while in the editor
    Vector3 verticalVelocity = Vector3.zero;

    [SerializeField] LayerMask groundMask;

    [SerializeField] bool isGrounded; // Serialized for debug
    bool jump;
    [SerializeField] int jumpAmount = 2;
    [SerializeField] int currentJump = 0; // Serialized for debug
    bool sprint;


    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundedRadius, groundMask); // Grounded check feels iffy - Logan
        if (isGrounded)
        {
            verticalVelocity.y = 0;
            currentJump = 0;
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime;

            if (currentJump == 0)
            {
                currentJump += 1;
            }
        }

        float currentSpeed = sprint ? sprintSpeed : speed;
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * currentSpeed;
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
