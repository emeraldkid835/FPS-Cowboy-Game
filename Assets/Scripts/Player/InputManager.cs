using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] CameraLook mouseLook;
    [SerializeField] PlayerPause playerPause;
    //[SerializeField] Gun gun;

    public InputMaster inputMaster;
    InputMaster.MovementActions movement;
    InputMaster.CameraLookActions cameraLook;
    InputMaster.PauseOptionsActions pause;
    //InputMaster.WeaponsActions weapon;

    Vector2 horizontalInput;
    Vector2 mouseInput;
    private void Awake()
    {
        inputMaster = new InputMaster();
        movement = inputMaster.Movement;
        cameraLook = inputMaster.CameraLook;
        pause = inputMaster.PauseOptions;

        movement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        movement.Sprint.performed += _ => playerMovement.OnSprintPressed();

        movement.Jump.performed += _ => playerMovement.OnJumpPressed();

        cameraLook.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        cameraLook.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        pause.Pause.performed += _ => playerPause.OnPausePressed();

       // weapon.Shoot.performed += _ => gun.Shoot();
    }

    private void Update()
    {
        playerMovement.ReceiveInput(horizontalInput);
        if(playerPause.isPaused == false)
        {
            mouseLook.ReceiveInput(mouseInput);
        }
        else
        {
            return;
        }
       
        
        
    }

    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }
}