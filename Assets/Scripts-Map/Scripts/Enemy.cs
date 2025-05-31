using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Enemy : Character, ITargetable
{
    [SerializeField] protected Transform player; 
    [SerializeField] protected float attackTimer;
    [SerializeField] protected float timeBetweenAttacks;

    [SerializeField] protected HealthBar healthBar;
    protected bool isDead = false;
    protected EnemyAnimation animationHandler;
    
    private void Start()
    {
        animationHandler = new EnemyAnimation(GetComponent<Animator>());
        healthBar.OnUpdateHealth(health, maxHealth);
    }
    
    public override void TakeDamage(int damage)
    {
        
        animationHandler.TriggerGotHit();
        health -= damage;
        healthBar.OnUpdateHealth(health, maxHealth);
        timeBetweenAttacks = attackTimer;
        Debug.Log($"{gameObject.name} has {health} HP left.");

        if (health <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {   
        animationHandler.SetDeathStatus();
        isDead = true;
        this.GetComponent<CapsuleCollider>().enabled = false;
    
        //Temporary Solution
        if (TryGetComponent(out Boss boss))
        {
            int index = SceneManager.GetActiveScene().buildIndex + 1;
            StartCoroutine(LoadSceneAfterDelayCoroutine(index));
        }
    }
    
    private IEnumerator LoadSceneAfterDelayCoroutine(int sceneIndex)
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(sceneIndex);
    }

    /*private void ApplyKnockback()
    {
        Vector3 knockbackDirection = (transform.position - _player.position).normalized;
        knockbackDirection.y = 0; 

        float knockbackDistance = 1f; 
        transform.position += knockbackDirection * knockbackDistance;
    }*/
}
