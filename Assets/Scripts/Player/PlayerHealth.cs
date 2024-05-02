using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


    public class PlayerHealth : MonoBehaviour, IDamage // Declaring that it is an IDamage, which needs to incorporate the method from the IDamage interface
    {
        [Header("Settings")]
        [SerializeField] public float PlayerstartHealth = 100f; // Players starting health that can be changed in the inspector
        [SerializeField] public float Playercurrenthealth;      // Players current health at a given time

        
        

        [Header("Audio & Visual")]
        [SerializeField] private AudioSource audioSource;    
        [SerializeField] public AudioClip TakeFireDamageSoundClip;
        [SerializeField] public AudioClip RestoreHealthSoundClip;
        [SerializeField] private PostProcessProfile healthVFX;
        private HealthDisplay healthDisplay;    // Reference to my HealthDisplay script that handles my UI changes
        [SerializeField] private FireHazard fireHazard;  // Reference to my FireHazard Script
        [SerializeField] private AudioSource hurtSound;

        public bool isPlayerDead = false;  

        void Start()
        {
            

       
            audioSource = GetComponent<AudioSource>(); // Get audio source, don't have one? add one
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            if (PlayerPrefs.HasKey("PlayerHealth"))
            {
                Playercurrenthealth = PlayerPrefs.GetFloat("PlayerHealth");
            }
            else
            {
                Playercurrenthealth = PlayerstartHealth;
            }

            UpdateHealthFX();
            
            

            // Find the HealthDisplay script in the scene
            healthDisplay = FindObjectOfType<HealthDisplay>();
            
            if (healthDisplay == null)  // If the health script is not attached, tell the user that its not found
            {
                Debug.LogError("PlayerHealth: HealthDisplay script not found in the scene!");
            }
            // Find the FireHazard Script in the scene
            fireHazard = FindObjectOfType<FireHazard>();
            if (fireHazard == null)
            {
                Debug.Log("No fire hazard in scene");
            }
        }

    void OnDisable()
    {
        PlayerPrefs.SetFloat("PlayerHealth", Playercurrenthealth);
        PlayerPrefs.Save();
    }

    public void UpdateHealthFX()
    {
        healthVFX.GetSetting<ColorGrading>().colorFilter.value.g = (Playercurrenthealth / PlayerstartHealth);
        healthVFX.GetSetting<ColorGrading>().colorFilter.value.b = ((Playercurrenthealth / PlayerstartHealth));
        if(Playercurrenthealth <= 0)
        {
            Die();
        }
    }

    private void ResetHealthFX()
    {
        healthVFX.GetSetting<ColorGrading>().colorFilter.value.g = 1;
        healthVFX.GetSetting<ColorGrading>().colorFilter.value.r = 1;
        healthVFX.GetSetting<ColorGrading>().colorFilter.value.b = 1;
    }

    // IDamageable interface method
    public void TakeDamage(float damage, IDamage.DamageType damageType)
        {   

            Debug.Log("Player is taking damage from the PlayerHealth Script");
            //PlayDamageSound(TakeDamageSoundClip);// Visual and audio feedback using Scriptable Object settings
            Playercurrenthealth -= damage; // Playercurrenthealth = Playercurrenthealth - damage
            if(hurtSound != null && audiomanager.instance.alreadyPlaying(hurtSound.clip) == false)
            {
                audiomanager.instance.PlaySFX3D(hurtSound.clip, this.transform.position, 1, 0.9f, 1.1f);
            }
            UpdateHealthFX();
            //SpawnDamageParticles();

            healthDisplay.FlashBlood();  // calls my blood effect method from the HealthDisplay script

            if (Playercurrenthealth <= 0) // If the players current health drops to or below zero, begin the die method
            {
                Debug.Log("Player Is Dead");
                Die();
            }
        }

        public float GetPlayerCurrentHealth() // Easy way to access Playerhealth values from other scripts if needed
        {
            return Playercurrenthealth;
        }

        public float GetPlayerInitialHealth()
        {
            return PlayerstartHealth;
        }

        public void RestoreHealth(float amount)
        {
            if (Playercurrenthealth < PlayerstartHealth)
            {
                Playercurrenthealth = Mathf.Min(Playercurrenthealth + amount, PlayerstartHealth);
                PlayRestoreHealthSound(RestoreHealthSoundClip);
                UpdateHealthFX();
            }
            
        }

        public void AlterMaxHealth(float amount)
        {
            PlayerstartHealth += amount;
            UpdateHealthFX();
        }

        // Method to play damage sound
        public void PlayDamageSound(AudioClip sound)
        {
            if (sound != null && fireHazard != null )
            {
                if (fireHazard.isBurning == true)
                {
                    audioSource.clip = sound;
                    audioSource.loop = true;
                    audioSource.Play();
                }

                if (fireHazard.isBurning == false)
                {
                    audioSource.clip = sound;
                    audioSource.loop = false;
                    audioSource.Stop();
                    Debug.Log("should stop the damagesound");
                }
                
                
            }
        }
        
        private void PlayRestoreHealthSound(AudioClip sound)
        {
            if (sound != null)
            {
                audioSource.clip = sound;
                audioSource.Play();
            }
                
        }
        

        // Method to spawn damage particles
        private void SpawnDamageParticles(GameObject particlesPrefab) // In this case I did not add particles yet so this method is unused
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
            Debug.Log("Player Is Dead");
            isPlayerDead = true;
            audiomanager.instance.SetSoundValid(false);
            // Implement player death logic here
            // Example: Destroy(gameObject);
        }

        
    }

