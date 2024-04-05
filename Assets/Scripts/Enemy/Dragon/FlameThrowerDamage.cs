using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerDamage : MonoBehaviour
{
    [SerializeField] private float damagePerSecond = 25f; // Damage per second inflicted by the flamethrower
    [SerializeField] PlayerHealth playerhealth; // Reference to the PlayerHealth script

    public bool isBurning = false; // Flag to track if the player is currently burning

    
    private void Awake()
    {
        playerhealth = FindObjectOfType<PlayerHealth>(); // Find the player health script in the scene
    }

    // Method called when a collider enters the trigger collider of the fire object
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collider belongs to the player
        {
            Debug.Log("Player entered Fire");
            isBurning = true; // Set isBurning to true
            playerhealth.PlayDamageSound(playerhealth.TakeFireDamageSoundClip); // Play fire damage sound
            StartCoroutine(DealDamageOverTime(other)); // Start coroutine to deal damage over time
        }
    }

    // Method called when a collider exits the trigger collider of the fire object
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collider belongs to the player
        {
            Debug.Log("Player exited Fire");
            isBurning = false; // Set isBurning to false
            playerhealth.PlayDamageSound(playerhealth.TakeFireDamageSoundClip); // Play fire damage sound
        }
    }

    // Coroutine to deal damage over time to the player while inside the fire
    private IEnumerator DealDamageOverTime(Collider playerCollider)
    {
        while (isBurning)
        {
            IDamage damageable = playerCollider.GetComponent<IDamage>(); // Get the component that implements the IDamage interface from the player's collider

            if (damageable != null)
            {
                float damage = damagePerSecond * Time.deltaTime; // Calculate damage per frame based on the damage per second
                damageable.TakeDamage(damage); // Call the TakeDamage method of the IDamage interface
            }

            yield return null; // Wait for the next frame
        }
    }
}
