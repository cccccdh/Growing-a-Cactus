using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float scrollSpeed = 0.5f;

    private bool isMoving = false;
    private float width;
    private Vector3 startPosition;

    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        startPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            float newPosition = Mathf.Repeat(Time.time * scrollSpeed, width);
            transform.position = startPosition + Vector3.left * newPosition;

            if (transform.position.x <= startPosition.x - width)
            {
                startPosition = transform.position;
            }
        }
    }

    public void Moving()
    {
        isMoving = !isMoving;
    }
}
