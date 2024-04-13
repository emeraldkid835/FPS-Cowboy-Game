using UnityEngine;

public class DoorScripts : MonoBehaviour
{
    public bool isOpen = false;

    public void OpenDoor()
    {
        if (!isOpen)
        {
            transform.Translate(new Vector3(0, 5, 0)); // Move the door up by 5 units
            isOpen = true;
        }
    }

    public void LockDoor()
    {
        isOpen = false; // Just sets the state, doesn't physically move the door back
    }
}
