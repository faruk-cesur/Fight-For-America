using UnityEngine;

[CreateAssetMenu]
public class PlayerMovementData : ScriptableObject
{
    [field: SerializeField] public float MovementSpeed { get; set; }
    [field: SerializeField] public float RotateSpeed { get; private set; }
    [field: SerializeField] public bool IsCharacterInteract { get; set; }
}