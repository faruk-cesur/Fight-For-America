using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField, BoxGroup("HEALTH SETTINGS")] private float _startingHealth;
    [SerializeField, BoxGroup("HEALTH SETTINGS")] private float _sliderSpeed = 0.5f;

    [SerializeField, BoxGroup("HEALTH SETUP")] private Slider _healthSlider;
    [SerializeField, BoxGroup("HEALTH SETUP")] private Gradient _gradientHealthColor;
    [SerializeField, BoxGroup("HEALTH SETUP")] private Image _fill;

    public UnityAction OnDeath;
    public bool IsDead { get; set; }
    public bool IsHealthFull => _currentHealth >= _startingHealth;
    private float _currentHealth;

    public float CurrentHealth
    {
        get => _currentHealth;

        set
        {
            if (IsDead)
                return;

            _currentHealth = value;
            SetHealthSliderValue();

            if (IsHealthLowerThanZero())
            {
                Kill();
            }
        }
    }

    private bool IsHealthLowerThanZero()
    {
        return _currentHealth <= 0;
    }

    private void Start()
    {
        SetStartingHealth();
    }

    private void SetStartingHealth()
    {
        CurrentHealth = _startingHealth;
        _healthSlider.maxValue = _startingHealth;
        _healthSlider.minValue = 0;
        SetHealthSliderValue();
    }

    private void SetHealthSliderValue()
    {
        if (_healthSlider.value <= 0)
            return;

        _healthSlider.DOValue(_currentHealth, _sliderSpeed);
        SetGradientHealthColor();
    }

    private void SetGradientHealthColor()
    {
        var targetHealthSliderValue = (_healthSlider.maxValue - CurrentHealth);
        var normalizedSliderValue = Mathf.InverseLerp(_healthSlider.maxValue, 0, targetHealthSliderValue);
        _fill.DOColor(_gradientHealthColor.Evaluate(normalizedSliderValue), _sliderSpeed);
    }

    public void Damage(float damageAmount)
    {
        if (IsDead)
            return;

        CurrentHealth -= damageAmount;
    }

    public void Heal(float healAmount)
    {
        if (IsDead)
            return;

        CurrentHealth += healAmount;
        if (CurrentHealth > _startingHealth)
        {
            CurrentHealth = _startingHealth;
        }
    }

    private void Kill()
    {
        if (IsDead)
            return;

        IsDead = true;
        _currentHealth = 0;
        OnDeath?.Invoke();
    }
}