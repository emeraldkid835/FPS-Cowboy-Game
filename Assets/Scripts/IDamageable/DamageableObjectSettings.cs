using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damageables
{


    [CreateAssetMenu(fileName = "NewDamageableObjectSettings", menuName = "Damage System/DamageableObjectSettings")]
    public class DamageableObjectSettings : ScriptableObject
    {
        [Header("Health Settings")]
        public float maxHealth = 100f;
        public float initialHealth = 100f;

        [Header("Visual and Audio Feedback")]
        public AudioClip damageSound;
        public GameObject damageParticlesPrefab;
        public GameObject DestroyParticlesPrefab;

        [Header("Particle System Settings")]
        [SerializeField] public float particleDestroyDelay = 3f;
    }
}
