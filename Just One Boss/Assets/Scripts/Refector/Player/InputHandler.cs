using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{
    PlayerInputActions inputActions;

    public event Action<Vector2> OnMoveEvent;
    public event Action OnDashEvent;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Dash.performed += OnDash;
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Dash.performed -= OnDash;
        inputActions.Disable();
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            var v = ctx.ReadValue<Vector2>();
            OnMoveEvent?.Invoke(v);
        }
        if (ctx.canceled)
            OnMoveEvent?.Invoke(Vector2.zero);
    }

    void OnDash(InputAction.CallbackContext ctx)
    {
        OnDashEvent?.Invoke();
    }
}
