using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using System;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private InputActionReference _movementAction;
    [SerializeField] private float _speed;
    private Vector2 _moveVector;

    private void Player_OnDeadStatusChange(bool obj)
    {
        if(!isLocalPlayer) return;

        if (obj)
        {
            _movementAction.action.Disable();
        }
        else
        {
            _movementAction.action.Enable();
        }
    }

    public void FixedUpdate()
    {
        transform.Translate(_speed * Time.fixedDeltaTime * _moveVector);
        
    }
    public override void OnStartAuthority()
    {
        _player.OnDeadStatusChange += Player_OnDeadStatusChange;

        _movementAction.action.performed += OnMovementActionPerform;
        _movementAction.action.canceled += OnMovementActionCancel;
    }

    public override void OnStopAuthority()
    {
        _player.OnDeadStatusChange -= Player_OnDeadStatusChange;

        _movementAction.action.performed -= OnMovementActionPerform;
        _movementAction.action.canceled -= OnMovementActionCancel;
    }

    private void OnMovementActionPerform(InputAction.CallbackContext context)
    {
        _moveVector = context.ReadValue<Vector2>();
    }

    private void OnMovementActionCancel(InputAction.CallbackContext context)
    {
        _moveVector = Vector2.zero;
    }
}
