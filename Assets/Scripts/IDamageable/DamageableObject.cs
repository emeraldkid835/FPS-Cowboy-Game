using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damageables
{


    public class DamageableObject : MonoBehaviour, DamageSystem.IDamageable
    {
        [Header("Settings")]
        [SerializeField] DamageableObjectSettings settings;

        [SerializeField] public float currentHealth;

        private bool hasDied = false;

        private GameObject deathParticlesInstance;
        private GameObject damageParticlesInstance;
        private bool hasPlayedDeathParticles = false;
        private bool hasPlayedDamageParticles = false;

        private AudioSource audioSource;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            currentHealth = settings.initialHealth;
            DamageSystem.DamageEvent.OnDamageTaken += TakeDamage;
        }

        // IDamageable interface method
        public void TakeDamage(float damage)
        {
            Debug.Log($"{gameObject.name} took {damage} damage. Current Health: {currentHealth}");
            currentHealth -= damage;

            // Visual and audio feedback using Scriptable Object settings
            PlayDamageSound(settings.damageSound);
            SpawnDamageParticles(settings.damageParticlesPrefab);

            if (currentHealth <= 0)
            {
                PlayDamageSound(settings.damageSound);
                Die();
                
            }
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        // Method to play damage sound
        private void PlayDamageSound(AudioClip sound)
        {
            if (sound != null)
            {
                audioSource.enabled = true;
                
                
                audioSource.clip = settings.damageSound;
                audioSource.Play();
                
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
                if (!hasPlayedDamageParticles)
                    damageParticlesInstance = Instantiate(particlesPrefab, transform.position, Quaternion.identity);
                Destroy(damageParticlesInstance, settings.particleDestroyDelay);
            }
        }

       
        // Method to handle object destruction
        private void Die()
        {
            if (!hasDied)
            {
                hasDied = true;
                
                // Check if death particles have not been played before
                if (!hasPlayedDeathParticles && settings.DestroyParticlesPrefab != null)
                {
                    // Instantiate and store a reference to the death particle effect
                    deathParticlesInstance = Instantiate(settings.DestroyParticlesPrefab, transform.position, Quaternion.identity);

                    // Set the flag to indicate that death particles have been played
                    hasPlayedDeathParticles = true;

                    // Destroy the instantiated particles after a delay
                    Destroy(deathParticlesInstance, settings.particleDestroyDelay);
                }

                // Implement object death logic here
                Destroy(gameObject);
            }


        }

        void OnDestroy()
        {
            // Unsubscribe from the damage event to prevent memory leaks
            DamageSystem.DamageEvent.OnDamageTaken -= TakeDamage;
        }
    }
}
