using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EyeballEnemy : MonoBehaviour, IDamage // Declaring that it is an IDamage, which needs to incorporate the method from the IDamage interface
{
    [Header("Settings")]
    public GameObject bloodEffectPrefab;
    public Transform bloodEffectLocation;
    private EnemyAI enemyai;
    [SerializeField] public float bloodEffectDuration = 3f;
    [SerializeField] public float EnemystartHealth = 50f; // Enemy starting health

    [SerializeField] AudioSource eyeballHurtSound;

    public Animator animator;
    public NavMeshAgent agent;
    

    [SerializeField] public float EnemycurrentHealth; // Enemy current health at a given time

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyai = GetComponent<EnemyAI>();
        
    }

    public void Update()
    {
        if (EnemycurrentHealth <= 15f && !enemyai.isDoneRetreating)
        {
            enemyai.isRetreating = true;
        }
        else
        {
            enemyai.isRetreating = false;
        }
    }
    public void Start()
    {
        EnemycurrentHealth = EnemystartHealth; // At start, sets the EnemycurrentHealth to the EnemystartHealth
    }
    public void TakeDamage(float damage, IDamage.DamageType damageType)  // IDamage interface method
    {
        Debug.Log($"Enemy took {damage} damage. Enemy has {EnemycurrentHealth - damage} health.");
        animator.SetBool("TookHit", true);
        EnemycurrentHealth -= damage; // EnemycurrentHealth = EnemycurrentHealth - damage
        if(eyeballHurtSound != null)
        {
            audiomanager.instance.PlaySFX3D(eyeballHurtSound.clip, this.transform.position);
        }
        // Instantiate blood effect at the position where the enemy was hit
        GameObject bloodEffect = Instantiate(bloodEffectPrefab, bloodEffectLocation.position, Quaternion.identity);
        enemyai.hurt = true;
        // Destroy the blood effect after a delay
        Destroy(bloodEffect, bloodEffectDuration);

        if (EnemycurrentHealth <= 0) // If enemy health hits 0 or less, begin die method
        {
            Die();
        }
    }

    private void Die() // Simple die method that will be added to in the future
    {
        StartCoroutine(DieTimer());
    }

    IEnumerator DieTimer()
    {
        animator.SetBool("isDead", true);
        agent.SetDestination(transform.position);
        agent.Stop();
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);

    }

    
}


