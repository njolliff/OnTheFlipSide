using System.Collections;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public enum ActivatorEffect { WorldReflection, MapReflection, BoxSpawner };
    [SerializeField] private ActivatorEffect activatorEffect;

    // PUBLIC
    public float pressSpeed;
    public bool isFlipped = false, reflectXAxis, reflectYAxis;
    public GameObject box;
    public Transform boxSpawnPoint;

    // PRIVATE
    [SerializeField] private float maxDownDistance;
    [SerializeField] private Rigidbody2D rb;
    private GameObject map, currentBox;
    private Vector3 originalPos, actuationPos;
    private bool wasPressed = false, canActivate = true, isPressed = false;
    private float direction;

    void Start()
    {
        // Store original position
        originalPos = transform.localPosition;

        // Set actuation position based on original position and max down distance
        actuationPos = originalPos;
        actuationPos.y = isFlipped ? actuationPos.y += maxDownDistance : actuationPos.y -= maxDownDistance;

        // Set direction
        direction = isFlipped ? 1f : -1f;

        // Get map
        map = GameObject.Find("Map");

        // Subscribe to events
        if (EventManager.instance != null)
        {
            EventManager.instance.onXReflection += FlipY; // Subscribe to x-axis reflection event
        }
    }

    void OnDisable()
    {
        // Unsubscribe to events
        if (EventManager.instance != null)
        {
            EventManager.instance.onXReflection -= FlipY; // Subscribe to x-axis reflection event
        }
    }

    void FixedUpdate()
    {
        if (isPressed)
        {
            // Move down if button is pressed and not at maximum down distance
            if (transform.localPosition.y > actuationPos.y)
            {
                rb.linearVelocityY = direction * pressSpeed;
            }
            // Activate effect if button is pressed and has reached or gone below the actuation position
            else if (transform.localPosition.y <= actuationPos.y)
            {
                rb.linearVelocityY = 0; // Stop any residual movement
                transform.localPosition = new Vector2(transform.localPosition.x, actuationPos.y); // Ensure it doesn't go below the actuation position

                if (canActivate && !wasPressed)
                {
                    wasPressed = true;
                    StartCoroutine(ActivateEffect());
                }
            }
        }

        else
        {
            // Move up if button is not pressed and not at original position
            if (transform.localPosition.y < originalPos.y)
            {
                rb.linearVelocityY = -direction * pressSpeed;
            }

            else if (transform.localPosition.y >= originalPos.y)
            {
                rb.linearVelocityY = 0; // Stop any residual movement
                transform.localPosition = new Vector2(transform.localPosition.x, originalPos.y); // Ensure it doesn't go above the original position
            }

            // If button was pressed and has raised above the actuation position, activate effect again to reverse reflection
            if (Mathf.Abs(actuationPos.y - transform.localPosition.y) > 0.1f && wasPressed && canActivate)
            {
                wasPressed = false;
                StartCoroutine(ActivateEffect());
            }
        }
    }

    private IEnumerator ActivateEffect()
    {
        canActivate = false;

        switch (activatorEffect)
        {
            case ActivatorEffect.WorldReflection:
                EventManager.instance.WorldReflection(reflectXAxis, reflectYAxis, map);
                break;
            case ActivatorEffect.MapReflection:
                EventManager.instance.MapReflection(reflectXAxis, reflectYAxis, map);
                break;
            case ActivatorEffect.BoxSpawner:
                if (currentBox != null)
                    Destroy(currentBox);
                currentBox = Instantiate(box, boxSpawnPoint.position, Quaternion.identity);
                break;
        }

        yield return new WaitForSeconds(1f);
        canActivate = true;
    }

    private void FlipY()
    {
        isFlipped = !isFlipped;
        direction = -direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            isPressed = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Keep the button pressed if the player is still in contact
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            other.attachedRigidbody.linearVelocityY = rb.linearVelocityY;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            isPressed = false;
        }
    }
}