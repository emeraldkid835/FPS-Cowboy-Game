using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenEnemy : MonoBehaviour, IDamage
{
    [SerializeField] private AudioSource hurtSound;
    [Header("Settings")]
    public GameObject bloodEffectPrefab;
    public Transform bloodEffectLocation;
    [SerializeField] public float bloodEffectDuration = 3f;
    [SerializeField] public float EnemystartHealth = 500f; // Enemy starting health

    [SerializeField] public float EnemycurrentHealth; // Enemy current health at a given time

    [Header("Fireball Attack")]
    [SerializeField] public float fireballSpeed = 60f;
    public GameObject FireballPrefab;
    public GameObject FireballSpawnPoint;
    [SerializeField] public float TimeBetweenShots = .5f;


    public Animator animator;
    public NavMeshAgent agent;
    public Transform player;

    private bool isAttacking = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("GoodPlayer").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        EnemycurrentHealth = EnemystartHealth; // At start, sets the EnemycurrentHealth to the EnemystartHealth
    }

    // Update is called once per frame
    void Update()
    {
        bool isCurrentlyAttacking = animator.GetBool("isAttacking");
        if (isCurrentlyAttacking && !isAttacking)
        {
            StartCoroutine(ShootFireball());
        }
    }
    public void TakeDamage(float damage, IDamage.DamageType damageType)  // IDamage interface method
    {
        
        
        EnemycurrentHealth -= damage; // EnemycurrentHealth = EnemycurrentHealth - damage
        Debug.Log($"Enemy took {damage} damage. Enemy has {EnemycurrentHealth - damage} health.");
        if (hurtSound != null)
        {
            audiomanager.instance.PlaySFX3D(hurtSound.clip, this.transform.position);
        }
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
    public IEnumerator ShootFireball()
    {
        transform.LookAt(player);
        isAttacking = true;
        GameObject FireballInstance = Instantiate(FireballPrefab, FireballSpawnPoint.transform.position, FireballSpawnPoint.transform.rotation);
        Rigidbody FireballRigidbody = FireballInstance.GetComponent<Rigidbody>();

        if (FireballRigidbody != null)
        {

            FireballInstance.transform.LookAt(player);
            FireballRigidbody.AddForce(FireballInstance.transform.forward * fireballSpeed, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(TimeBetweenShots);

        isAttacking = false;


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
