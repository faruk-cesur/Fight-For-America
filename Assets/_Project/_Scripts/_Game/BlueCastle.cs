using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlueCastle : MonoBehaviour
{
    public Health BlueCastleHealth;
    public UnityAction OnBlueCastleDeath;
    public CameraController VirtualCameraController;

    private void Start()
    {
        BlueCastleHealth.OnDeath += LoseOnBlueCastleDeath;
        BlueCastleHealth.OnDeath += SetBlueCastleDeathVirtualCamera;
    }

    private void LoseOnBlueCastleDeath()
    {
        OnBlueCastleDeath?.Invoke();
    }

    private void SetBlueCastleDeathVirtualCamera()
    {
        StartCoroutine(VirtualCameraController.SetBlueCastleDeathVirtualCamera());
    }
}