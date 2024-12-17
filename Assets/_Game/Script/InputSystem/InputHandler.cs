using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    #region Variables
    private PlayerController playerController;
    private PlayerInputActions playerInputActions;

    private Vector2 movementInput;
    private bool isFiring = false;
    public Vector2 MovementInput => playerInputActions.Player.Movement.ReadValue<Vector2>();
    #endregion

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        playerInputActions = new PlayerInputActions();
        //enable the input from player map
        playerInputActions.Player.Enable();

        //listen to events (key inputs)
        playerInputActions.Player.Dash.performed += OnDashPerformed;
        playerInputActions.Player.Fire.performed += OnFirePerformed;
        playerInputActions.Player.Fire.canceled += OnFireCanceled;
    }

    private void Update()
    {
        // Read continuous movement input
        movementInput = playerInputActions.Player.Movement.ReadValue<Vector2>();

        if (isFiring)
        {
            playerController.Fire();
        }
    }

    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        // Set dash input to true; it can be reset by the PlayerController
        playerController.Dash();
    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        isFiring = true;
    }

    private void OnFireCanceled(InputAction.CallbackContext context)
    {
        isFiring = false;

    }
}
