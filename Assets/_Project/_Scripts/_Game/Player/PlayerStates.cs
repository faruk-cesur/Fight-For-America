using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStates : MonoBehaviour
{
    public enum PlayerState
    {
        None,
        Idle,
        Running,
        RunningShoot,
        StandingShoot,
        Death,
        Victory
    }

    public PlayerState CurrentPlayerState;

    public UnityAction IdleState;
    public UnityAction RunningState;
    public UnityAction RunningShootState;
    public UnityAction StandingShootState;
    public UnityAction DeathState;
    public UnityAction VictoryState;

    private void FixedUpdate()
    {
        StateMachine();
    }

    private void StateMachine()
    {
        switch (CurrentPlayerState)
        {
            case PlayerState.None:
                break;
            case PlayerState.Idle:
                IdleState?.Invoke();
                break;
            case PlayerState.Running:
                RunningState?.Invoke();
                break;
            case PlayerState.RunningShoot:
                RunningShootState?.Invoke();
                break;
            case PlayerState.StandingShoot:
                StandingShootState?.Invoke();
                break;
            case PlayerState.Death:
                //DeathState?.Invoke();
                break;
            case PlayerState.Victory:
                //VictoryState?.Invoke();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}