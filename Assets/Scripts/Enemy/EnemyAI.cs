using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    // References
    public NavMeshAgent agent; // Reference to the NavMeshAgent component
    public Transform player; // Reference to the player's transform
    public Animator animator; // Reference to the Animator component
    private AudioSource audioSource; // Reference to the AudioSource component

    // Audio
    public AudioClip attackClip; // Audio clip for enemy attack

    // Layers
    public LayerMask whatIsGround, whatIsPlayer; // Layer masks for ground and player detection

    // Waypoints
    public List<Transform> waypoints; // List of waypoints for patrolling
    private int currentWaypointIndex = 0; // Index of the current waypoint
    public float waypointStopDuration = 2f; // Duration to stop at each waypoint
    private bool isWaitingAtWaypoint = false; // Flag to indicate if currently waiting at a waypoint

    // Patrolling
    public Vector3 walkPoint; // Random point for patrolling (Isn't used in this ai script but on enemyspawner AI to deal with non existent waypoints)
    bool walkPointSet; // Flag to indicate if a walk point is set
    public float walkPointRange; // Range for setting walk points

    // Attacking
    public float timeBetweenAttacks; // Time between each attack
    bool alreadyAttacked; // Flag to indicate if already attacked

    // States
    public float sightRange, attackRange, sightAngle; // Sight and attack ranges
    public bool playerInSightRange, playerInAttackRange; // Flags indicating if player is in sight or attack range

    // Initialization
    private void Awake()
    {
        // Find references
        player = GameObject.Find("GoodPlayer").transform; // Find the player's transform by name
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        animator = GetComponent<Animator>(); // Get the Animator component

        // Add AudioSource if not already present
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = .5f;
        }

        // Check if attackClip is assigned
        if (attackClip == null)
        {
            Debug.LogError("Need to assign AudioClip on EnemyAI");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Check player's position relative to the enemy
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Handle AI behavior based on player's position
        if (isWaitingAtWaypoint && !playerInSightRange && !playerInAttackRange)
        {
            // Idle at waypoint
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttacking", false);
        }
        else if (!isWaitingAtWaypoint && !playerInSightRange && !playerInAttackRange)
        {
            // Patrol if player not detected
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttacking", false);
            Patrolling();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            // Chase player if in sight range
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", true);
            animator.SetBool("isAttacking", false);
            ChasePlayer();
        }
        else if (playerInAttackRange && playerInSightRange)
        {
            // Attack player if in attack range
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttacking", true);
            AttackPlayer();
        }
    }

    // Patrolling behavior
    private void Patrolling()
    {
        if (!isWaitingAtWaypoint)
        {
            if (waypoints.Count == 0)
            {
                Debug.LogWarning("No waypoints assigned!");
                return;
            }

            // Move towards the current waypoint
            agent.SetDestination(waypoints[currentWaypointIndex].position);

            // Check if arrived at waypoint
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 1f)
            {
                isWaitingAtWaypoint = true;
                StartCoroutine(StopAtWaypoint());
            }
        }
    }

    // Coroutine to stop at waypoints
    IEnumerator StopAtWaypoint()
    {
        if (isWaitingAtWaypoint)
        {
            yield return new WaitForSeconds(waypointStopDuration);
            isWaitingAtWaypoint = false;
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }

    // Search for a random walk point within range
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    // Chase the player
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    // Attack the player
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position); // Stop moving
        transform.LookAt(player); // Look at the player
        PlayAudioClip(attackClip); // Play attack audio

        if (!alreadyAttacked)
        {
            // Attack logic goes here
            Debug.Log("Enemy attacked");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    // Reset attack state
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // Play an audio clip
    public void PlayAudioClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            if (!alreadyAttacked)
            {
                StartCoroutine(soundDelay());
            }
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is null. Cannot play audio.");
        }
    }

    // Draw gizmos for sight and attack ranges
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    // Delay for playing audio
    private IEnumerator soundDelay()
    {
        yield return new WaitForSeconds(.5f);
        audioSource.Play();
    }
}
