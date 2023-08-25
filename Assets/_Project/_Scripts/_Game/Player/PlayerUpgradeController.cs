using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeController : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private PlayerAttackData _playerAttackData;

    private void Start()
    {
        StartCoroutine(SetAllStartingUpgradeData());
        UpgradeManager.Instance.OnUpgradeHealth += UpgradeCurrentHealth;
        UpgradeManager.Instance.OnUpgradeDamage += UpgradeCurrentDamage;
        UpgradeManager.Instance.OnUpgradeFireRate += UpgradeCurrentFireRate;
    }

    private void OnDestroy()
    {
        UpgradeManager.Instance.OnUpgradeHealth -= UpgradeCurrentHealth;
        UpgradeManager.Instance.OnUpgradeDamage -= UpgradeCurrentDamage;
        UpgradeManager.Instance.OnUpgradeFireRate -= UpgradeCurrentFireRate;
    }

    private IEnumerator SetAllStartingUpgradeData()
    {
        yield return new WaitForSeconds(0.5f);

        UpgradeCurrentHealth();
        UpgradeCurrentDamage();
        UpgradeCurrentFireRate();
    }

    private void UpgradeCurrentHealth()
    {
        var upgrade = UpgradeManager.Instance.Upgrade;
        if (UpgradeManager.Instance.IsUpgradeFull(upgrade.HealthLevel, upgrade.HealthValueList.Count))
        {
            return;
        }

        var upgradedHealth = upgrade.HealthValueList[upgrade.HealthLevel];
        _health.StartingHealth = upgradedHealth;
        _health.SetStartingHealth();
    }

    private void UpgradeCurrentDamage()
    {
        var upgrade = UpgradeManager.Instance.Upgrade;
        if (UpgradeManager.Instance.IsUpgradeFull(upgrade.DamageLevel, upgrade.DamageValueList.Count))
        {
            return;
        }

        var upgradedDamage = upgrade.DamageValueList[upgrade.DamageLevel];
        _playerAttackData.Damage = upgradedDamage;
    }

    private void UpgradeCurrentFireRate()
    {
        var upgrade = UpgradeManager.Instance.Upgrade;
        if (UpgradeManager.Instance.IsUpgradeFull(upgrade.FireRateLevel, upgrade.FireRateValueList.Count))
        {
            return;
        }

        var upgradedFireRate = upgrade.FireRateValueList[upgrade.FireRateLevel];
        _playerAttackData.FireRate = upgradedFireRate;
    }
}