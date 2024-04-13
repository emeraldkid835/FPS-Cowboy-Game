using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthCollider : MonoBehaviour
{
    public PlayerHealth playerHealth;
    [SerializeField] float Damage = 10f;
    [SerializeField] IDamage.DamageType damageType = IDamage.DamageType.Sharp;
    [SerializeField] AudioSource hittheMF;

    private void Awake()
    {
        playerHealth = GameObject.Find("GoodPlayer").GetComponent<PlayerHealth>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy Hit the Player");
            audiomanager.instance.PlaySFX3D(hittheMF.clip, this.transform.position);
            playerHealth.TakeDamage(Damage, damageType);
        }
    }
}
