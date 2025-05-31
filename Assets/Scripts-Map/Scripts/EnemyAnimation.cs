using UnityEngine;

public class EnemyAnimation
{
    private Animator _anim;
    
    private readonly int _speedHash = Animator.StringToHash("speed");
    private readonly int _attackHash = Animator.StringToHash("attack");
    private readonly int _deathHash = Animator.StringToHash("isDead");
    private readonly int _hitHash = Animator.StringToHash("getHit");
    public  EnemyAnimation(Animator animator)
    {
        _anim = animator;
    }

    public void SetAttackTrigger()
    {
        _anim.SetTrigger(_attackHash);
    }

    public void UpdateMovementSpeed(float speed)
    {
        _anim.SetFloat(_speedHash, speed);
    }

    public void SetDeathStatus()
    {
        _anim.SetBool(_deathHash, true);
    }

    public void TriggerGotHit()
    {
        _anim.SetTrigger(_hitHash);
    }
    
}
