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
    [SerializeField] private AudioClip _shootAudio;
    private float _currentFireRate = 0;

    public void Attack()
    {
        var closestTarget = _findClosestTarget.ClosestTarget;
        if (closestTarget is null)
        {
            return;
        }
        if (closestTarget.TryGetComponent(out Enemy enemy))
        {
            if (enemy.IsEnemyInteract)
                return;

            AudioManager.Instance.PlayAudio(_shootAudio, 0.25f, 0, false);
            PlayShootParticle();
            var spawnedBullet = SpawnBullet();

            if (spawnedBullet.TryGetComponent(out Bullet bullet))
            {
                bullet.TargetTransform = closestTarget.transform;
                bullet.TargetEnemy = enemy;
                bullet.BulletDamage = AttackerData.Damage;
            }
        }
    }

    private void PlayShootParticle()
    {
        _shootParticle.Play();
    }

    private void Update()
    {
        DecreaseCurrentFireRate();
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
        if (!_findClosestTarget.ClosestTarget)
            return;

        var difference = _findClosestTarget.ClosestTarget.transform.position - transform.position;
        _playerController.PlayerVisual.localRotation = Quaternion.Slerp(_playerController.PlayerVisual.localRotation, Quaternion.LookRotation(new Vector3(difference.x, 0, difference.z)), Time.fixedDeltaTime * AttackerData.RotateAimSpeed);
    }

    private void DecreaseCurrentFireRate()
    {
        _currentFireRate -= Time.deltaTime;
    }

    private Transform SpawnBullet()
    {
        var bullet = Instantiate(_bulletPrefab, _bulletSpawnPosition.position, Quaternion.identity);
        return bullet.transform;
    }

    public void RunningShootState()
    {
        _playerController.Move();
        _playerAnimator.RunningShootAnimation();
        LookAtTarget();

        if (FireRateCompletedForNextAttack())
        {
            _currentFireRate = AttackerData.FireRate;
            Attack();
        }
    }

    public void StandingShootState()
    {
        _playerController.Stop();
        _playerAnimator.StandingShootAnimation();
        LookAtTarget();

        if (FireRateCompletedForNextAttack())
        {
            _currentFireRate = AttackerData.FireRate;
            Attack();
        }
    }

    private bool FireRateCompletedForNextAttack()
    {
        return _currentFireRate <= 0;
    }
}