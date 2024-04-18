using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireball : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] public float DamageAmount = 50f;

    private void OnCollisionEnter(Collision collision)
    {
        IDamage damageable = collision.gameObject.GetComponent<IDamage>();
        Debug.Log("Enemy Fireball hit something");
        
        if (damageable != null)
        {
            damageable.TakeDamage(DamageAmount, IDamage.DamageType.Fire);
            Debug.Log("Hit damageable, Should Take Damage");
        }

        Destroy(gameObject);
    }
}
