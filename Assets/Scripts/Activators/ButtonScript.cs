using System.Collections;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public enum ActivatorEffect { WorldReflection, MapReflection, BoxSpawner };
    [SerializeField] private ActivatorEffect activatorEffect;

    // PUBLIC
    public float pressSpeed;
    public bool reflectXAxis, reflectYAxis;
    public GameObject box;
    public Transform boxSpawnPoint;

    // PRIVATE
    private GameObject map, currentBox;
    private Vector3 originalPos;
    private bool isPressed, wasPressed = false, canActivate = true, isFlipped = false;
    [SerializeField] private float maxDownDistance;

    void Start()
    {
        originalPos = transform.localPosition;
        map = GameObject.Find("Map");
    }

    void FixedUpdate()
    {
        float direction = isFlipped ? 1f : -1f; // Reverse movement when flipped
        float targetPosition = isFlipped ? originalPos.y + maxDownDistance : originalPos.y - maxDownDistance; // Adjust target based on flip

        if (isPressed && transform.localPosition.y > targetPosition)
        {
            transform.Translate(0f, direction * (pressSpeed / 100), 0f, Space.Self);
        }
        else if (!isPressed && transform.localPosition.y < originalPos.y)
        {
            transform.Translate(0f, -direction * (pressSpeed / 100), 0f, Space.Self);
        }
        else if (!isPressed && wasPressed && transform.localPosition.y == originalPos.y)
        {
            wasPressed = false;
            StartCoroutine(ActivateEffect());
            isFlipped = !isFlipped;
        }
    }

    private IEnumerator ActivateEffect()
    {
        canActivate = false;

        switch (activatorEffect)
        {
            case ActivatorEffect.WorldReflection:
                EventManager.instance.WorldReflection(reflectXAxis, reflectYAxis, map);
                if (reflectXAxis)
                {
                    isFlipped = !isFlipped;
                    originalPos.y = -originalPos.y;
                }
                break;
            case ActivatorEffect.MapReflection:
                EventManager.instance.MapReflection(reflectXAxis, reflectYAxis, map);
                if (reflectXAxis)
                {
                    isFlipped = !isFlipped;
                    originalPos.y = -originalPos.y;
                }
                break;
            case ActivatorEffect.BoxSpawner:
                if (currentBox != null)
                    Destroy(currentBox);
                currentBox = Instantiate(box, boxSpawnPoint.position, Quaternion.identity);
                break;
        }

        yield return new WaitForSeconds(2f);
        canActivate = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPressed = true;
        }
        else if (other.CompareTag("Actuation Zone") && canActivate)
        {
            StartCoroutine(ActivateEffect());
            wasPressed = true;
            isFlipped = !isFlipped;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPressed = false;
        }
    }
}