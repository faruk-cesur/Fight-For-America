using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private Health _health;

    private void Start()
    {
        StartCoroutine(HealPlayer());
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
            enemy.Death();
            _health.Damage(enemy.ShootableHealth.CurrentHealth);
        }
    }
}