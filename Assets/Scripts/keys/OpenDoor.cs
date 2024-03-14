using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Animation doorHinge;
    private bool isLocked = true; // Assuming the door starts locked

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isLocked)
            {
                doorHinge.Play();
            }
            else
            {
                Debug.Log("The door is locked. You need a key to unlock it.");
            }
        }
    }

    // Method to unlock the door
    public void UnlockDoor()
    {
        isLocked = false;
    }
}