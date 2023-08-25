using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public UnityAction OnUpgradeHealth;
    public UnityAction OnUpgradeDamage;
    public UnityAction OnUpgradeFireRate;
    public UpgradeData Upgrade;
    [SerializeField] private TextMeshProUGUI _healthUpgradePriceText;
    [SerializeField] private TextMeshProUGUI _damageUpgradePriceText;
    [SerializeField] private TextMeshProUGUI _fireRateUpgradePriceText;
    [SerializeField] private Button _healthUpgradeButton;
    [SerializeField] private Button _damageUpgradeButton;
    [SerializeField] private Button _fireRateUpgradeButton;
    [SerializeField] private Button _openUpgradeUIButton;
    [SerializeField] private Button _closeUpgradeUIButton;
    [SerializeField] private CanvasGroup _upgradeUICanvas;

    private void Start()
    {
        SaveManager.Instance.LoadData(Upgrade, Upgrade.name);
        CurrencyManager.Instance.OnMoneyIsChanged += SetAllUpgradesOnMoneyChanged;
        SetAllUpgradesOnMoneyChanged();
    }

    private void SetAllUpgradesOnMoneyChanged()
    {
        SetHealthUpgradeOnMoneyChanged();
        SetDamageUpgradeOnMoneyChanged();
        SetFireRateUpgradeOnMoneyChanged();
    }

    private void SetHealthUpgradeOnMoneyChanged()
    {
        if (IsUpgradeFull(Upgrade.HealthLevel + 1, Upgrade.HealthValueList.Count))
        {
            _healthUpgradePriceText.text = "FULL";
            _healthUpgradeButton.interactable = false;
        }
        else
        {
            _healthUpgradePriceText.text = Upgrade.HealthPriceList[Upgrade.HealthLevel].ToString();
            _healthUpgradeButton.interactable = CurrencyManager.Instance.GetCurrencyData.IsMoneyEnough(Upgrade.HealthPriceList[Upgrade.HealthLevel]);
        }
    }

    private void SetDamageUpgradeOnMoneyChanged()
    {
        if (IsUpgradeFull(Upgrade.DamageLevel + 1, Upgrade.DamageValueList.Count))
        {
            _damageUpgradePriceText.text = "FULL";
            _damageUpgradeButton.interactable = false;
        }
        else
        {
            _damageUpgradePriceText.text = Upgrade.DamagePriceList[Upgrade.DamageLevel].ToString();
            _damageUpgradeButton.interactable = CurrencyManager.Instance.GetCurrencyData.IsMoneyEnough(Upgrade.DamagePriceList[Upgrade.DamageLevel]);
        }
    }

    private void SetFireRateUpgradeOnMoneyChanged()
    {
        if (IsUpgradeFull(Upgrade.FireRateLevel + 1, Upgrade.FireRateValueList.Count))
        {
            _fireRateUpgradePriceText.text = "FULL";
            _fireRateUpgradeButton.interactable = false;
        }
        else
        {
            _fireRateUpgradePriceText.text = Upgrade.FireRatePriceList[Upgrade.FireRateLevel].ToString();
            _fireRateUpgradeButton.interactable = CurrencyManager.Instance.GetCurrencyData.IsMoneyEnough(Upgrade.FireRatePriceList[Upgrade.FireRateLevel]);
        }
    }

    public bool IsUpgradeFull(int upgradeLevel, int valueListCount)
    {
        return upgradeLevel == valueListCount;
    }

    public void UpgradeHealth()
    {
        if (IsUpgradeFull(Upgrade.HealthLevel, Upgrade.HealthValueList.Count))
            return;

        Upgrade.HealthLevel++;
        OnUpgradeHealth?.Invoke();
        CurrencyManager.Instance.LoseMoney(Upgrade.HealthPriceList[Upgrade.HealthLevel - 1]);
        SaveManager.Instance.SaveData(Upgrade, Upgrade.name);
    }

    public void UpgradeDamage()
    {
        if (IsUpgradeFull(Upgrade.DamageLevel, Upgrade.DamageValueList.Count))
            return;

        Upgrade.DamageLevel++;
        OnUpgradeDamage?.Invoke();
        CurrencyManager.Instance.LoseMoney(Upgrade.DamagePriceList[Upgrade.DamageLevel - 1]);
        SaveManager.Instance.SaveData(Upgrade, Upgrade.name);
    }

    public void UpgradeFireRate()
    {
        if (IsUpgradeFull(Upgrade.FireRateLevel, Upgrade.FireRateValueList.Count))
            return;

        Upgrade.FireRateLevel++;
        OnUpgradeFireRate?.Invoke();
        CurrencyManager.Instance.LoseMoney(Upgrade.FireRatePriceList[Upgrade.FireRateLevel - 1]);
        SaveManager.Instance.SaveData(Upgrade, Upgrade.name);
    }

    public void OpenUpgradeUI()
    {
        _openUpgradeUIButton.interactable = false;
        _upgradeUICanvas.gameObject.SetActive(true);
        _upgradeUICanvas.DOFade(1f, 0.5f).OnComplete(() => _openUpgradeUIButton.interactable = true);
    }

    public void CloseUpgradeUI()
    {
        _closeUpgradeUIButton.interactable = false;
        _upgradeUICanvas.DOFade(0f, 0.5f).OnComplete(() =>
        {
            _closeUpgradeUIButton.interactable = true;
            _upgradeUICanvas.gameObject.SetActive(false);
        });
    }
}