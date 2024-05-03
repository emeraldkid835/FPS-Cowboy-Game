using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

 public class CatfishEnemy : MonoBehaviour, IDamage // Declaring that it is an IDamage, which needs to incorporate the method from the IDamage interface
 {
    [Header("Settings")]
    public GameObject bloodEffectPrefab;
    public Transform bloodEffectLocation;
    [SerializeField] public float bloodEffectDuration = 3f;
    [SerializeField] public float EnemystartHealth = 100f; // Enemy starting health

    [SerializeField] private AudioSource hurtSound;

    public Animator animator;
    public NavMeshAgent agent;
    public BoxCollider WeaponCollider;
    public EnemyAI enemyai;

    [SerializeField] public float EnemycurrentHealth; // Enemy current health at a given time

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        WeaponCollider = GetComponentInChildren<BoxCollider>();
        bloodEffectLocation = GameObject.Find("BloodeffectLocation").GetComponent<Transform>();
        enemyai = GetComponent<EnemyAI>();
    }
    public void Start()
    {
        EnemycurrentHealth = EnemystartHealth; // At start, sets the EnemycurrentHealth to the EnemystartHealth
    }
    public void TakeDamage(float damage, IDamage.DamageType damageType)  // IDamage interface method
    {
        Debug.Log($"Enemy took {damage} damage. Enemy has {EnemycurrentHealth - 10f} health.");
        EnemycurrentHealth -= damage; // EnemycurrentHealth = EnemycurrentHealth - damage

        if (hurtSound != null && audiomanager.instance.alreadyPlaying(hurtSound.clip) == false)
        {
            audiomanager.instance.PlaySFX3D(hurtSound.clip, this.transform.position);
        }
        // Instantiate blood effect at the position where the enemy was hit
        if (bloodEffectPrefab != null && bloodEffectLocation != null)
        {
            GameObject bloodEffect = Instantiate(bloodEffectPrefab, bloodEffectLocation.position, Quaternion.identity);
            StartCoroutine(isHitToggle());
            // Destroy the blood effect after a delay
            Destroy(bloodEffect, bloodEffectDuration);
        }
        enemyai.hurt = true;
          if (EnemycurrentHealth <= 0) // If enemy health hits 0 or less, begin die method
          {
              Die();
          }
    }

    public void Update()
    {
        if (EnemycurrentHealth <= 30f && !enemyai.isDoneRetreating)
        {
            enemyai.isRetreating = true;
        }
        else
        {
            enemyai.isRetreating = false;
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
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);

    }

    IEnumerator isHitToggle()
    {
        animator.SetBool("isHit", true);
        
        yield return new WaitForSeconds(1);
        animator.SetBool("isHit", false);

    }

   




}

