using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChickenAI : MonoBehaviour
{

    public NavMeshAgent agent;

    public Transform player;

    public Animator animator;
    
    public AudioClip attackClip;

    private AudioSource audioSource;

    public LayerMask whatIsGround, whatIsPlayer;

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


        if (!playerInAttackRange && !playerInSightRange && !isenemyDead)
        {
            animator.SetBool("isAttacking", false);
        }
        

        
        

        if (playerInAttackRange && playerInSightRange && !isenemyDead)
        {

            animator.SetBool("isAttacking", true);
            
            
            AttackPlayer();
        }
        /*if (isRetreating && RetreatWaypoint != null && !isDoneRetreating && !isenemyDead)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", true);
            animator.SetBool("isAttacking", false);

            Retreat();
        }*/

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
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

   

    private IEnumerator soundDelay()
    {
        yield return new WaitForSeconds(.5f);
        audioSource.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
