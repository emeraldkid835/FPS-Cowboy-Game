using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickKey : MonoBehaviour
{
    public GameObject OpenDoor; // Reference to the GameObject with the OpenDoor script
    public GameObject keyGone; // Reference to the key GameObject
    private bool hasKey = false;
    private bool isNearDoor = false;

    void Update()
    {
        // Check if the player is near the door and has the key, and presses "E" to unlock it
        if (isNearDoor && hasKey && Input.GetKeyDown(KeyCode.E))
        {
            OpenDoor.GetComponent<OpenDoor>().UnlockDoor(); // Call UnlockDoor method in OpenDoor script
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the player is near the door
            isNearDoor = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset the flag when the player moves away from the door
            isNearDoor = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasKey)
        {
            // If the player interacts with the key, pick it up
            PickUpKey();
        }
    }

    void PickUpKey()
    {
        hasKey = true;
        keyGone.SetActive(false); // Deactivate the key GameObject
    }
}