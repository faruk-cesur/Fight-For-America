using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CaptureEnemyCastle : MonoBehaviour
{
    [SerializeField] private Slider _captureCastleSlider;
    [SerializeField] private float _captureCastleSliderDuration = 2f;
    [SerializeField] private Transform _redCastle;
    [SerializeField] private Transform _newBlueCastle;
    private Tween _captureCastleSliderTween;
    private bool _isSliderCompleted;
    public UnityAction OnCaptureCastle;

    public void IncreaseCaptureCastleSlider()
    {
        _captureCastleSliderTween.Kill();
        _captureCastleSliderTween = _captureCastleSlider.DOValue(1f, _captureCastleSliderDuration).OnComplete(() =>
        {
            OnCaptureCastle?.Invoke();
            _isSliderCompleted = true;
            CaptureTheCastle();
        });
    }

    public void DecreaseCaptureCastleSlider()
    {
        if (_isSliderCompleted)
            return;

        _captureCastleSliderTween.Kill();
        _captureCastleSliderTween = _captureCastleSlider.DOValue(0f, _captureCastleSliderDuration);
    }

    private void CaptureTheCastle()
    {
        _redCastle.DOLocalMoveY(-4, 1f).SetEase(Ease.InBack).OnComplete(() =>
        {
            _newBlueCastle.SetActive(true);
            _newBlueCastle.DOLocalMoveY(0, 1f).SetEase(Ease.OutBack);
            _redCastle.SetActive(false);
        });
    }
}