using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RabbitTrace : MonoBehaviour
{
    [SerializeField] private Transform _destination;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _distanceRadius = 5f;
    
    private NavMeshAgent _nma;
    private Animator _anim;
    private static readonly int _speed = Animator.StringToHash("speed");

    private void Start()
    {
        _nma = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        _anim.SetFloat(_speed, _nma.velocity.magnitude);
        
        if (distance < _distanceRadius)
        {
            _nma.isStopped = false;
            _nma.SetDestination(_destination.position);
        }else if(Vector3.Distance(transform.position, _destination.position) < 1f)
        {
            _nma.isStopped = true;
            Debug.Log("Reached");
        }
        else
        {
            _nma.isStopped = true;
        }
        
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _distanceRadius);
    }
}
