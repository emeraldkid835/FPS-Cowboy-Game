using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public InputManager inputManager;

    public Rigidbody rb;
    public CapsuleCollider playerHeight;

    public float speed = 10f;
    public float runSpeed = 15;

    public float jumpForce = 200f;

    private bool _isGrounded;

    

    //public float maxSlopeAngle;
    //private RaycastHit slopeHit;

    void Start()
    {
        inputManager.inputMaster.Movement.Jump.started += _ => Jump();
        rb = GetComponent<Rigidbody>();
        //rb.freezeRotation = true;
        
    }

    private void Update()
    {
        MovementInput();
    }   


    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
    
    public void MovementInput()
    {
        float forward = inputManager.inputMaster.Movement.Forward.ReadValue<float>();
        float right = inputManager.inputMaster.Movement.Right.ReadValue<float>();
        Vector3 move = transform.right * right + transform.forward * forward;

        move *= inputManager.inputMaster.Movement.Run.ReadValue<float>() == 0 ? speed : runSpeed;
        
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
    }

    void Jump()
    {
        if (_isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
    }

    /*private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight.height * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }*/
}
