using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageableObjectSettings", menuName = "Damage System/DamageableObjectSettings")]
public class DamageableObjectSettings : ScriptableObject
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float initialHealth = 100f;

    [Header("Visual and Audio Feedback")]
    public AudioClip damageSound;
    public GameObject damageParticlesPrefab;
}
