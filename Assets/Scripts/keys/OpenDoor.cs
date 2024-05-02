using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour, IInteract
{
    public Animation doorHinge;
    private bool isLocked = true; // Assuming the door starts locked
    private int keysCollected = 0; // Number of keys collected by the player
    private bool isOpen = false;
    private bool inRange = false;

    // Add references to your audio clips and AudioSource
    public AudioClip openSound;
    public AudioClip lockedSound;
    private AudioSource audioSource;

    void Start()
    {
        isOpen = false;
        isLocked = true;
        // Initialize the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        { //could just use [Requirecomponent(typeof(AudioSource))]
            // Log an error if no AudioSource is found
            Debug.LogError("No AudioSource component found on the GameObject.");
        }
    }

    

    public void Interaction() //this shit used to be in update!
    {
        if (isLocked == false)
        {
            doorHinge.Play();
            isOpen = true;
            audioSource.clip = openSound;
            
        }
        else
        {

            audioSource.clip = lockedSound;
        }
        audiomanager.instance.PlaySFX(audioSource.clip);
    }

    public bool validToReinteract()
    {
        return true;
    }

    public string contextText()
    {
        return "Unlock";
    }

    // Method to unlock the door
    public void UnlockDoor()
    {
        isLocked = false;
    }

    // Method to inform the door script that a key has been collected
    public void KeyCollected()
    {
        keysCollected++;
        if (keysCollected >= 2) // Check if the player has collected enough keys to unlock the door
        {
            UnlockDoor();
        }
    }

}
