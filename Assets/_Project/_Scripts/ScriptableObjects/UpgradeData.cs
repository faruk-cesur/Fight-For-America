using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class UpgradeData : ScriptableObject
{
    [field: SerializeField, BoxGroup("PRICE LIST")] public List<int> HealthPriceList { get; set; }
    [field: SerializeField, BoxGroup("PRICE LIST")] public List<int> DamagePriceList { get; set; }
    [field: SerializeField, BoxGroup("PRICE LIST")] public List<int> FireRatePriceList { get; set; }
    [field: SerializeField, BoxGroup("VALUE LIST")] public List<int> HealthValueList { get; set; }
    [field: SerializeField, BoxGroup("VALUE LIST")] public List<int> DamageValueList { get; set; }
    [field: SerializeField, BoxGroup("VALUE LIST")] public List<float> FireRateValueList { get; set; }
    [field: SerializeField, BoxGroup("LEVELS")] public int HealthLevel { get; set; }
    [field: SerializeField, BoxGroup("LEVELS")] public int DamageLevel { get; set; }
    [field: SerializeField, BoxGroup("LEVELS")] public int FireRateLevel { get; set; }
    
    [Button("Reset Levels")]
    public void ResetLevels()
    {
        HealthLevel = 0;
        DamageLevel = 0;
        FireRateLevel = 0;
        SaveManager.DeleteData(this.name);
    }
}