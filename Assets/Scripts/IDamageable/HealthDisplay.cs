using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Damageables
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private DamageableObject playerDamageable;

        void Start()
        {
            // Ensure the healthText and playerDamageable are properly assigned in the Unity Editor
            if (healthText == null || playerDamageable == null)
            {
                Debug.LogError("HealthDisplay: Assign the healthText and playerDamageable in the Unity Editor!");
                return;
            }

            // Subscribe to the damage event to update health when damage is taken
            DamageSystem.DamageEvent.OnDamageTaken += UpdateHealthText;

            // Set the initial health text
            UpdateHealthText(playerDamageable.GetCurrentHealth());
        }

        void UpdateHealthText(float currentHealth)
        {
            // Update the health text
            healthText.text = $"Health: {currentHealth}";
        }

        void OnDestroy()
        {
            // Unsubscribe from the damage event to prevent memory leaks
            DamageSystem.DamageEvent.OnDamageTaken -= UpdateHealthText;
        }
    }
}
