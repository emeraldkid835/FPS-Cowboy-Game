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

    //Waypoints
    public List<Transform> waypoints;
    private int currentWaypointIndex = 0;
    [SerializeField] public float waypointStopDuration = 2f;
    private bool isWaitingAtWaypoint = false;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange, sightAngle;
    public bool playerInSightRange, playerInAttackRange;

    //Retreat
    public Transform RetreatWaypoint;
    public bool isRetreating = false;
    public bool isAtRetreat = false;
    public bool isDoneRetreating = false;
    

    


    private void Awake()
    {
        player = GameObject.Find("GoodPlayer").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = .5f;
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
        

        
        if (isWaitingAtWaypoint && !playerInSightRange && !playerInAttackRange && !isenemyDead)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttacking", false);
        }

        if (!isWaitingAtWaypoint && !playerInSightRange && !playerInAttackRange && !isenemyDead)
        {
            if (waypoints.Count >= 1)
            {
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalking", true);
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", false);
                Patrolling();
            }
            else
            {
                animator.SetBool("isIdle", true);
                animator.SetBool("isWalking", false);
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", false);
            }
           
        }
        if (playerInSightRange && !playerInAttackRange && !isenemyDead)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", true);
            animator.SetBool("isAttacking", false);
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange && !isenemyDead)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttacking", true);
            AttackPlayer();
        }
        if (isRetreating && RetreatWaypoint != null && !isDoneRetreating && !isenemyDead)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", true);
            animator.SetBool("isAttacking", false);

            Retreat();
        }

    }


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
        /*if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }*/
    }

    IEnumerator StopAtWaypoint()
    {
        Debug.Log("Coroutine Called");
        if (isWaitingAtWaypoint)
        {
            Debug.Log("StopAtWaypoint started");
            

            yield return new WaitForSeconds(waypointStopDuration);
            
            isWaitingAtWaypoint = false;

            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;

        
        
            Debug.Log("Moving to waypoint " + currentWaypointIndex);
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

    private void Retreat()
    {
        isAtRetreat = false;
        agent.SetDestination(RetreatWaypoint.position);
        if(Vector3.Distance(transform.position, RetreatWaypoint.position) < 1f)
        {
            isAtRetreat = true;
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttacking", false);
            StartCoroutine(WaitDelay());
        }
    }

    private void AttackPlayer()
    {
        
        // Stop the enemy from moving
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        
        //PlayAudioClip(attackClip);

        if (!alreadyAttacked)
        {
            // Put Attack Code/Logic in here
            Debug.Log("Enemy attacked");

            audiomanager.instance.PlaySFX3D(attackClip, this.transform.position, 1, 0.9f, 1.1f); //should fix weird ah attack sound calls

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    /*private bool PlayerInSightRange()
    {
        // Calculate direction towards the player
        Vector3 directionToPlayer = player.position - transform.position;

        // Check if the player is within the sight angle
        if (Vector3.Angle(transform.forward, directionToPlayer) < sightAngle / 2f)
        {
            RaycastHit hit;

            // Cast a ray towards the player
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, sightRange, whatIsPlayer))
            {
                // Check if the ray hit the player
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }

        return false;
    }*/

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        
    }

    private IEnumerator soundDelay()
    {
        yield return new WaitForSeconds(.5f);
        audioSource.Play();
    }

    private IEnumerator WaitDelay()
    {

        yield return new WaitForSeconds(4f);
        isDoneRetreating = true;
        isRetreating = false;
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isChasing", true);
        animator.SetBool("isAttacking", false);
        ChasePlayer();
    }




}
