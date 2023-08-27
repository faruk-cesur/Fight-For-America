using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlueCastle : MonoBehaviour
{
    public Health BlueCastleHealth;
    public UnityAction OnBlueCastleDeath;
    
    private void Start()
    {
        BlueCastleHealth.OnDeath += LoseOnBlueCastleDeath;
    }

    private void LoseOnBlueCastleDeath()
    {
        OnBlueCastleDeath?.Invoke();
    }
}