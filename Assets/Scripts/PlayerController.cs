using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float wallClimbForce = 2f;

    private Vector2 movementInput;
    private bool sprinting;
    private bool isGrounded;
    private bool isWallClimbing;

    private Rigidbody rb;

    public PlayerControls playerControls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        // Enable the input actions
        playerControls.Gameplay.Move.Enable();
        playerControls.Gameplay.Jump.Enable();
        playerControls.Gameplay.Sprint.Enable();
    }

    private void OnDisable()
    {
        // Disable the input actions when the script is disabled
        playerControls.Gameplay.Move.Disable();
        playerControls.Gameplay.Jump.Disable();
        playerControls.Gameplay.Sprint.Disable();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleMouseLook();
        HandleJump();
        HandleSprint();
        HandleWallClimb();
    }

    private void HandleMovementInput()
    {
        Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        float speed = sprinting ? sprintSpeed : walkSpeed;
        Vector3 moveAmount = moveDirection * speed * Time.deltaTime;
        transform.Translate(moveAmount);
    }

    private void HandleMouseLook()
    {
        Vector2 mouseDelta = playerControls.Gameplay.Look.ReadValue<Vector2>();
        float mouseX = mouseDelta.x;
        float mouseY = mouseDelta.y;

        transform.Rotate(Vector3.up * mouseX);

        float currentRotationX = Camera.main.transform.rotation.eulerAngles.x;
        float newRotationX = Mathf.Clamp(currentRotationX - mouseY, -90f, 90f);
        Camera.main.transform.rotation = Quaternion.Euler(newRotationX, transform.rotation.eulerAngles.y, 0f);
    }

    private void HandleSprint()
    {
        sprinting = playerControls.Gameplay.Sprint.triggered;
    }

    private void HandleJump()
    {
        if (isGrounded && playerControls.Gameplay.Jump.triggered)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleWallClimb()
    {
        if (isWallClimbing)
        {
            Vector3 wallClimbVelocity = Vector3.up * wallClimbForce;
            rb.velocity = wallClimbVelocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        isWallClimbing = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            isWallClimbing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            isWallClimbing = false;
        }
    }
}

