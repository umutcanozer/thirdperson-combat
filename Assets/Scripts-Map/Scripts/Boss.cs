using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float specialAttackCooldown = 10f;
    
    [SerializeField] private float sphereRadius = 2f;  
    [SerializeField] private int damage = 5;     
 
    [SerializeField] private float sphereDuration;
    [SerializeField] private GameObject spherePosition;
    
    private NavMeshAgent _navMeshAgent;
    private float _speed;
    

    private enum BossState
    {
        Idle,
        Chase,
        Attack
    }

    private BossState currentState;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = BossState.Idle;
        animationHandler = new EnemyAnimation(GetComponent<Animator>());
    }

    private void Update()
    {
        if (isDead) return;
        if (base.player.TryGetComponent(out Player player))
        {
            if(player.IsDead) currentState = BossState.Idle;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, base.player.position);
        _speed = _navMeshAgent.velocity.magnitude;

        switch (currentState)
        {
            case BossState.Idle:
                HandleIdleState(distanceToPlayer);
                break;
            case BossState.Chase:
                HandleChaseState(distanceToPlayer);
                break;
            case BossState.Attack:
                HandleAttackState(distanceToPlayer);
                break;
        }
    }

    private void HandleIdleState(float distanceToPlayer)
    {
        animationHandler.UpdateMovementSpeed(_speed);

        if (distanceToPlayer <= detectionRadius)
        {
            currentState = BossState.Chase;
        }
    }

    private void HandleChaseState(float distanceToPlayer)
    {
        RotateTowardsPlayer();
        if (distanceToPlayer <= attackDistance)
        {
            currentState = BossState.Attack;
        }
        else
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(player.position);
            animationHandler.UpdateMovementSpeed(_speed);
        }
    }

    private void HandleAttackState(float distanceToPlayer)
    {
        RotateTowardsPlayer();
        if (timeBetweenAttacks > 0f) timeBetweenAttacks -= Time.deltaTime;
        else timeBetweenAttacks = attackTimer;
        if (distanceToPlayer > attackDistance)
        {
            _navMeshAgent.isStopped = false;
            currentState = BossState.Chase;
        }
        else
        {
            _navMeshAgent.isStopped = true;
            animationHandler.UpdateMovementSpeed(_speed);
            if (timeBetweenAttacks <= 0f)
            {
                PerformAttack();
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        }
    }

    private void PerformAttack()
    {
        animationHandler.SetAttackTrigger();
    }

    public void AttackSphere()
    {
        GameObject sphereObject = new GameObject("AttackSphere");
        sphereObject.transform.position = spherePosition.transform.position;
        
        SphereCollider sphereCollider = sphereObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = sphereRadius;
        
        SphereDamage sphereDamage = sphereObject.AddComponent<SphereDamage>();
        sphereDamage.Initialize(damage);
        Destroy(sphereObject, sphereDuration);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}