using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    //build a state pattern for the status of wielding sword etc.
    [SerializeField] private GameObject _currentWeapon;
    [SerializeField] private Transform _sheathSocket; 
    [SerializeField] private Transform _handSocket; 
    [SerializeField] private float _resetTime = 0.5f;
    
    [SerializeField] private Vector3 _handPositionOffset; 
    [SerializeField] private Vector3 _handRotationOffset; 

    [SerializeField] private Vector3 _sheathPositionOffset; 
    [SerializeField] private Vector3 _sheathRotationOffset;

    [SerializeField] private HealthBar _healthBar;
    
    private AnimationHandler _animatorHandler;
    private int _currentComboIndex = 0;
    private float _timeBetweenAttacks = 0.5f;
    private bool _isAttacking = false;

    private bool _isSwordDrawed = true;
    private bool _hasSword = true;
    public bool HasSword => _hasSword;
    
    private bool _isLocked;
    private bool _isDead;
    public bool IsDead => _isDead;
    private void OnEnable()
    {
        EventManager.OnAttackInputPerformed += OnAttack;
        EventManager.OnLockStateChanged += UpdateLockState;
        EventManager.OnDrawInputPerformed += OnDrawWeapon;
    }
    
    private void UpdateLockState(bool isLocked, GameObject target)
    {
        if (_isDead) return;
        _isLocked = isLocked; 
    }
    private void OnAttack()
    {
        if (_isAttacking || !_hasSword || _isDead) return;
        _timeBetweenAttacks = _resetTime;
        Debug.Log(_currentComboIndex + " " + _isAttacking);
        _animatorHandler.TriggerAttack(_currentComboIndex);
       
        _currentWeapon.GetComponent<Weapon>().Attack();
        _currentComboIndex++;
        if (_currentComboIndex >= _animatorHandler.ComboHashesLength) ResetCombo();
    }

    private void OnDrawWeapon()
    {
        if (_isLocked || _isDead) return;

        if(_isSwordDrawed && _hasSword)
        {
            _animatorHandler.TriggerUndrawSword();
        }
        else if(!_hasSword)
        {
            _animatorHandler.TriggerDrawSword();
        }
        
    }

    private void Start()
    {
        _animatorHandler = AnimationHandler.GetInstance(GetComponent<Animator>());
    }

    private void Update()
    {
        if (_isDead) return;
        if (_timeBetweenAttacks > 0)
        {
            _timeBetweenAttacks -= Time.deltaTime;
            if (_timeBetweenAttacks <= 0)
            {
                ResetCombo();
            }
        }
    }

    private void ResetCombo()
    {
        _currentComboIndex = 0;
        _timeBetweenAttacks = _resetTime;
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        _healthBar.OnUpdateHealth(health, maxHealth);
        Debug.Log($"{gameObject.name} has {health} HP left.");

        if (health <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        _animatorHandler.SetDeathStatus(true);
        _isDead = true;
        this.GetComponent<CapsuleCollider>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
    }

    //animation events
    public void StartAttack()
    {
        _isAttacking = true;
    }
    
    public void EndAttack()
    {
        _isAttacking = false;
    }

    public void UndrawingSword()
    {
        _isSwordDrawed = false;
        _hasSword = false;
        _animatorHandler.SetHavingWeaponBool(false);
    }

    public void AttachSwordToHand()
    {
        _currentWeapon.transform.SetParent(_handSocket);
        _currentWeapon.transform.localPosition = _handPositionOffset; 
        _currentWeapon.transform.localRotation = Quaternion.Euler(_handRotationOffset); 
    }

    public void AttachSwordToSheath()
    {
        _currentWeapon.transform.SetParent(_sheathSocket); 
        _currentWeapon.transform.localPosition = _sheathPositionOffset; 
        _currentWeapon.transform.localRotation = Quaternion.Euler(_sheathRotationOffset); 
    }

    public void DrawingSword()
    {
        _isSwordDrawed = true;
        _hasSword = true;
        _animatorHandler.SetHavingWeaponBool(true);
    }

    public void ResetDeathStatus()
    {
        _animatorHandler.SetDeathStatus(false);
    }

    private void OnDisable()
    {
        EventManager.OnAttackInputPerformed -= OnAttack;
        EventManager.OnLockStateChanged -= UpdateLockState;
        EventManager.OnDrawInputPerformed -= OnDrawWeapon;
    }
}
