using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    PlayerInputActions playerInputActions;

    public event Action<Vector2> OnMoveEvent;
    public event Action OnDashEvent;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMove;
        
        playerInputActions.Player.Dash.performed += OnDash;
        
        playerInputActions.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 move = context.ReadValue<Vector2>();
            OnMoveEvent?.Invoke(move);
        }

        if (context.canceled)
        {
            OnMoveEvent?.Invoke(Vector2.zero);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        Debug.Log("Dash");
        OnDashEvent?.Invoke();
    }
}
