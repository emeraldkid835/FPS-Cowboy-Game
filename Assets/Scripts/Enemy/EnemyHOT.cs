using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHOT : MonoBehaviour
{
    [SerializeField] public float HealthOverTime = 25f;
    [SerializeField] CatfishEnemy catfishenemy;
    

    private bool isHealing = false;

    

   


    private void OnTriggerEnter(Collider other) // Method to detect if the Player is in the trigger collider of the health area object, and sets the isHealing boolean to true. allowing for the coroutine method to apply health over time
    {
        Debug.Log("yep");
        if (other.CompareTag("Enemy")) // These two trigger methods can also have other tags to add health to those IDamage objects
        {
            Debug.Log("Enemy entered Healing area");
            catfishenemy = other.gameObject.GetComponent<CatfishEnemy>();
            
            
            isHealing = true;
            //Play healing noise
            StartCoroutine(AddHealthOverTime());
        }
    }

    private void OnTriggerExit(Collider other) // This method detects when the player exits the healing area, setting isHealing to false. 
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy exited Healing area");
            isHealing = false;


        }
    }

    private IEnumerator AddHealthOverTime()
    {
        while (isHealing)
        {


            if (catfishenemy.EnemycurrentHealth < catfishenemy.EnemystartHealth) 
            {
                catfishenemy.EnemycurrentHealth = Mathf.Min(catfishenemy.EnemycurrentHealth + HealthOverTime * Time.deltaTime);
            }

            if (catfishenemy.EnemycurrentHealth > catfishenemy.EnemystartHealth)
            {
                catfishenemy.EnemycurrentHealth = catfishenemy.EnemystartHealth;
            }

            yield return null;
        }
    }
    
}
