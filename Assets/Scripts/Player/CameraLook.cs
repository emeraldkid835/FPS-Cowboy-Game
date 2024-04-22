using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLook : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] public float sensitivityX = 8f;
    [SerializeField] public float sensitivityY = 0.5f;
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

    public Button saveButton;
    public Button loadButton;

    Vector3 originalCameraPosition;

    

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        

        originalCameraPosition = playerCamera.localPosition;
        GameObject pausedPanel = GameObject.Find("Canvas/PausedPanel");

       if (pausedPanel != null)
        {
            // Find the sliders by type within the paused panel
            sensitivityXSlider = pausedPanel.GetComponentInChildren<Slider>();
            sensitivityYSlider = pausedPanel.GetComponentsInChildren<Slider>()[1]; // Assuming it's the second Slider in the hierarchy

            
        }
            // Add listeners to sliders
            if (sensitivityXSlider != null)
            {
                sensitivityXSlider.onValueChanged.AddListener(UpdateSensitivityX);
            }
            if (sensitivityYSlider != null)
            {
                sensitivityYSlider.onValueChanged.AddListener(UpdateSensitivityY);
            }
        // Add listeners to the Save and Load buttons
        saveButton.onClick.AddListener(SaveSettings);
        loadButton.onClick.AddListener(LoadSettings);

        LoadSettings();

        
    }
    private void Update()
    {
        HandleHeadBobbing();


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
        PlayerPrefs.SetFloat("sensX", value);
        PlayerPrefs.Save();

        Debug.Log("Updated sensX: " + sensitivityX);
        Debug.Log("SliderX value: " + value);
    }

    // Method to update sensitivityY based on UI Slider value
    public void UpdateSensitivityY(float value)
    {
        sensitivityY = value;
        PlayerPrefs.SetFloat("sensY", value);
        PlayerPrefs.Save();

        Debug.Log("Updated sensY: " + sensitivityY);
        Debug.Log("SliderY value: " + value);
    }

    // Method to save sensitivity settings
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("sensX", sensitivityX);
        PlayerPrefs.SetFloat("sensY", sensitivityY);
        PlayerPrefs.Save();
        Debug.Log("Saved sensX: " + sensitivityX + ", sensY: " + sensitivityY);

    }

    // Method to load sensitivity settings
    private void LoadSettings()
    {
        sensitivityX = PlayerPrefs.GetFloat("sensX", sensitivityX);
        sensitivityY = PlayerPrefs.GetFloat("sensY", sensitivityY);

        Debug.Log("Loaded sensX: " + sensitivityX + ", sensY: " + sensitivityY);

        // Update sliders
        if (sensitivityXSlider != null)
        {
            sensitivityXSlider.value = sensitivityX;
            UpdateSensitivityX(sensitivityX);
            Debug.Log("SliderX value: " + sensitivityXSlider.value);
        }
        if (sensitivityYSlider != null)
        {
            sensitivityYSlider.value = sensitivityY;
            UpdateSensitivityY(sensitivityY);
            Debug.Log("SliderY value: " + sensitivityYSlider.value);
        }
    }









}
