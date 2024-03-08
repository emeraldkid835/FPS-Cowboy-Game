using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damageables
{


    public class FireHazard : MonoBehaviour
    {
        [SerializeField] private float damagePerSecond = 1f;

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player is in Fire");
                DamageSystem.DamageEvent.TriggerPlayerDamageOverTime(damagePerSecond);
            }
        }
    }
}
