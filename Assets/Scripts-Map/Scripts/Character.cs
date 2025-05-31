using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamagable
{
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;
    public abstract void TakeDamage(int damage);

    protected abstract void Die();
}
