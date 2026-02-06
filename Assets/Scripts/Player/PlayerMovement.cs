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
    public event Action<Vector2> OnVectorChange;
    public event Action OnDash;

    [SerializeField] private Player _player;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _visuals;
    [Header("Input actions")]
    [SerializeField] private InputActionReference _movementAction;
    [SerializeField] private InputActionReference _dashAction;

    [Header("Dash")]
    [SerializeField] private float _dashCooldown = 1f;
    [SerializeField] private float _dashDuration = 0.15f;
    [Tooltip("Best to set bigger than regular speed")]
    [SerializeField] private float _dashStrength = 2;

    private Vector2 _moveVector;
    private Vector2 _prevMoveVector;

    private bool _isDashing = false;
    private Vector2 _dashDirection;
    private float _dashTimer = 0f;
    private float _dashCooldownTimer = 0f;

    private void Player_OnDeadStatusChange(bool obj)
    {
        if(!isLocalPlayer) return;

        if (obj)
        {
            _movementAction.action.Disable();
            _dashAction.action.Disable();
        }
        else
        {
            _dashAction.action.Enable();
            _movementAction.action.Enable();
        }
    }

    public void Update()
    {
        if(_prevMoveVector == _moveVector || _moveVector == Vector2.zero) return;

        _visuals.transform.localScale = new Vector3(Mathf.Sign(_moveVector.x) * 4,4,4);
    }

    public void FixedUpdate()
    {
        if (_dashCooldownTimer > 0f)
        {
            _dashCooldownTimer -= Time.fixedDeltaTime;
        }

        // Handle dash logic
        if (_isDashing)
        {
            if (_dashTimer < _dashDuration)
            {
                // Apply dash movement
                transform.Translate(_dashStrength * Time.fixedDeltaTime * _dashDirection);
                _dashTimer += Time.fixedDeltaTime;
            }
            else
            {
                // End dash
                _isDashing = false;
                _dashTimer = 0f;
                _dashCooldownTimer = _dashCooldown;
            }
        }
        else
        {
            // Normal movement (only when not dashing)
            transform.Translate(_speed * Time.fixedDeltaTime * _moveVector);
        }
    }

    public override void OnStartAuthority()
    {
        _player.OnDeadStatusChange += Player_OnDeadStatusChange;

        _movementAction.action.performed += OnMovementActionPerform;
        _movementAction.action.canceled += OnMovementActionCancel;
        _dashAction.action.started += OnDashActionStarted;
    }

    public override void OnStopAuthority()
    {
        _player.OnDeadStatusChange -= Player_OnDeadStatusChange;

        _movementAction.action.performed -= OnMovementActionPerform;
        _movementAction.action.canceled -= OnMovementActionCancel;
        _dashAction.action.started -= OnDashActionStarted;
    }

    private void OnMovementActionPerform(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
        _moveVector = context.ReadValue<Vector2>();
        OnVectorChange?.Invoke(_moveVector);
    }

    private void OnMovementActionCancel(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
        _moveVector = Vector2.zero;
        OnVectorChange?.Invoke(_moveVector);
    }


    private void OnDashActionStarted(InputAction.CallbackContext context)
    {
        if (_dashCooldownTimer <= 0f && _moveVector != Vector2.zero)
        {
            _isDashing = true;
            _dashDirection = _moveVector;
            _dashTimer = 0f;
            OnDash?.Invoke();
        }
    }
}
