using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    // Button Activator Effect
    public enum ActivatorEffect { WorldReflection, MapReflection, BoxSpawner}

    [SerializeField] private ActivatorEffect activatorEffect;

    // PUBLIC
    public bool reflectXAxis, reflectYAxis;
    public Transform boxSpawnPoint;
    public GameObject box;
    public float pressSpeed;

    // PRIVATE
    private GameObject map, currentBox;
    private Rigidbody2D rb;
    private int movementDirection;
    //private bool isWeighted = false, canActivate = true; // If the button is weighted down by a box or player
    private float activationHeight, originalHeight;

    void Start()
    {
        map = GameObject.Find("Map"); // Find the map object in the scene
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        originalHeight = rb.position.y; // Store the original height of the button
        activationHeight = originalHeight - 0.25f; // Set the activation height to the initial position of the button
    }

    void FixedUpdate()
    {
        rb.linearVelocityY = pressSpeed * movementDirection;
    }

    public void ActivateButton()
    {
        switch (activatorEffect)
        {
            // World Reflection: Reflect the world and player position based on the selected axes
            case ActivatorEffect.WorldReflection:
                if (reflectXAxis) // X-Axis Reflection
                    map.transform.localScale = new Vector3(map.transform.localScale.x, -map.transform.localScale.y, map.transform.localScale.z); // Reflect the world vertically
                    PlayerLogic.instance.transform.position = new Vector2(PlayerLogic.instance.transform.position.x, -PlayerLogic.instance.transform.position.y); // Reflect the player's position
                if (reflectYAxis) // Y-Axis Reflection
                    map.transform.localScale = new Vector3(-map.transform.localScale.x, map.transform.localScale.y, map.transform.localScale.z); // Reflect the world horizontally
                    PlayerLogic.instance.transform.position = new Vector2(-PlayerLogic.instance.transform.position.x, PlayerLogic.instance.transform.position.y); // Reflect the player's position

                break;

            // Map Reflection: Reflect the map based on the selected axes
            case ActivatorEffect.MapReflection:
                if (reflectXAxis) // X-Axis Reflection
                    map.transform.localScale = new Vector3(map.transform.localScale.x, -map.transform.localScale.y, map.transform.localScale.z); // Reflect the world vertically
                if (reflectYAxis) // Y-Axis Reflection
                    map.transform.localScale = new Vector3(-map.transform.localScale.x, map.transform.localScale.y, map.transform.localScale.z); // Reflect the world horizontally

                break;

            // Box Spawner: Spawn a box at the specified spawn point
            case ActivatorEffect.BoxSpawner:
                // Destroy the previous box if it exists
                if (currentBox != null)
                    Destroy(currentBox);

                // Spawn new box
                currentBox = Instantiate(box, boxSpawnPoint.position, Quaternion.identity); // Spawn a box at the specified spawn point

                break;

            default:
                Debug.LogWarning("No activator effect selected!");
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            // Move down if button is above activation height and being pressed
            if (rb.position.y > activationHeight)
                movementDirection = -1;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            // Stop movement if activation height is reached
            if (rb.position.y <= activationHeight)
                movementDirection = 0;
            
            // Move the player/box with the button
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocityY = rb.linearVelocityY;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            // Move up if button is below original height and not being pressed
            if (rb.position.y < originalHeight)
                movementDirection = 1;
        }
    }
}