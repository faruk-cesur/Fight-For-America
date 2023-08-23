using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour, IMovable<PlayerMovementData>
{
    [field: SerializeField] public PlayerMovementData MovementData { get; set; }
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private PlayerStates _playerStates;
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private PlayerAttacker _playerAttacker;
    [SerializeField] private Health _health;
    [SerializeField] public Rigidbody _rigidbody;
    [SerializeField] public Transform PlayerVisual;

    private void Start()
    {
        SetEventListeners();
        MovementData.IsCharacterInteract = false;
    }

    private void SetEventListeners()
    {
        _playerInput.Joystick.OnReleaseJoystick += Stop;
        _playerInput.Joystick.OnTouchJoystick += RunningState;
        _playerStates.IdleState += IdleState;
        _playerStates.RunningState += RunningState;
        _playerStates.RunningShootState += _playerAttacker.RunningShootState;
        _playerStates.RunningShootState += Move;
        _playerStates.StandingShootState += _playerAttacker.StandingShootState;
        _health.OnDeath += DeathState;
    }

    public void Move()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        Vector3 targetVelocity = new Vector3(_playerInput.Joystick.Horizontal, 0f, _playerInput.Joystick.Vertical).normalized * (MovementData.MovementSpeed * Time.fixedDeltaTime);
        _rigidbody.velocity = targetVelocity;
    }

    public void Stop()
    {
        _playerStates.CurrentPlayerState = PlayerStates.PlayerState.Idle;
        _playerInput.DisplayJoystick(false);
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _playerAnimator.IdleAnimation();
    }

    public void Rotate()
    {
        if (_rigidbody.velocity == Vector3.zero)
            return;
        _playerInput.DisplayJoystick(true);
        PlayerVisual.localRotation = Quaternion.Slerp(PlayerVisual.localRotation, Quaternion.LookRotation(_rigidbody.velocity), Time.fixedDeltaTime * MovementData.RotateSpeed);
    }

    public bool IsFingerMovingOnJoystick()
    {
        return _playerInput.Joystick.Direction.sqrMagnitude >= 0.1f * 0.1f;
    }

    private void IdleState()
    {
        if (MovementData.IsCharacterInteract)
            return;

        if (IsFingerMovingOnJoystick())
        {
            _playerStates.CurrentPlayerState = PlayerStates.PlayerState.Running;
        }
    }

    private void RunningState()
    {
        _playerStates.CurrentPlayerState = PlayerStates.PlayerState.Running;

        if (MovementData.IsCharacterInteract)
        {
            Stop();
            return;
        }

        if (IsFingerMovingOnJoystick())
        {
            _playerAnimator.RunningAnimation();
            Move();
            Rotate();
        }
    }

    private void DeathState()
    {
        _playerAnimator.DeathAnimation();
        MovementData.IsCharacterInteract = true;
        GameManager.Instance.Lose(0);
        _playerStates.CurrentPlayerState = PlayerStates.PlayerState.Death;
    }

    private void VictoryState()
    {
        _playerAnimator.VictoryAnimation();
        MovementData.IsCharacterInteract = true;
        GameManager.Instance.Win(100);
        _playerStates.CurrentPlayerState = PlayerStates.PlayerState.Victory;
    }
}