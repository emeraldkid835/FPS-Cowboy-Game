using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damageables
{
    public class PlayerHealth : MonoBehaviour, DamageSystem.IDamageable
    {
        [Header("Settings")]
        [SerializeField] PlayerSettings playerSettings; // Reference to PlayerSettings Scriptable Object

        [SerializeField] public float Playercurrenthealth;

        public bool playerHurtSound = false;
        private AudioSource audioSource;
        private AudioClip TakeDamageSoundClip;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            Playercurrenthealth = playerSettings.PlayerinitialHealth;
            DamageSystem.DamageEvent.OnPlayerDamaged += TakeDamage;
        }

        // IDamageable interface method
        public void TakeDamage(float damage)
        {
            Debug.Log("Player is taking damage from the PlayerHealth Script");
            playerHurtSound = true;
            Playercurrenthealth -= damage;

            // Visual and audio feedback using Scriptable Object settings
            PlayDamageSound(playerSettings.takeFireDamageSound);
            SpawnDamageParticles(playerSettings.damageParticlesPrefab);

            if (Playercurrenthealth <= 0)
            {
                Debug.Log("Player Is Dead");
                Die();
            }
        }

        public float GetPlayerCurrentHealth()
        {
            return Playercurrenthealth;
        }

        // Method to play damage sound
        private void PlayDamageSound(AudioClip sound)
        {
            if (sound != null )
            {
                if (playerHurtSound == true)
                {
                    audioSource.clip = playerSettings.takeFireDamageSound;
                    audioSource.Play();
                }
                
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
