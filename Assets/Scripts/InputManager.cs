using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput playerInput;

    public static Vector2 Movement;
    public static bool JumpWasPressed;
    public static bool JumpIsHeld;
    public static bool JumpWasReleased;

    private InputAction moveAction;
    private InputAction jumpAction;

    public static bool PauseIsPressed;
    private InputAction pauseAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        pauseAction = playerInput.actions["Pause"];
    }

    private void Update()
    {
        Movement = moveAction.ReadValue<Vector2>();

        JumpWasPressed = jumpAction.WasPressedThisFrame();
        JumpIsHeld = jumpAction.IsPressed();
        JumpWasReleased = jumpAction.WasReleasedThisFrame();

        PauseIsPressed = pauseAction.WasPressedThisFrame();
    }
}
