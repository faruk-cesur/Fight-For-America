using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public UnityAction OnTouchJoystick;
    public UnityAction OnReleaseJoystick;
    [SerializeField] private PlayerMovementData _playerMovementData;

    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (_playerMovementData.IsCharacterInteract)
            return;
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
        OnTouchJoystick?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
        OnReleaseJoystick?.Invoke();
    }
}