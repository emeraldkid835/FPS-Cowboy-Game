using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 startPosition;
    public Vector3 endOffset;
    public float speed = .5f;
    

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Move the platform between start and end positions
        transform.position = Vector3.Lerp(startPosition, startPosition + endOffset, Mathf.PingPong(Time.time * speed, 1.0f));
    }
}
