using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    #region Variables
    private PlayerController playerController;
    private PlayerInputActions playerInputActions;

    private Vector2 moveDirection;
    private bool isFiring = false;
    #endregion

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        playerInputActions = new PlayerInputActions();
        //enable the input from player map
        playerInputActions.Player.Enable();

        //listen to events (key inputs)
        //playerInputActions.Player.Dash.performed += OnDashPerformed;
        playerInputActions.Player.Fire.performed += OnFirePerformed;
        playerInputActions.Player.Fire.canceled += OnFireCanceled;
    }

    private void Update()
    {
        // Read continuous movement input
        moveDirection = playerInputActions.Player.Movement.ReadValue<Vector2>();

        if (isFiring)
        {
            playerController.Fire();
        }
    }

    private void FixedUpdate()
    {
        GetComponent<IMovable>().Move(moveDirection);
    }

    //private void OnDashPerformed(InputAction.CallbackContext context)
    //{
    //    // Set dash input to true; it can be reset by the PlayerController
    //    playerController.Dash();
    //}

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        isFiring = true;
    }

    private void OnFireCanceled(InputAction.CallbackContext context)
    {
        isFiring = false;

    }
}
