using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] float playerHealth = 100f;

    private Damageable damageable;
    // Start is called before the first frame update
    void Start()
    {
        damageable = new Damageable(playerHealth); // Initial health for the player
    }

    public void TakeDamage(float amount)
    {
        damageable.TakeDamage(amount);
    }
}
