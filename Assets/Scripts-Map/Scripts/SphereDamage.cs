using UnityEngine;
public class SphereDamage : MonoBehaviour
{
    private int _damage;
    private LayerMask _playerLayer;
    private bool _hasHitTarget;

    public void Initialize(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasHitTarget) return;
        
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out IDamagable target))
            {
                target.TakeDamage(_damage);
                _hasHitTarget = true; 
            }
        }
    }
}