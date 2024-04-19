using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIStationary : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public Animator animator;

    public AudioClip attackClip;

    private AudioSource audioSource;

    public LayerMask whatIsPlayer;


    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange, sightAngle;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("GoodPlayer").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.minDistance = 3;
            audioSource.volume = 1f;
        }

        if (attackClip == null)
        {
            Debug.LogError("Need to assign AudioClip on EnemyAI");
        }
        
        
    }

    private void Update()
    {
        bool isenemyDead = animator.GetBool("isDead");
        //Check For sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange &&  !playerInAttackRange)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isAttacking", false);
        }

        if (playerInAttackRange && playerInSightRange && !isenemyDead)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttacking", true);
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        // Stop the enemy from moving
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        PlayAudioClip(attackClip);

        if (!alreadyAttacked)
        {
            // Put Attack Code/Logic in here
            Debug.Log("Enemy attacked");



            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void PlayAudioClip(AudioClip clip)
    {
        // Ensure audioSource is not null
        if (audioSource != null && clip != null)
        {
            // Assign the AudioClip to the AudioSource
            audioSource.clip = clip;
            if (!alreadyAttacked)
            {
                // Play the AudioClip
                StartCoroutine(soundDelay());
            }

        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is null. Cannot play audio.");
        }
    }
    private IEnumerator soundDelay()
    {
        yield return new WaitForSeconds(.5f);
        audiomanager.instance.PlaySFX3D(attackClip, this.transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
