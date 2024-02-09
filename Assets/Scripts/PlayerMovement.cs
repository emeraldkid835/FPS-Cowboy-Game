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
    Vector3 verticalVelocity = Vector3.zero;

    [SerializeField] LayerMask groundMask;

    bool isGrounded;
    bool jump;
    bool sprint;


    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);
        if (isGrounded)
        {
            verticalVelocity.y = 0;
        }

        float currentSpeed = sprint ? sprintSpeed : speed;
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * currentSpeed;
        controller.Move(horizontalVelocity * Time.deltaTime);

        if (jump)
        {
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }

            jump = false;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
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
