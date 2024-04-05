using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireDragonEnemy : MonoBehaviour, IDamage // Declaring that it is an IDamage, which needs to incorporate the method from the IDamage interface
{
    [Header("Settings")]
    public GameObject bloodEffectPrefab; // Prefab for blood effect when hit
    public Transform bloodEffectLocation; // Location where the blood effect will be instantiated
    [SerializeField] public float bloodEffectDuration = 3f; // Duration of the blood effect
    [SerializeField] public float EnemystartHealth = 150f; // Enemy starting health

    [Header("Flamethrower Attack")]
    public GameObject flamethrowerPrefab; // Prefab for the flamethrower particle system
    public GameObject flamethrowerSpawnPoint; // Spawn point for the flamethrower
    [SerializeField] float flamethrowerDuration = 1.5f; // Duration of the flamethrower attack

    private bool isAttacking = false; // Flag to track if the enemy is currently attacking

    public Animator animator; // Reference to the Animator component
    public NavMeshAgent agent; // Reference to the NavMeshAgent component
    public EnemyAI enemyai; // Reference to the EnemyAI component

    [SerializeField] public float EnemycurrentHealth; // Enemy current health at a given time

    
    private void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        enemyai = GetComponent<EnemyAI>(); // Get the EnemyAI component
    }

    // Start is called before the first frame update
    public void Start()
    {
        EnemycurrentHealth = EnemystartHealth; // At start, sets the EnemycurrentHealth to the EnemystartHealth
    }

    // Update is called once per frame
    private void Update()
    {
        bool isCurrentlyAttacking = animator.GetBool("isAttacking"); // Check if the enemy is currently attacking
        if (isCurrentlyAttacking && !isAttacking)
        {
            StartCoroutine(InstantiateFlamethrowerAfterDelay()); // Start the coroutine to instantiate flamethrower after a delay
        }
    }

    // Method to take damage, required by the IDamage interface
    public void TakeDamage(float damage)
    {
        Debug.Log($"Enemy took {damage} damage. Enemy has {EnemycurrentHealth - damage} health."); // Log the damage taken by the enemy
        animator.SetBool("TookHit", true); // Trigger the "TookHit" animation state
        EnemycurrentHealth -= damage; // Decrease the enemy's health by the amount of damage

        // Instantiate blood effect at the position where the enemy was hit
        GameObject bloodEffect = Instantiate(bloodEffectPrefab, bloodEffectLocation.position, Quaternion.identity);

        // Destroy the blood effect after a delay
        Destroy(bloodEffect, bloodEffectDuration);

        // Check if the enemy's health is less than or equal to 0
        if (EnemycurrentHealth <= 0)
        {
            Die(); // If health is 0 or less, initiate the die method
        }
    }

    // Method to handle enemy death
    private void Die()
    {
        StartCoroutine(DieTimer()); // Start coroutine for handling enemy death
    }

    // Coroutine to handle the death timer and cleanup
    IEnumerator DieTimer()
    {
        animator.SetBool("isDead", true); // Trigger the "isDead" animation state
        agent.SetDestination(transform.position); // Stop the agent's movement
        agent.Stop(); // Stop the NavMeshAgent
        yield return new WaitForSeconds(3f); // Wait for 3 seconds before destroying the enemy game object
        Destroy(gameObject); // Destroy the enemy game object
    }

    // Coroutine to instantiate the flamethrower after a delay
    private IEnumerator InstantiateFlamethrowerAfterDelay()
    {
        // Set the flag to true to prevent repeated instantiations
        isAttacking = true;

        // Instantiate flamethrower particle system at the spawn point
        GameObject flamethrowerInstance = Instantiate(flamethrowerPrefab, flamethrowerSpawnPoint.transform.position, flamethrowerSpawnPoint.transform.rotation);
        flamethrowerInstance.transform.parent = flamethrowerSpawnPoint.transform; // Set the flamethrower's parent to the spawn point
        Debug.Log("Flamethrower instantiated");

        // Wait for the specified duration of the flamethrower attack
        yield return new WaitForSeconds(flamethrowerDuration);

        // Destroy the flamethrower particle system after the attack duration
        Destroy(flamethrowerInstance);
        Debug.Log("Flamethrower stopped");

        // Reset the flag after the attack is complete
        isAttacking = false;
    }
}
