using UnityEngine;

public class DoorControllers : MonoBehaviour
{
    public Animation doorAnimation;
    private bool isOpening = false;

    // Update is called once per frame
    void Update()
    {
        if (isOpening)
        {
            // Check if the door animation has finished playing
            if (!doorAnimation.isPlaying)
            {
                isOpening = false;
            }
        }
    }

    // Call this method to open the door
    public void OpenDoor()
    {
        if (!isOpening)
        {
            // Play the door animation
            doorAnimation.Play();
            isOpening = true;

            // You can add sound effects or other effects here if needed
        }
    }
}
