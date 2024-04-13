using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    public PlayerHealth playerHealth;
    [SerializeField] float Damage = 20f;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] IDamage.DamageType damageType = IDamage.DamageType.Sharp;

    private void Awake()
    {
        playerHealth = GameObject.Find("GoodPlayer").GetComponent<PlayerHealth>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy Hit the Player");
            if(hitSound != null)
            {
                audiomanager.instance.PlaySFX3D(hitSound.clip, this.transform.position);
            }
            playerHealth.TakeDamage(Damage, damageType);
        }
    }
}
