using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [field: SerializeField, BoxGroup("Joystick")] public FloatingJoystick Joystick { get; set; }

    public void DisplayJoystick(bool trueOrFalse)
    {
        Joystick.background.gameObject.SetActive(trueOrFalse);
    }
}