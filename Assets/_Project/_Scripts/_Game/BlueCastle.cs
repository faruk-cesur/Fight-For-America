using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCastle : MonoBehaviour
{
    public Health BlueCastleHealth;

    private void Start()
    {
        BlueCastleHealth.OnDeath += LoseOnBlueCastleDeath;
    }

    private void LoseOnBlueCastleDeath()
    {
        GameManager.Instance.Lose(0);
    }
}