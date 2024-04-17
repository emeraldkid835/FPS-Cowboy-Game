using UnityEngine;

public class DoorScripts : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Remove the door object
            Destroy(gameObject);
        }
    }
}
