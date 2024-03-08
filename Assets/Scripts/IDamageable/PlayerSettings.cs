using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerSettings", menuName = "Damage System/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Health Settings")]
    public float PlayermaxHealth = 150f;
    public float PlayerinitialHealth = 100f;
    

    [Header("Visual and Audio Feedback")]
    public AudioClip takebulletdamageSound;
    public AudioClip takeFireDamageSound;
    public GameObject damageParticlesPrefab;

    
}
