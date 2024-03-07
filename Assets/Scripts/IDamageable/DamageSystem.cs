using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    // IDamageable interface
    public interface IDamageable
    {
        void TakeDamage(float damage);
    }

    // DamageEvent class
    public class DamageEvent
    {
        public delegate void DamageEventHandler(float damage);
        public static event DamageEventHandler OnDamageTaken;

        public static void TriggerDamage(float damage)
        {
            OnDamageTaken?.Invoke(damage);
        }
    }
}
