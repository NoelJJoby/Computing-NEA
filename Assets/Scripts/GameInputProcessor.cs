using System;
using UnityEngine;

public class GameInputProcessor : MonoBehaviour
{
    public static GameInputProcessor Instance { private set; get; }
    private GameInputActions inputActions;

    public event EventHandler OnCameraChangePOV;
    public event EventHandler OnGamePause;
    public event EventHandler OnPlayerJump;
    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerInteract;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        inputActions = new GameInputActions();
        inputActions.Player.Enable();

        inputActions.Player.CameraPOVChange.performed += CameraPOVChange_performed;
        inputActions.Player.Jump.performed += Jump_performed;
        inputActions.Player.Attack.performed += Attack_performed;
        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        Instance = null;
        inputActions.Player.Disable();
        inputActions.Dispose(); 
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnGamePause?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerInteract?.Invoke(this, EventArgs.Empty);
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerJump?.Invoke(this, EventArgs.Empty);
    }

    private void CameraPOVChange_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)

    {
        OnCameraChangePOV?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetInputCameraVector()
    {
        return inputActions.Player.CameraMovement.ReadValue<Vector2>();
    }

    public Vector2 GetInputMovementVectorNormalized()
    {
        return inputActions.Player.Movement.ReadValue<Vector2>();
    }



}
