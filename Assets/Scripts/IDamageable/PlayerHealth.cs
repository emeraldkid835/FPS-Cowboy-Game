using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damageables
{


    public class PlayerHealth : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] PlayerSettings playerSettings; // Reference to PlayerSettings Scriptable Object

        private float currentHealth;

        void Start()
        {
            currentHealth = playerSettings.initialHealth;
            DamageSystem.DamageEvent.OnDamageTaken += TakeDamage;
        }

        // IDamageable interface method
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;

            // Visual and audio feedback using Scriptable Object settings
            PlayDamageSound(playerSettings.damageSound);
            SpawnDamageParticles(playerSettings.damageParticlesPrefab);

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

        // Method to handle player death
        private void Die()
        {
            // Implement player death logic here
            // Example: Destroy(gameObject);
        }

        void OnDestroy()
        {
            // Unsubscribe from the damage event to prevent memory leaks
            DamageSystem.DamageEvent.OnDamageTaken -= TakeDamage;
        }
    }
}
