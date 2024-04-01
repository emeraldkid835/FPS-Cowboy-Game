using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_on_Death_Damageable : DamageableObject
{
    [SerializeField] private Animation deathAnim;
    

    protected override void Die()
    {
        if (!hasDied)
        {
            hasDied = true;

            // Check if death particles have not been played before
            if (!hasPlayedDeathParticles && deathParticles != null)
            {
                // Instantiate and store a reference to the death particle effect
                deathParticlesInstance = Instantiate(deathParticles, transform.position, Quaternion.identity);

                //explodeDamage.DealDamageInRadius();

                // Set the flag to indicate that death particles have been played
                hasPlayedDeathParticles = true;



                // Destroy the instantiated particles after a delay
                Destroy(deathParticlesInstance, particlesDestroyDelay);
            }

            if (deathAnim != null)
            {
                deathAnim.Play(); //do an anim, assuming it exists
            }
            InvokeRepeating("Ticker", 0.5f, 0.5f); //check anim stopped to destroy it
        }
        


    }

    private void Ticker()
    {
        if(deathAnim.isPlaying == false || deathAnim == null)
        {
            Destroy(this.gameObject);
        }
    }
}

