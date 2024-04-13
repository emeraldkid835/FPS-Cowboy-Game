using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] public float damageRadius = 5f;
    [SerializeField] public float damageAmount = 10f;
    private DamageableObject damagableObject;
    [SerializeField] private IDamage.DamageType damageType = IDamage.DamageType.Fire;

    private void Awake()
    {
        damagableObject = GetComponent<DamageableObject>();
    }
    // Call this method to deal damage within the specified radius
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
                damagableObject.hasExploded = true;

            }
        }
    }

    private void OnDrawGizmos()
    {
        // Draw a wire sphere representing the damage radius in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
