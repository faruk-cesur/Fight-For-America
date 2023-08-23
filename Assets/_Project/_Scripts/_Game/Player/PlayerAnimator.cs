using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Shooting = Animator.StringToHash("Shooting");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Victory = Animator.StringToHash("Victory");

    public void IdleAnimation()
    {
        _playerAnimator.SetBool(Running, false);
        _playerAnimator.SetBool(Shooting, false);
    }

    public void RunningAnimation()
    {
        _playerAnimator.SetBool(Running, true);
        _playerAnimator.SetBool(Shooting, false);
    }

    public void RunningShootAnimation()
    {
        _playerAnimator.SetBool(Running, true);
        _playerAnimator.SetBool(Shooting, true);
    }

    public void StandingShootAnimation()
    {
        _playerAnimator.SetBool(Running, false);
        _playerAnimator.SetBool(Shooting, true);
    }

    public void DeathAnimation()
    {
        _playerAnimator.SetTrigger(Death);
    }

    public void VictoryAnimation()
    {
        _playerAnimator.SetTrigger(Victory);
    }
}