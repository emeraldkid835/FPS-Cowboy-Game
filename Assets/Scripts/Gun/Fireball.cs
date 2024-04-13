using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float damageAmount = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") //do nothing if player, since it's a player projectile
        {
            Debug.Log("Fireball hit player");
            return;
        }
        else
        {
            // Check if the collided object implements the IDamage interface
            IDamage damageable = collision.gameObject.GetComponent<IDamage>();
            Debug.Log("Fireball hit something");

            // If the collided object implements the IDamage interface, apply damage
            if (damageable != null)
            {
                damageable.TakeDamage(damageAmount, IDamage.DamageType.Fire);
                Debug.Log("Hit damageable, Should Take Damage");
            }

            // Destroy the fireball on collision
            Destroy(gameObject);
        }
    }
}
