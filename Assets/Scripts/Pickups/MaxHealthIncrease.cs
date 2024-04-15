using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MaxHealthIncrease : Pickup
{
    [SerializeField] private float increaseAmount = 15f;
    private PlayerHealth health;
    // Start is called before the first frame update
    void Start()
    {
        health = FindObjectOfType<PlayerHealth>();
    }

    public override void Collect()
    {
        health.AlterMaxHealth(increaseAmount);
        health.RestoreHealth(Mathf.Infinity);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
            audiomanager.instance.PlaySFX3D(pickupSound.clip, this.transform.position);
            Destroy(gameObject);
        }
    }
}
