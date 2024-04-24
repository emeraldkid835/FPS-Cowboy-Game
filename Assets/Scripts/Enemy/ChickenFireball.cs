using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IDamage;

public class ChickenFireball : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] public float damageAmount = 50f;
    [SerializeField] public float damageRadius = 5f;
    [SerializeField] private IDamage.DamageType damageType = IDamage.DamageType.Fire;
    [SerializeField] public GameObject damageParticle;
    [SerializeField] protected GameObject damageParticlesInstance;
    [SerializeField] public float particlesDestroyDelay = 3f;

    public void DealDamageInRadius()
    {
        // Find all colliders within the specified radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

        // Loop through all colliders found
        foreach (Collider collider in colliders)
        {
            
            // Check if the collider's GameObject implements the IDamage interface
            IDamage damageable = collider.GetComponent(typeof(IDamage)) as IDamage;
            if (damageable != null)
            {
                // Calculate the distance between this object and the damageable object
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                // Calculate the damage based on distance, for example, you can deal less damage to objects further away
                float actualDamage = damageAmount * (1f - distance / damageRadius);

                // Call the TakeDamage method on the damageable object
                damageable.TakeDamage(actualDamage, damageType);
                

            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        DealDamageInRadius();
        damageParticlesInstance = Instantiate(damageParticle, transform.position, Quaternion.identity);
        Destroy(damageParticlesInstance, particlesDestroyDelay);
        Destroy(this.gameObject);
    }
}
