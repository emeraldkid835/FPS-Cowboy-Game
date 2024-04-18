using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
#pragma warning disable 649

    // CharacterController component for player movement
    [SerializeField] CharacterController controller;
    public bool isMoving = false;

    // Movement speed variables
    [SerializeField] float speed = 11f;
    [SerializeField] public float sprintSpeed = 17f;
    [SerializeField] public float IncreasedSprintSpeed = 25f;
    [SerializeField] float sprintAcceleration = 5f;
    [SerializeField] float stepInterval = 0.6f;
    [SerializeField] float sprintStepInterval = 0.3f;
    [SerializeField] List<AudioSource> stepSounds = new List<AudioSource>();
    float stepT;

    // Variables for handling slopes
    [SerializeField] float slopeForce = 5f;
    [SerializeField] float slopeRayLength = 0.5f;
    [SerializeField, Range(0, 100)] float slopeLimit = 30f;

    // Variables for wall running
    [SerializeField] float wallRunForce = 5f;
    [SerializeField] float wallRunHeightOffset = 2f;

    // Capsule dimensions for grounding check
    [SerializeField] float capsuleHeight = 2f;
    [SerializeField] float capsuleRadius = 0.5f;
    [SerializeField] float capsuleVerticalOffset = 0.25f;

    // Input variables
    Vector2 horizontalInput;

    // Jumping variables
    [SerializeField] float jumpHeight = 3.5f;
    [SerializeField] AudioSource jumpSound;
    [SerializeField] float gravity = -30f;
    [SerializeField] float groundedRadius = 0.2f;
    
    [SerializeField] float jumpGroundedRadius = 0.8f;
    [SerializeField] private bool ableToLandSound = false; //Serialized for debug
    [SerializeField] private float ableToLandSoundRequiredTime = 0.3f;
    private float ableToLandTimer = 0f;
    [SerializeField] List<string> tagsForSounds = new List<string>();
    [SerializeField] List<AudioSource> soundsForLand = new List<AudioSource>();

    Vector3 verticalVelocity = Vector3.zero;

    // Ground check using layer mask
    [SerializeField] LayerMask groundMask;
    [SerializeField] public bool isGrounded; // Serialized for debug

    // Jumping control variables
    bool canJump;
    bool jump;
    [SerializeField] int jumpAmount = 1;
    [SerializeField] int currentJump = 0; // Serialized for debug

    // Sprinting control variables
    public bool sprint;

    
    // Update is called once per frame
    private void Update()
    {
        HandleMovementInput();
    }

    // Method to handle player movement input
    void HandleMovementInput()
    {
        // Check if the player can jump based on a downward raycast
        canJump = Physics.Raycast(transform.position, Vector3.down, out RaycastHit jumpInfo, jumpGroundedRadius, groundMask);
        Debug.Log("Surface normal? :" + jumpInfo.normal);
        if (canJump && jumpInfo.normal.y >= (1 - (slopeLimit / 100)))
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
        isGrounded = Physics.CapsuleCast(transform.position + Vector3.up * capsuleVerticalOffset, transform.position + Vector3.up * capsuleHeight + Vector3.up * capsuleVerticalOffset, capsuleRadius, Vector3.down, out RaycastHit info, groundedRadius, groundMask);
        
        if(verticalVelocity.y < 0)
        {
            if (ableToLandTimer >= ableToLandSoundRequiredTime)
            {
                ableToLandSound = true;
            }
            else
            {
                ableToLandTimer += Time.deltaTime;
            }

            
        }
        
        if (isGrounded)
        {
            ableToLandTimer = 0f;
            if(ableToLandSound == true) //<- this is where I assume the jank lies for timing whether or not landing sound should play.
                                        //i assume grounded and jump can happen at the same time, causing bad sound timing.
            {
                for (int i = 0; i < tagsForSounds.Count; i++) //run through every tag that should have a sound
                {
                    if(info.collider.gameObject.tag == tagsForSounds[i] && soundsForLand[i] != null) //does the object have one of the tags?
                                                                                                        //and does the corresponding sound even exist?
                    {
                        audiomanager.instance.PlaySFX3D(soundsForLand[i].clip, this.transform.position, 0, 0.99f, 1.01f); //play the correct sound, 2D,
                                                                                                                          //with slight pitch variation
                        Debug.Log("Playing a landing sound");
                    }
                }
                ableToLandSound = false; //guarentee that it doesn't play more than once, hopefully.
            }
            if (verticalVelocity.y < 0)
            {
                verticalVelocity.y = 0;
            }
        }
        else
        {
            // Apply gravity when not grounded
            verticalVelocity.y += gravity * Time.deltaTime;
        }

        // Determine current speed based on sprinting status
        float currentSpeed = sprint ? sprintSpeed : speed;
        // Determine target speed based on sprinting status
        float targetSpeed = sprint ? sprintSpeed : speed;

        // Lerp the current speed towards the target speed
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, sprintAcceleration * Time.deltaTime);
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * currentSpeed;

        if(horizontalVelocity != Vector3.zero)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //do stepping noises lol, lmao (should probably have this in the if above, but fuck that)
        if (isGrounded == true && isMoving == true)
        {
            if(stepT == 0)
            {
                for(int i = 0; i < tagsForSounds.Count; i++) {
                    if (info.collider.gameObject.tag == tagsForSounds[i] && stepSounds[i] != null) 
                    {
                        Debug.Log("Step sound should happen now!");
                        audiomanager.instance.PlaySFX3D(stepSounds[i].clip, this.transform.position, 0.2f, 0.95f, 1.05f);
                    } 
                }
            }
            stepT += Time.deltaTime;
            float targetInterval = sprint ? sprintStepInterval : stepInterval;
            if(stepT >= targetInterval)
            {
                stepT = 0;
            }
        }

        
        // Check for slope
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, slopeRayLength, groundMask))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            if (slopeAngle > controller.slopeLimit)
            {
                // Apply slope force to slide down
                verticalVelocity += Vector3.down * slopeForce * Time.deltaTime;

                // Add wall run force only while sprinting
                if (slopeAngle < 90f)
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

        // Move the player horizontally
        controller.Move(horizontalVelocity * Time.deltaTime);

        // Handle jumping
        if (jump)
        {
            if (currentJump < jumpAmount)
            {
                
                // Set vertical velocity for jumping
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
                currentJump += 1;
                audiomanager.instance.PlaySFX3D(jumpSound.clip, this.transform.position, 0, 0.99f, 1.01f);

            }

            jump = false;
        }

        // Move the player vertically (apply gravity)
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    // Method to receive horizontal input from an external source
    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }

    // Method to handle sprinting input
    public void OnSprintPressed()
    {
        // Toggle sprinting on or off
        sprint = !sprint;
    }

    // Method to handle jump input
    public void OnJumpPressed()
    {
        jump = true;
    }

    public void increaseJumpAmount(int amount)
    {
        jumpAmount += amount;
    }

    private void Awake()
    {
        stepT = 0f;
        ableToLandTimer = 0f;
    }
}
