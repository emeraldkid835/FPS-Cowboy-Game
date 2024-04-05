using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script
    [SerializeField] float Damage = 20f; // Damage inflicted by the melee attack

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        playerHealth = GameObject.Find("GoodPlayer").GetComponent<PlayerHealth>(); // Find and get the PlayerHealth component from the player game object
    }

    // Method called when a collider enters the trigger collider of the melee weapon
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collider belongs to the player
        {
            Debug.Log("Enemy Hit the Player"); // Log that the player is hit by the enemy
            playerHealth.TakeDamage(Damage); // Inflict damage to the player
        }
    }
}
