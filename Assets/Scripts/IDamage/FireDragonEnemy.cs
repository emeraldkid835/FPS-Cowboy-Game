using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireDragonEnemy : MonoBehaviour, IDamage // Declaring that it is an IDamage, which needs to incorporate the method from the IDamage interface
{
    [Header("Settings")]
    public GameObject bloodEffectPrefab;
    public Transform bloodEffectLocation;
    [SerializeField] public float bloodEffectDuration = 3f;
    [SerializeField] public float EnemystartHealth = 150f; // Enemy starting health

    [Header("Flamethrower Attack")]
    public GameObject flamethrowerPrefab;
    public GameObject flamethrowerSpawnPoint;
    [SerializeField] float flamethrowerDuration = 3.5f;
    
    private bool isAttacking = false;

    public Animator animator;
    public NavMeshAgent agent;

    public EnemyAI enemyai;


    [SerializeField] public float EnemycurrentHealth; // Enemy current health at a given time

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyai = GetComponent<EnemyAI>();

    }
    public void Start()
    {
        EnemycurrentHealth = EnemystartHealth; // At start, sets the EnemycurrentHealth to the EnemystartHealth
    }

    private void Update()
    {
        bool isCurrentlyAttacking = animator.GetBool("isAttacking");
        if (isCurrentlyAttacking && !isAttacking)
        {
            StartCoroutine(InstantiateFlamethrowerAfterDelay());
        }
    }
    public void TakeDamage(float damage)  // IDamage interface method
    {
        Debug.Log($"Enemy took {damage} damage. Enemy has {EnemycurrentHealth - damage} health.");
        animator.SetBool("TookHit", true);
        EnemycurrentHealth -= damage; // EnemycurrentHealth = EnemycurrentHealth - damage

        // Instantiate blood effect at the position where the enemy was hit
        GameObject bloodEffect = Instantiate(bloodEffectPrefab, bloodEffectLocation.position, Quaternion.identity);

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


    private IEnumerator InstantiateFlamethrowerAfterDelay()
    {
        // Set the flag to true to prevent repeated instantiations
        isAttacking = true;

        
        

        // Instantiate flamethrower particle system
        GameObject flamethrowerInstance = Instantiate(flamethrowerPrefab, flamethrowerSpawnPoint.transform.parent.localPosition, flamethrowerSpawnPoint.transform.parent.localRotation);
        //flamethrowerInstance.transform.parent = flamethrowerSpawnPoint.transform;

        

        // Destroy the flamethrower particle system after a certain duration
        Destroy(flamethrowerInstance, flamethrowerDuration);
        yield return new WaitForSeconds(flamethrowerDuration);// Wait for the specified delay
        // Reset the flag after the instantiation is complete
        isAttacking = false;
    }

}
