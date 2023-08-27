using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private ParticleSystem _moneyBlastParticle;
    private Coroutine _healthSliderCoroutine;
    
    private void Start()
    {
        StartCoroutine(HealPlayer());
        DisableHealthSlider();
    }

    private IEnumerator SetHealthSliderCoroutine()
    {
        _healthSlider.gameObject.SetActive(true);
        yield return new WaitUntil(() => _health.IsHealthFull);
        yield return new WaitForSeconds(1f);
        _healthSlider.gameObject.SetActive(false);
        _healthSliderCoroutine = null;
    }
    
    private IEnumerator HealPlayer()
    {
        while (!_health.IsDead)
        {
            _health.Heal(1);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            _health.Damage(enemy.ShootableHealth.CurrentHealth);
            DisplayHealthSlider();
            enemy.ShootableHealth.Damage(enemy.ShootableHealth.StartingHealth);
        }

        if (other.TryGetComponent(out StartFightController startFightController))
        {
            startFightController.IncreaseFightSlider();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out StartFightController startFightController))
        {
            startFightController.DecreaseFightSlider();
        }
    }

    private void DisableHealthSlider()
    {
        _healthSlider.gameObject.SetActive(false);
    }

    private void DisplayHealthSlider()
    {
        if (_healthSliderCoroutine != null)
        {
            StopCoroutine(_healthSliderCoroutine);
        }

        _healthSliderCoroutine = StartCoroutine(SetHealthSliderCoroutine());
    }

    public void PlayMoneyBlastParticle()
    {
        _moneyBlastParticle.Play();
    }
}