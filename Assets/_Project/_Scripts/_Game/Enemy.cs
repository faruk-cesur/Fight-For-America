using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PathCreation;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IShootable, IMovable<EnemyMovementData>
{
    [field: SerializeField, BoxGroup("IShootable Setup")] public Health ShootableHealth { get; set; }
    [field: SerializeField, BoxGroup("IShootable Setup")] public ParticleSystem ShootableParticle { get; set; }
    [field: SerializeField, BoxGroup("IShootable Setup")] public Collider ShootableCollider { get; set; }
    [field: SerializeField, BoxGroup("IShootable Setup")] public Rigidbody ShootableRigidbody { get; set; }
    [field: SerializeField, BoxGroup("IShootable Setup")] public Slider ShootableSlider { get; set; }

    public bool IsEnemyInteract { get; set; }
    [field: SerializeField, BoxGroup("Movement Setup")] public EnemyMovementData MovementData { get; set; }
    [SerializeField, BoxGroup("Movement Setup")] public PathCreator PathCreatorScript;
    [SerializeField, BoxGroup("Movement Setup")] public PlayerTrigger PlayerTriggerScript;
    [SerializeField, BoxGroup("Movement Setup")] private Animator _enemyAnimator;
    [SerializeField, BoxGroup("Movement Setup")] private NavMeshAgent _navMeshAgent;

    [SerializeField, BoxGroup("Death Setup")] private SkinnedMeshRenderer _enemyMeshRenderer;
    [SerializeField, BoxGroup("Death Setup")] private ParticleSystem _deathParticle;
    [SerializeField, BoxGroup("Death Setup")] private List<GameObject> _moneyList;

    [SerializeField, BoxGroup("Settings")] private int _moneyAmount;
    
    private bool IsEnemyDeadOrInteract => ShootableHealth.IsDead || IsEnemyInteract;
    private bool _isEnemyKilledByBlueCastle;
    private float _movedDistance;
    private Tween _getShotColorTween;
    private Coroutine _healthSliderCoroutine;
    private static readonly int IdleAnimation = Animator.StringToHash("Idle");
    private static readonly int DeathAnimation = Animator.StringToHash("Death");
    private static readonly int RunningAnimation = Animator.StringToHash("Running");

    private void Start()
    {
        Stop();
        DisableHealthSlider();
        ShootableHealth.OnDeath += Death;
        ShootableHealth.OnDeath += DeathOnTriggerBlueCastle;
    }

    private void FixedUpdate()
    {
        if (IsEnemyDeadOrInteract)
        {
            Stop();
            return;
        }

        Move();
        //Rotate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BlueCastle blueCastle))
        {
            _isEnemyKilledByBlueCastle = true;
            blueCastle.BlueCastleHealth.Damage(ShootableHealth.CurrentHealth);
            ShootableHealth.Damage(ShootableHealth.StartingHealth);
        }
    }

    public void GetShot(float damage)
    {
        if (_healthSliderCoroutine != null)
        {
            StopCoroutine(_healthSliderCoroutine);
        }

        _healthSliderCoroutine = StartCoroutine(EnableHealthSlider());
        ShootableHealth.Damage(damage);
        ShootableParticle.Play();
        if (_getShotColorTween == null && !ShootableHealth.IsDead)
        {
            _getShotColorTween = _enemyMeshRenderer.material.DOColor(Color.white, 0.25f).From().OnKill(() => _getShotColorTween = null);
        }
    }

    public void Death()
    {
        if (_isEnemyKilledByBlueCastle)
        {
            return;
        }

        Stop();
        _enemyAnimator.SetTrigger(DeathAnimation);
        _deathParticle.Play();
        DisableEnemyCollider();
        TurnEnemyColorBlack();
        DropMoneyFromEnemy();
        DisableHealthSlider();
        StartCoroutine(DestroyAfterDeath());
    }

    private void DeathOnTriggerBlueCastle()
    {
        if (_isEnemyKilledByBlueCastle)
        {
            _deathParticle.transform.SetParent(null);
            _deathParticle.Play();
            Destroy(_deathParticle.gameObject, 2);
            Destroy(gameObject, 0.1f);
        }
    }

    private IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(2.5f);
        transform.DOLocalMoveY(-1, 2.5f).OnComplete(() => Destroy(gameObject));
    }

    private void DisableHealthSlider()
    {
        ShootableSlider.gameObject.SetActive(false);
    }

    private IEnumerator EnableHealthSlider()
    {
        ShootableSlider.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        ShootableSlider.gameObject.SetActive(false);
        _healthSliderCoroutine = null;
    }

    private void DisableEnemyCollider()
    {
        ShootableCollider.enabled = false;
    }

    private void TurnEnemyColorBlack()
    {
        _enemyMeshRenderer.material.DOColor(new Color(0.2f, 0.2f, 0.2f), 0.25f);
    }

    private void DropMoneyFromEnemy()
    {
        foreach (var money in _moneyList)
        {
            money.SetActive(true);
            money.transform.SetParent(null);
            money.transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360).SetLoops(2, LoopType.Restart).SetEase(Ease.Linear);
            money.transform.DOMoveY(2f, 0.5f).SetLoops(4, LoopType.Yoyo).OnComplete(() =>
            {
                money.transform.SetParent(PlayerTriggerScript.transform);
                money.transform.DOLocalMove(Vector3.zero, 1f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    PlayerTriggerScript.PlayMoneyBlastParticle();
                    CurrencyManager.Instance.EarnMoney(_moneyAmount);
                    Destroy(money);
                });
            });
        }
    }

    public void Move()
    {
        //_movedDistance += MovementData.MovementSpeed * Time.deltaTime;
        //transform.position = _pathCreator.path.GetPointAtDistance(_movedDistance, EndOfPathInstruction.Stop);
        _enemyAnimator.SetTrigger(RunningAnimation);
        _navMeshAgent.speed = MovementData.MovementSpeed;
        _navMeshAgent.SetDestination(PathCreatorScript.path.GetPoint(PathCreatorScript.path.NumPoints - 1));
    }

    public void Stop()
    {
        _enemyAnimator.SetTrigger(IdleAnimation);
        IsEnemyInteract = true;
        if (_navMeshAgent.hasPath)
        {
            _navMeshAgent.ResetPath();
        }
    }

    public void Rotate()
    {
        // var pathRotation = _pathCreator.path.GetRotationAtDistance(_movedDistance, EndOfPathInstruction.Stop).eulerAngles;
        // transform.rotation = Quaternion.Euler(0, pathRotation.y, 0);
    }
}