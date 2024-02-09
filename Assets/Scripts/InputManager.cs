using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] CameraLook mouseLook;
    //[SerializeField] Gun gun;

    public InputMaster inputMaster;
    InputMaster.MovementActions movement;
    InputMaster.CameraLookActions cameraLook;
    //InputMaster.WeaponsActions weapon;

    Vector2 horizontalInput;
    Vector2 mouseInput;
    private void Awake()
    {
        inputMaster = new InputMaster();
        movement = inputMaster.Movement;
        cameraLook = inputMaster.CameraLook;

        movement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        movement.Sprint.performed += _ => playerMovement.OnSprintPressed();

        movement.Jump.performed += _ => playerMovement.OnJumpPressed();

        cameraLook.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        cameraLook.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

       // weapon.Shoot.performed += _ => gun.Shoot();
    }

    private void Update()
    {
        playerMovement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
        
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
