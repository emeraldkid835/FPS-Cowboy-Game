using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Damageables
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private PlayerHealth playerhealth;

        void Start()
        {
            // Ensure the healthText and playerDamageable are properly assigned in the Unity Editor
            if (healthText == null || playerhealth == null)
            {
                Debug.LogError("HealthDisplay: Assign the healthText and playerDamageable in the Unity Editor!");
                return;
            }

            // Subscribe to the damage event to update health when damage is taken
            DamageSystem.DamageEvent.OnPlayerDamaged += UpdateHealthText;

            // Set the initial health text
            UpdateHealthText(playerhealth.GetPlayerCurrentHealth());
        }

        private void Update()
        {
            UpdateHealthText(playerhealth.GetPlayerCurrentHealth());
        }

        void UpdateHealthText(float PlayercurrentHealth)
        {
            // Update the health text
            healthText.text = $"Health: {PlayercurrentHealth}";
        }

        void OnDestroy()
        {
            // Unsubscribe from the damage event to prevent memory leaks
            DamageSystem.DamageEvent.OnPlayerDamaged -= UpdateHealthText;
        }
    }
}
