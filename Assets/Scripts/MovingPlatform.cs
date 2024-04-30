using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float speed = 2.0f;

    private void Start()
    {
        transform.position = startPosition;
    }

    private void Update()
    {
        // Move the platform between start and end positions
        transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.PingPong(Time.time * speed, 1.0f));
    }
}
