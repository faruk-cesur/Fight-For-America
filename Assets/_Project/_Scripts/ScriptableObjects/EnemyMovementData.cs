using UnityEngine;

[CreateAssetMenu]
public class EnemyMovementData : ScriptableObject
{
    [field: SerializeField] public float MovementSpeed { get; set; }
}