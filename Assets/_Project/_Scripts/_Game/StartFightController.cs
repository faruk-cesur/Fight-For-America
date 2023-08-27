using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartFightController : MonoBehaviour
{
    [SerializeField] private EnemyHolder _enemyHolder;
    [SerializeField] private Slider _startFightSlider;
    [SerializeField] private float _startFightSliderDuration = 2f;
    private Tween _startFightSliderTween;
    private bool _isSliderCompleted;

    public void StartFight()
    {
        foreach (var enemy in _enemyHolder.EnemyWaveList[_enemyHolder.CurrentWaveNumber].EnemiesInWave)
        {
            enemy.IsEnemyInteract = false;
        }
    }

    public void IncreaseFightSlider()
    {
        _startFightSliderTween.Kill();
        _startFightSliderTween = _startFightSlider.DOValue(1f, _startFightSliderDuration).OnComplete(() =>
        {
            _isSliderCompleted = true;
            StartFight();
        });
    }

    public void DecreaseFightSlider()
    {
        if (_isSliderCompleted)
            return;

        _startFightSliderTween.Kill();
        _startFightSliderTween = _startFightSlider.DOValue(0f, _startFightSliderDuration);
    }
}