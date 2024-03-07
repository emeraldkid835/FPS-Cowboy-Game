using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableObject : MonoBehaviour, DamageSystem.IDamageable
{
    [Header("Settings")]
    [SerializeField] DamageableObjectSettings settings;

    private float currentHealth;

    void Start()
    {
        currentHealth = settings.initialHealth;
        DamageSystem.DamageEvent.OnDamageTaken += TakeDamage;
    }

    // IDamageable interface method
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Visual and audio feedback using Scriptable Object settings
        PlayDamageSound(settings.damageSound);
        SpawnDamageParticles(settings.damageParticlesPrefab);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to play damage sound
    private void PlayDamageSound(AudioClip sound)
    {
        if (sound != null)
        {
            // Play the sound
            // Example: AudioSource.PlayOneShot(sound);
        }
    }

    // Method to spawn damage particles
    private void SpawnDamageParticles(GameObject particlesPrefab)
    {
        if (particlesPrefab != null)
        {
            // Instantiate and spawn particles using the provided prefab
            // Example: Instantiate(particlesPrefab, transform.position, Quaternion.identity);
        }
    }

    // Method to handle object destruction
    private void Die()
    {
        // Implement object death logic here
        // Example: Destroy(gameObject);
    }

    void OnDestroy()
    {
        // Unsubscribe from the damage event to prevent memory leaks
        DamageSystem.DamageEvent.OnDamageTaken -= TakeDamage;
    }
}
