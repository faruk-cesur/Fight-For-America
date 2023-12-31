using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CurrencyManager : Singleton<CurrencyManager>
{
    #region Variables

    [field: SerializeField, BoxGroup("CURRENCY DATA")] public CurrencyData GetCurrencyData { get; private set; }

    public UnityAction OnMoneyIsChanged;

    #endregion

    private void Awake() => GameManager.Instance.OnGameStart += LoadCurrency;

    #region Money Change Functions

    public void EarnMoney(int money)
    {
        GetCurrencyData.Money += money;
        SaveCurrency();
        UIManager.Instance.PrintTotalMoneyText();
        OnMoneyIsChanged?.Invoke();
    }

    public void LoseMoney(int money)
    {
        if (GetCurrencyData.IsMoneyEnough(money))
        {
            GetCurrencyData.Money -= money;
            SaveCurrency();
            UIManager.Instance.PrintTotalMoneyText();
            OnMoneyIsChanged?.Invoke();
        }
    }

    #endregion

    #region Save Load Currency

    public void SaveCurrency()
    {
        SaveManager.Instance.SaveData(GetCurrencyData, GetCurrencyData.name);
    }

    private void LoadCurrency()
    {
        SaveManager.Instance.LoadData(GetCurrencyData, GetCurrencyData.name);
    }

    #endregion
}