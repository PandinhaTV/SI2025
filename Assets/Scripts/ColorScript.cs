using UnityEngine;

public class ColorScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;              // How fast it moves
    public Vector2 direction = Vector2.zero; // Direction (use x or y)
    public float startDelay = 1f;         // Wait before moving
    public float lifetime = 5f;           // Destroy after this many seconds

    private float timer = 0f;
    private bool isMoving = false;

    void Start()
    {
        // Start the destruction countdown
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Start moving after delay
        if (!isMoving && timer >= startDelay)
            isMoving = true;

        if (isMoving)
            transform.Translate(direction.normalized * speed * Time.deltaTime);
    }
}
