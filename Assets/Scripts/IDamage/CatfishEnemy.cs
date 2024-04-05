using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

 public class CatfishEnemy : MonoBehaviour, IDamage // Declaring that it is an IDamage, which needs to incorporate the method from the IDamage interface
 {
    [Header("Settings")]
    public GameObject bloodEffectPrefab; // Prefab for blood effect when hit
    public Transform bloodEffectLocation; // Location where the blood effect will be instantiated
    [SerializeField] public float bloodEffectDuration = 3f; // Duration of the blood effect
    [SerializeField] public float EnemystartHealth = 100f; // Enemy starting health

    public Animator animator; // Reference to the Animator component
    public NavMeshAgent agent; // Reference to the NavMeshAgent component
    public BoxCollider WeaponCollider; // Reference to the BoxCollider component for the enemy's weapon

    [SerializeField] public float EnemycurrentHealth; // Enemy current health at a given time

    
    private void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        WeaponCollider = GetComponentInChildren<BoxCollider>(); // Get the BoxCollider component of the child objects
        bloodEffectLocation = GameObject.Find("BloodeffectLocation").GetComponent<Transform>(); // Find and get the transform of the blood effect location
    }

    // Start is called before the first frame update
    public void Start()
    {
        EnemycurrentHealth = EnemystartHealth; // At start, sets the EnemycurrentHealth to the EnemystartHealth
    }

    // Method to take damage, required by the IDamage interface
    public void TakeDamage(float damage)
    {
        Debug.Log($"Enemy took {damage} damage. Enemy has {EnemycurrentHealth - 10f} health."); // Log the damage taken by the enemy
        EnemycurrentHealth -= damage; // Decrease the enemy's health by the amount of damage

        // Instantiate blood effect at the position where the enemy was hit
        GameObject bloodEffect = Instantiate(bloodEffectPrefab, bloodEffectLocation.position, Quaternion.identity);

        // Start coroutine to toggle the "isHit" animation state
        StartCoroutine(isHitToggle());

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
        yield return new WaitForSeconds(4f); // Wait for 4 seconds before destroying the enemy game object
        Destroy(gameObject); // Destroy the enemy game object
    }

    // Coroutine to toggle the "isHit" animation state
    IEnumerator isHitToggle()
    {
        animator.SetBool("isHit", true); // Set the "isHit" animation state to true

        yield return new WaitForSeconds(1); // Wait for 1 second
        animator.SetBool("isHit", false); // Set the "isHit" animation state to false
    }
 }

