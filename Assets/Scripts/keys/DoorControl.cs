using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public AudioSource openSound;
    public AudioSource closeSound;
    private bool isLocked = true;

    // Call this method to unlock the door
    public void UnlockDoor()
    {
        isLocked = false;
    }

    // Call this method to lock the door
    public void LockDoor()
    {
        isLocked = true;
    }

    public void OpenDoor()
    {
        if (!isLocked)
        {
            // Open the door animation or logic here
            openSound.Play();
        }
    }

    public void CloseDoor()
    {
        // Close the door animation or logic here
        closeSound.Play();
    }
}
