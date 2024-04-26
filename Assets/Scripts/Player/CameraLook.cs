using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLook : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    private PlayerPause pauser;

    [SerializeField] float sensitivityX = 8f;
    [SerializeField] float sensitivityY = 0.5f;
    float mouseX, mouseY;

    [SerializeField] Transform playerCamera;
    [SerializeField] float xClamp = 85f;
    float xRotation = 0f;

    //Head bobbing stuff
    [SerializeField] float headBobFrequency = 2f;
    [SerializeField] float headBobAmplitude = 0.1f;

    [SerializeField] float lateralBobFrequency = 5f; // Adjust the frequency of lateral head bobbing while sprinting
    [SerializeField] float lateralBobAmplitude = 0.05f;

    float headBobOffset = 2f;

    [SerializeField] Slider sensitivityXSlider;
    [SerializeField] Slider sensitivityYSlider;

    Vector3 originalCameraPosition;

    [SerializeField] private float maxInteractionDistance = 1f;
    [SerializeField] private Transform interactShooterPoint;
    [SerializeField] private GameObject interactUI;

    private bool validInteract;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        validInteract = false;
        interactUI.SetActive(false);
        originalCameraPosition = playerCamera.localPosition;
        GameObject pausedPanel = GameObject.Find("Canvas/PausedPanel");
        pauser = this.GetComponent<PlayerPause>();

       if (pausedPanel != null)
        {
            // Find the sliders by type within the paused panel
            sensitivityXSlider = pausedPanel.GetComponentInChildren<Slider>();
            sensitivityYSlider = pausedPanel.GetComponentsInChildren<Slider>()[1]; // Assuming it's the second Slider in the hierarchy

            // Add listeners to the sliders
            sensitivityXSlider.onValueChanged.AddListener(UpdateSensitivityX);
            sensitivityYSlider.onValueChanged.AddListener(UpdateSensitivityY);
        }

        
    }

    void Interactioncheck()
    {
        //hide interaction canvas???
        interactUI.SetActive(false);
        RaycastHit hit;
        int mask = 1 << 6;
        mask = ~mask;

        if(Physics.Raycast(interactShooterPoint.position, interactShooterPoint.forward, out hit, maxInteractionDistance, mask))
        {
            //PLAYER cAMERA IS WEIRD???
            Debug.DrawLine(interactShooterPoint.position, hit.transform.position, Color.blue, 5f);
            GameObject hitobject = hit.transform.gameObject;
            MonoBehaviour[] mono = hitobject.GetComponents<MonoBehaviour>();
            foreach(MonoBehaviour script in mono)
            {
                if(script is IInteract)
                {
                    IInteract temp = script as IInteract;
                    if (temp.validToReinteract() == true && pauser.isPaused == false)
                    {
                        interactUI.SetActive(true);
                    }
                    //show interaction canvas;
                    if (validInteract == true && pauser.isPaused == false)
                    {
                        temp.Interaction();
                        validInteract = false;
                    }
                    break;
                }
            }
        }
    }

    private void Update()
    {
        HandleHeadBobbing();

        Interactioncheck();

        transform.Rotate(Vector3.up, mouseX * Time.deltaTime);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;
    }
    public void ReceiveInput (Vector2 mouseInput)
    {
        mouseX = mouseInput.x * sensitivityX;
        mouseY = mouseInput.y * sensitivityY;
    }

    public void InteractButtonPressed()
    {
        validInteract = true;
       
    }

    private void HandleHeadBobbing()
    {
       

        // Calculate lateral bobbing motion based on sprinting status
        if (playerMovement.isGrounded == true && playerMovement.isMoving == true)
        { 
            float bobAmount = Mathf.Sin(Time.time * headBobFrequency) * headBobAmplitude;
            float lateralBobAmount = playerMovement.sprint ? Mathf.Sin(Time.time * lateralBobFrequency) * lateralBobAmplitude : 0f;
            //Calculate the target head bobbing position
            float targetHeadBobPosition = originalCameraPosition.y + bobAmount;

            // Smoothly interpolate to the target head bobbing position
            float currentHeadBobPosition = Mathf.Lerp(playerCamera.localPosition.y, targetHeadBobPosition, Time.deltaTime * 5f);
            // Update the camera's local position with head bobbing
            playerCamera.localPosition = new Vector3(lateralBobAmount, currentHeadBobPosition, 0f);
        }


        

        
    }
    // Method to update sensitivityX based on UI Slider value
    public void UpdateSensitivityX(float value)
    {
        sensitivityX = value;
    }

    // Method to update sensitivityY based on UI Slider value
    public void UpdateSensitivityY(float value)
    {
        sensitivityY = value;
    }









}
