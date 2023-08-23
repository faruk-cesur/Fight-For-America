using UnityEngine;

[CreateAssetMenu]
public class PlayerAttackData : ScriptableObject
{
    [field: SerializeField] public int Damage { get; set; }
    [field: SerializeField] public float FireRate { get; set; }
    [field: SerializeField] public float RotateAimSpeed { get; private set; }
}