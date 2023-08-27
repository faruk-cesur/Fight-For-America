using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerStates), typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour, IMovable<PlayerMovementData>
{
    [field: SerializeField] public PlayerMovementData MovementData { get; set; }
    [SerializeField] private PlayerStates _playerStates;
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private PlayerAttacker _playerAttacker;
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private Health _health;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private EnemyHolder _enemyHolder;
    [SerializeField] private BlueCastle _blueCastle;
    [SerializeField] public Transform PlayerVisual;

    private void Start()
    {
        SetEventListeners();
        MovementData.IsCharacterInteract = false;
    }

    private void SetEventListeners()
    {
        _playerStates.IdleState += IdleState;
        _playerStates.RunningState += RunningState;
        _playerStates.RunningShootState += _playerAttacker.RunningShootState;
        _playerStates.StandingShootState += _playerAttacker.StandingShootState;
        _health.OnDeath += DeathState;
        _blueCastle.OnBlueCastleDeath += DeathState;
    }

    public void Move()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        Vector3 targetVelocity = new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical).normalized * (MovementData.MovementSpeed * Time.fixedDeltaTime);
        _rigidbody.velocity = targetVelocity;
    }

    public void Stop()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Rotate()
    {
        if (_rigidbody.velocity == Vector3.zero)
            return;
        PlayerVisual.localRotation = Quaternion.Slerp(PlayerVisual.localRotation, Quaternion.LookRotation(_rigidbody.velocity), Time.fixedDeltaTime * MovementData.RotateSpeed);
    }

    public bool IsFingerMovingOnJoystick()
    {
        return _joystick.Direction.sqrMagnitude >= 0.1f * 0.1f;
    }

    private void IdleState()
    {
        Stop();
        _playerAnimator.IdleAnimation();
    }

    private void RunningState()
    {
        if (MovementData.IsCharacterInteract)
        {
            IdleState();
            return;
        }

        _playerAnimator.RunningAnimation();
        Move();
        Rotate();
    }

    private void DeathState()
    {
        Stop();
        MovementData.IsCharacterInteract = true;
        _playerStates.CurrentPlayerState = PlayerStates.PlayerState.Death;
        _playerAnimator.DeathAnimation();
        _enemyHolder.StopAllEnemies();
        GameManager.Instance.Lose(0);
    }

    private void VictoryState()
    {
        MovementData.IsCharacterInteract = true;
        _playerStates.CurrentPlayerState = PlayerStates.PlayerState.Victory;
        _playerAnimator.VictoryAnimation();
        GameManager.Instance.Win(100);
    }
}