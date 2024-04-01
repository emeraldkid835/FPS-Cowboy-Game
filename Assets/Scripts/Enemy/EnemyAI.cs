using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public Animator animator;

    public AudioClip attackClip;

    private AudioSource audioSource;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    


    private void Awake()
    {
        player = GameObject.Find("GoodPlayer").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (attackClip == null)
        {
            Debug.LogError("Need to assign AudioClip on EnemyAI");
        }
    }

    private void Update()
    {
        //Check For sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        

        

        if (!playerInSightRange && !playerInAttackRange)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttacking", false);
            Patrolling();
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            
            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", true);
            animator.SetBool("isAttacking", false);
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttacking", true);
            AttackPlayer();
        }
    }


    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint() //should probably use a start position, instead of current position, to prevent wandering to eternity
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
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
            // Play the AudioClip
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is null. Cannot play audio.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }




}
