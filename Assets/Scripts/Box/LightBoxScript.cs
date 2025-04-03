using UnityEngine;

public class LightBoxScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        if (EventManager.instance != null)
        {
            EventManager.instance.onXReflection += FlipGravity;
        }
    }
    void OnDisable()
    {
        if (EventManager.instance != null)
        {
            EventManager.instance.onXReflection -= FlipGravity;
        }
    }

    private void FlipGravity()
    {
        rb.gravityScale = -rb.gravityScale; // Invert the gravity scale
    }
}
