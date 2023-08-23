using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerAttacker : MonoBehaviour, IAttacker<PlayerAttackData>
{
    [field: SerializeField] public PlayerAttackData AttackerData { get; set; }
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerStates _playerStates;
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private FindClosestTarget _findClosestTarget;
    [SerializeField] private Transform _bulletSpawnPosition;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private ParticleSystem _shootParticle;
    private float _currentFireRate = 0;

    public void Attack()
    {
        var spawnedBullet = SpawnBullet();
        var closestTarget = _findClosestTarget.ClosestTarget;
        
        PlayShootParticle();

        if (spawnedBullet.TryGetComponent(out Bullet bullet))
        {
            if (closestTarget.TryGetComponent(out Enemy enemy))
            {
                bullet.TargetTransform = closestTarget.transform;
                bullet.TargetEnemy = enemy;
            }
        }
    }

    private void PlayShootParticle()
    {
        _shootParticle.Play();
    }

    private void FixedUpdate()
    {
        DecreaseCooldownBetweenAttacks();
        FindTargetToChangeState();
    }

    private void FindTargetToChangeState()
    {
        if (!_findClosestTarget.ClosestTarget)
        {
            _playerStates.CurrentPlayerState = _playerController.IsFingerMovingOnJoystick() ? PlayerStates.PlayerState.Running : PlayerStates.PlayerState.Idle;

            return;
        }

        _playerStates.CurrentPlayerState = _playerController.IsFingerMovingOnJoystick() ? PlayerStates.PlayerState.RunningShoot : PlayerStates.PlayerState.StandingShoot;
    }

    private void LookAtTarget()
    {
        var difference = _findClosestTarget.ClosestTarget.transform.position - transform.position;
        _playerController.PlayerVisual.localRotation = Quaternion.Slerp(_playerController.PlayerVisual.localRotation, Quaternion.LookRotation(new Vector3(difference.x, 0, difference.z)), Time.fixedDeltaTime * AttackerData.RotateAimSpeed);
    }

    private void DecreaseCooldownBetweenAttacks()
    {
        _currentFireRate -= Time.fixedDeltaTime;
    }

    private Transform SpawnBullet()
    {
        var bullet = Instantiate(_bulletPrefab, _bulletSpawnPosition.position, Quaternion.identity);
        return bullet.transform;
    }

    public void RunningShootState()
    {
        _playerAnimator.RunningShootAnimation();
        LookAtTarget();

        if (CooldownCompletedForNextAttack())
        {
            _currentFireRate = AttackerData.FireRate;
            Attack();
        }
    }

    public void StandingShootState()
    {
        LookAtTarget();
        
        if (CooldownCompletedForNextAttack())
        {
            _currentFireRate = AttackerData.FireRate;
            Attack();
            _playerAnimator.StandingShootAnimation();
        }
    }
    
    private bool CooldownCompletedForNextAttack()
    {
        return _currentFireRate <= 0;
    }
}