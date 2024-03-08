using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHazard : MonoBehaviour
{
    [SerializeField] private float damagePerSecond = 1f;

    public bool isBurning = false;

    private void OnTriggerEnter(Collider other) // Method to detect if the Player is in the trigger collider of the fire object, and sets the isBurning boolean to true. allowing for the coroutine method to deal damage over time
    {
        if (other.CompareTag("Player")) // These two trigger methods can also have other tags to inflict damage to those IDamage objects
        {
            Debug.Log("Player entered Fire");
            isBurning = true;
            StartCoroutine(DealDamageOverTime(other));
        }
    }

    private void OnTriggerExit(Collider other) // This method detects when the player exits the fire, setting isBurning to false. 
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited Fire");
            isBurning = false;
        }
    }

    private IEnumerator DealDamageOverTime(Collider playerCollider) // Coroutine to deal damage over time to the player while he is in the fire.
    {
        while (isBurning)
        {
            IDamage damageable = playerCollider.GetComponent<IDamage>(); // Attempt to get the component that implements the IDamage interface from the player's collider

            if (damageable != null)
            {
                float damage = damagePerSecond * Time.deltaTime; // Calculates damage per frame based on the damage per second
                damageable.TakeDamage(damage); // IDamage interface method
            }

            yield return null;
        }
    }
}





