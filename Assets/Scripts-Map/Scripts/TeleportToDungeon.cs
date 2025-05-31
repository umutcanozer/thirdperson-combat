using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToDungeon : MonoBehaviour
{
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 targetRotation;
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.transform.position = targetPosition;
            _player.transform.rotation = Quaternion.Euler(targetRotation);
        }
    }
}
