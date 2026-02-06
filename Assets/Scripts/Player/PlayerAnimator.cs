using System;
using Mirror;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    [Header("Player components")]
    [SerializeField] private Player _player;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _isDeadString;
    [SerializeField] private string _isWalkingString;
    [SerializeField] private string _dashString;

    void Awake()
    {
        _player.OnPlayerChange += Player_OnPlayerChange;
        _playerMovement.OnVectorChange += Player_OnVectorChange;
        _playerMovement.OnDash += Player_OnDash;

    }

    void OnDestroy()
    {
        _player.OnPlayerChange -= Player_OnPlayerChange;
        _playerMovement.OnVectorChange -= Player_OnVectorChange;
        _playerMovement.OnDash -= Player_OnDash;
    }

    private void Player_OnPlayerChange()
    {
        _animator.SetBool(_isDeadString, _player.isDead);
    }

    private void Player_OnVectorChange(Vector2 vector)
    {
        CmdSetIsWalking(vector != Vector2.zero);
    }

    private void Player_OnDash()
    {
        _animator.SetTrigger(_dashString);
    }

    [Command] private void CmdSetIsWalking(bool isWalking) => RpcSetIsWalking(isWalking);
    [ClientRpc] private void RpcSetIsWalking(bool isWalking) => _animator.SetBool(_isWalkingString, isWalking);
}
