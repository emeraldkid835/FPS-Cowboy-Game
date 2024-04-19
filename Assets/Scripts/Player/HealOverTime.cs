using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HealOverTime : MonoBehaviour
{
    [SerializeField] PlayerHealth playerhealth;
    [SerializeField] private float healthOverTime = 25f;
    [SerializeField] private AudioSource meSound;

    private float audioTimer;

    private bool isHealing = false;
   

    private void Awake()
    {
        playerhealth = FindObjectOfType<PlayerHealth>(); // Find the player health script in the scene.
        audioTimer = 0;
    
    }

    private void Update()
    {
        playerhealth.GetPlayerCurrentHealth();
    }

    private void OnTriggerEnter(Collider other) // Method to detect if the Player is in the trigger collider of the health area object, and sets the isHealing boolean to true. allowing for the coroutine method to apply health over time
    {
        if (other.CompareTag("Player")) // These two trigger methods can also have other tags to add health to those IDamage objects
        {
            Debug.Log("Player entered Healing area");
            
            isHealing = true;
            audioTimer = Mathf.Infinity;
            //Play healing noise
            StartCoroutine(AddHealthOverTime());
        }
    }

    private void OnTriggerExit(Collider other) // This method detects when the player exits the healing area, setting isHealing to false. 
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited Healing area");
            audioTimer = 0;
            isHealing = false;
            
        }
    }

    private IEnumerator AddHealthOverTime() 
    {
        while (isHealing)
        {
            

            if (playerhealth.Playercurrenthealth <= playerhealth.PlayerstartHealth) //<= for negative values being valid at full health
            {
                
                playerhealth.Playercurrenthealth = Mathf.Min(playerhealth.Playercurrenthealth + healthOverTime * Time.deltaTime); // Calculates health per frame based on the health per second
                playerhealth.UpdateHealthFX();
                audioTimer += Time.deltaTime;
                if (meSound != null)
                {
                    if (audioTimer >= meSound.clip.length)
                    {
                        
                        if (audiomanager.instance.alreadyPlaying(meSound.clip) == false)
                        {
                            audioTimer = 0;
                            audiomanager.instance.PlaySFX3D(meSound.clip, this.transform.position);
                        }
                    }
                }
            }

            if(playerhealth.Playercurrenthealth > playerhealth.PlayerstartHealth)
            {
                playerhealth.Playercurrenthealth = playerhealth.PlayerstartHealth;
            }

            yield return null;
        }
    }

}
