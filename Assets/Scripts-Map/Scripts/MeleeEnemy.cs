using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class MeleeEnemy : Enemy
{
    [SerializeField] private GameObject _currentWeapon;
    [SerializeField] float _detectionRadius = 10f; 
    [SerializeField] float _attackDistance = 2f; 
    [SerializeField] float _returnWaitTime = 3f;
    [SerializeField] private float _rotateSmooth = 5f;
    
    private Vector3 _initialPosition; 
    private NavMeshAgent _navMeshAgent; 
    private float _distanceToPlayer; 
    private float _returnTimer;
    private float _speed;
    
    private enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Return
    }

    private EnemyState _currentState;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _initialPosition = transform.position; 
        _currentState = EnemyState.Idle;
        animationHandler = new EnemyAnimation(GetComponent<Animator>());
    }

    private void Update()
    {
        if (isDead) return;
        if (base.player.TryGetComponent(out Player player))
        {
            if(player.IsDead) _currentState = EnemyState.Return;
        }
        
        _distanceToPlayer = Vector3.Distance(base.player.position, transform.position);
        _speed = _navMeshAgent.velocity.magnitude; 
        switch (_currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Chase:
                HandleChaseState();
                break;
            case EnemyState.Attack:
                HandleAttackState();
                break;
            case EnemyState.Return:
                HandleReturnState();
                break;
        }
    }

    private void HandleIdleState()
    {
        animationHandler.UpdateMovementSpeed(_speed);
        if (_distanceToPlayer <= _detectionRadius)
        {
            _currentState = EnemyState.Chase;
        }
    }

    private void HandleChaseState()
    {
        UpdateRotationTowardsPlayer();
        if (_distanceToPlayer <= _attackDistance)
        {
            _currentState = EnemyState.Attack;
        }
        else if (_distanceToPlayer > _detectionRadius)
        {
            _returnTimer = _returnWaitTime;
            _navMeshAgent.isStopped = true;
            _currentState = EnemyState.Return;
        }
        else
        {
            _navMeshAgent.SetDestination(player.position);
            animationHandler.UpdateMovementSpeed(_speed);
        }
    }

    private void HandleAttackState()
    {
        UpdateRotationTowardsPlayer();
        if (timeBetweenAttacks > 0f) timeBetweenAttacks -= Time.deltaTime;
        else timeBetweenAttacks = attackTimer;
        if (_distanceToPlayer > _attackDistance)
        {
            _navMeshAgent.isStopped = false;
            _currentState = EnemyState.Chase;
        }
        else
        {
            _navMeshAgent.isStopped = true;
            animationHandler.UpdateMovementSpeed(_speed);

            if (timeBetweenAttacks <= 0f)
            {
                animationHandler.SetAttackTrigger();
                if(_currentWeapon.TryGetComponent(out Weapon weapon)) weapon.Attack();
            }
        }
    }

    private void HandleReturnState()
    {
        animationHandler.UpdateMovementSpeed(_speed);
        if (_returnTimer > 0f)
        {
            _returnTimer -= Time.deltaTime; 
        }
        else
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_initialPosition);
            _currentState = EnemyState.Idle;
        }
    }
    
    private void UpdateRotationTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0;
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotateSmooth);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDistance);
    }
}
